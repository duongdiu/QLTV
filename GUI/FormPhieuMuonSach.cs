using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using QLTV_Database;

namespace GUI
{
    public partial class FormPhieuMuonSach : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        private string tenDangNhap;
        public class ComboBoxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }
        public class SachMuonTam
        {
            public string MaSach { get; set; }
            public string TenSach { get; set; }
            public int SoLuongMuon { get; set; }
        }
        private List<SachMuonTam> danhSachTam = new List<SachMuonTam>();
        public FormPhieuMuonSach(string tenDangNhap)
        {
            InitializeComponent();
            dgvChiTietMuon.DataBindingComplete += (s, e) => ResizeRowsToFill();
            this.tenDangNhap = tenDangNhap;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FormPhieuMuonSach_Load(object sender, EventArgs e)
        {

            LoadComboBoxData();
            LoadGrid();
            dtpNgayMuon.ValueChanged += dtpNgayMuon_ValueChanged;
            dgvChiTietMuon.CellClick += dgvChiTietMuon_CellClick;
            dgvDanhSachSachMuon.CellClick += dgvDanhSachSachMuon_CellClick;
        }
        private void LoadComboBoxData()
        {
            try
            {
                var docGias = db.DocGiaDatabase
                    .Select(dg => new ComboBoxItem { Value = dg.MaDocGia, Text = dg.HoTen })
                    .ToList();
                docGias.Insert(0, new ComboBoxItem { Value = "", Text = "-- Chọn độc giả --" });

                cboDocGia.DataSource = docGias;
                cboDocGia.DisplayMember = "Text";
                cboDocGia.ValueMember = "Value";

                var sachs = db.SachDatabase
                    .Select(s => new ComboBoxItem { Value = s.MaSach, Text = s.TenSach })
                    .ToList();
                sachs.Insert(0, new ComboBoxItem { Value = "", Text = "-- Chọn sách --" });

                cboMaSach.DataSource = sachs;
                cboMaSach.DisplayMember = "Text";
                cboMaSach.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnThem_Click(object sender, EventArgs e)
        {

            try
            {
                string maPhieuTuNhap = txtMaPhieuMuon.Text.Trim();
                string newMaPhieu = "";

                if (!string.IsNullOrEmpty(maPhieuTuNhap))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(maPhieuTuNhap, @"^PM\d{2}$"))
                    {
                        MessageBox.Show("Mã phiếu mượn phải có định dạng PM01, PM02, ...");
                        return;
                    }

                    if (db.PhieuMuonDatabase.Any(p => p.MaPhieuMuon == maPhieuTuNhap))
                    {
                        MessageBox.Show("Mã phiếu mượn đã tồn tại. Vui lòng nhập mã khác.");
                        return;
                    }

                    newMaPhieu = maPhieuTuNhap;
                }
                else
                {
                    var lastPhieu = db.PhieuMuonDatabase
                        .OrderByDescending(p => p.MaPhieuMuon)
                        .Select(p => p.MaPhieuMuon)
                        .FirstOrDefault();

                    int nextNumber = 1;
                    if (!string.IsNullOrEmpty(lastPhieu) && lastPhieu.Length > 2)
                    {
                        int.TryParse(lastPhieu.Substring(2), out nextNumber);
                        nextNumber++;
                    }

                    newMaPhieu = "PM" + nextNumber.ToString("D2");
                }

                if (cboDocGia.SelectedIndex <= 0)
                {
                    MessageBox.Show("Vui lòng chọn độc giả.");
                    return;
                }

                if (danhSachTam.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất 1 sách để mượn.");
                    return;
                }

                var pm = new PhieuMuon
                {
                    MaPhieuMuon = newMaPhieu,
                    MaDocGia = cboDocGia.SelectedValue.ToString(),
                    NgayMuon = dtpNgayMuon.Value,
                    NgayTraDuKien = dtpNgayTraDuKien.Value,
                    TinhTrang = "Đang mượn"
                };

                db.PhieuMuonDatabase.Add(pm);

                foreach (var sach in danhSachTam)
                {
                    var ct = new ChiTietPhieuMuon
                    {
                        MaPhieuMuon = newMaPhieu,
                        MaSach = sach.MaSach,
                        SoLuongMuon = sach.SoLuongMuon,
                        TinhTrangSach = ""
                    };
                    db.ChiTietPhieuMuonDatabase.Add(ct);
                }

                db.SaveChanges();
                MessageBox.Show($"Đã lập phiếu mượn: {newMaPhieu} với {danhSachTam.Count} sách.");
                
                txtMaPhieuMuon.Clear();
                cboDocGia.SelectedIndex = 0;
                cboMaSach.SelectedIndex = 0;
                nudSoLuong.Value = 0;
                dtpNgayMuon.Value = DateTime.Now;
                dtpNgayTraDuKien.Value = DateTime.Now.AddDays(7);
                danhSachTam.Clear();
                dgvChiTietMuon.DataSource = null;
                dgvDanhSachSachMuon.DataSource = null;
                dgvDanhSachSachMuon.Refresh();
                nudSoLuong.Value = 0;
                LoadGrid();
                ResizeRowsToFill();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lập phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtpNgayMuon_ValueChanged(object sender, EventArgs e)
        {
            dtpNgayTraDuKien.Value = dtpNgayMuon.Value.AddDays(7);
        }
        private void LoadGrid()
        {
            try
            {
                // Lấy dữ liệu từ CSDL
                var data = (from pm in db.PhieuMuonDatabase
                            join ct in db.ChiTietPhieuMuonDatabase
                                on pm.MaPhieuMuon equals ct.MaPhieuMuon into ctGroup
                            from ct in ctGroup.DefaultIfEmpty()
                            select new
                            {
                                pm.MaPhieuMuon,
                                pm.MaDocGia,
                                ct.MaSach,
                                ct.SoLuongMuon,
                                pm.NgayMuon,
                                pm.NgayTraDuKien,
                                pm.TinhTrang
                            }).ToList();  // Thực hiện truy vấn và tải dữ liệu ra bộ nhớ

                // Nhóm dữ liệu theo mã phiếu mượn và nối các sách mượn thành chuỗi
                var groupedData = data
                    .GroupBy(d => new { d.MaPhieuMuon, d.MaDocGia, d.NgayMuon, d.NgayTraDuKien, d.TinhTrang })
                    .Select(g => new
                    {
                        g.Key.MaPhieuMuon,
                        g.Key.MaDocGia,
                        DanhSachSach = string.Join(", ", g.Where(x => x.MaSach != null)
                                                         .Select(x => x.MaSach + $" (x{x.SoLuongMuon})")),
                        g.Key.NgayMuon,
                        g.Key.NgayTraDuKien,
                        g.Key.TinhTrang
                    }).ToList();  // Nối chuỗi sau khi truy vấn đã hoàn thành

                allData = groupedData.Cast<object>().ToList();
                totalPages = (int)Math.Ceiling((double)allData.Count / pageSize);
                currentPage = 1;
                LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPageData()
        {
            try {
                var pageData = allData
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                dgvChiTietMuon.DataSource = pageData;
                ResizeRowsToFill();

                if (pageData.Count > 0)
                {
                    dgvChiTietMuon.Columns["MaPhieuMuon"].HeaderText = "Mã phiếu";
                    dgvChiTietMuon.Columns["MaDocGia"].HeaderText = "Mã độc giả";
                    dgvChiTietMuon.Columns["DanhSachSach"].HeaderText = "Danh sách sách mượn";
                    dgvChiTietMuon.Columns["NgayMuon"].HeaderText = "Ngày mượn";
                    dgvChiTietMuon.Columns["NgayTraDuKien"].HeaderText = "Ngày trả";
                    dgvChiTietMuon.Columns["TinhTrang"].HeaderText = "Tình trạng";
                    dgvChiTietMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvChiTietMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                lblPage.Text = $"Trang {currentPage}/{(totalPages == 0 ? 1 : totalPages)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResizeRowsToFill()
        {
            int headerHeight = dgvChiTietMuon.ColumnHeadersHeight;
            int availableHeight = dgvChiTietMuon.ClientSize.Height - headerHeight;
            int rowCount = dgvChiTietMuon.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvChiTietMuon.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void LamMoi()
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try {
                string maPhieu = txtMaPhieuMuon.Text.Trim();

                if (string.IsNullOrEmpty(maPhieu))
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn để sửa.");
                    return;
                }

                // Tìm phiếu mượn
                var phieuMuon = db.PhieuMuonDatabase.FirstOrDefault(p => p.MaPhieuMuon == maPhieu);
                var chiTiet = db.ChiTietPhieuMuonDatabase.FirstOrDefault(c => c.MaPhieuMuon == maPhieu);

                if (phieuMuon == null || chiTiet == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn hoặc chi tiết phiếu mượn.");
                    return;
                }

                // Dữ liệu hiện tại trong CSDL
                string oldMaDocGia = phieuMuon.MaDocGia;
                DateTime oldNgayMuon = phieuMuon.NgayMuon ?? DateTime.MinValue;
                DateTime oldNgayTra = phieuMuon.NgayTraDuKien ?? DateTime.MinValue;
                string oldMaSach = chiTiet.MaSach;
                int oldSoLuong = chiTiet.SoLuongMuon ?? 0;

                // Dữ liệu mới từ form
                string newMaDocGia = cboDocGia.SelectedValue?.ToString();
                DateTime newNgayMuon = dtpNgayMuon.Value;
                DateTime newNgayTra = dtpNgayTraDuKien.Value;
                string newMaSach = cboMaSach.SelectedValue?.ToString();
                int newSoLuong = (int)nudSoLuong.Value;

                // Kiểm tra thay đổi
                bool isChanged =
                    oldMaDocGia != newMaDocGia ||
                    oldNgayMuon != newNgayMuon ||
                    oldNgayTra != newNgayTra ||
                    oldMaSach != newMaSach ||
                    oldSoLuong != newSoLuong;

                if (!isChanged)
                {
                    MessageBox.Show("Không có thay đổi nào được thực hiện.");
                    return;
                }

                // Cập nhật nếu có thay đổi
                phieuMuon.MaDocGia = newMaDocGia;
                phieuMuon.NgayMuon = newNgayMuon;
                phieuMuon.NgayTraDuKien = newNgayTra;

                chiTiet.MaSach = newMaSach;
                chiTiet.SoLuongMuon = newSoLuong;

                db.SaveChanges();
                MessageBox.Show("Đã cập nhật phiếu mượn!");
                LoadGrid();
                ResizeRowsToFill();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var maPhieu = txtMaPhieuMuon.Text;

                if (string.IsNullOrWhiteSpace(maPhieu))
                {
                    MessageBox.Show("Vui lòng chọn một phiếu mượn để xóa.");
                    return;
                }

                var confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phiếu mượn này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    var ct = db.ChiTietPhieuMuonDatabase.Where(x => x.MaPhieuMuon == maPhieu).ToList();
                    db.ChiTietPhieuMuonDatabase.RemoveRange(ct);

                    var pm = db.PhieuMuonDatabase.FirstOrDefault(x => x.MaPhieuMuon == maPhieu);
                    if (pm != null)
                    {
                        db.PhieuMuonDatabase.Remove(pm);
                        db.SaveChanges();
                        MessageBox.Show("Đã xóa thành công!");
                        LoadGrid();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy phiếu mượn để xóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                txtMaPhieuMuon.Clear();
                cboDocGia.SelectedIndex = 0;
                cboMaSach.SelectedIndex = 0;
                nudSoLuong.Value = 0;
                dtpNgayMuon.Value = DateTime.Now;
                dtpNgayTraDuKien.Value = DateTime.Now.AddDays(7);
                danhSachTam.Clear();
                dgvChiTietMuon.DataSource = null;
                dgvDanhSachSachMuon.DataSource = null;
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvChiTietMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                if (e.RowIndex < 0) return;

                // 1. Lấy dữ liệu từ dòng được click
                var row = dgvChiTietMuon.Rows[e.RowIndex];
                string maPhieu = row.Cells["MaPhieuMuon"].Value.ToString();
                string maDocGia = row.Cells["MaDocGia"].Value.ToString();
                DateTime ngayMuon = Convert.ToDateTime(row.Cells["NgayMuon"].Value);
                DateTime ngayTra = Convert.ToDateTime(row.Cells["NgayTraDuKien"].Value);

                // 2. Điền vào các control trên form
                txtMaPhieuMuon.Text = maPhieu;
                cboDocGia.SelectedValue = maDocGia;
                dtpNgayMuon.Value = ngayMuon;
                dtpNgayTraDuKien.Value = ngayTra;

                danhSachTam = db.ChiTietPhieuMuonDatabase
                  .Where(ct => ct.MaPhieuMuon == maPhieu)
                  .Select(ct => new SachMuonTam
                  {
                      MaSach = ct.MaSach,
                      TenSach = db.SachDatabase
                                        .Where(s => s.MaSach == ct.MaSach)
                                        .Select(s => s.TenSach)
                                        .FirstOrDefault(),
                      SoLuongMuon = ct.SoLuongMuon ?? 0       // nếu ct.SoLuongMuon == null thì lấy 0
                  })
                  .ToList();

                // 4. Đổ xuống DataGridView danh sách sách mượn
                LoadDanhSachSachMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void btnInPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = txtMaPhieuMuon.Text.Trim();
                if (string.IsNullOrEmpty(maPhieu))
                {
                    MessageBox.Show("Vui lòng chọn phiếu mượn để in.");
                    return;
                }

                var phieu = db.PhieuMuonDatabase.FirstOrDefault(p => p.MaPhieuMuon == maPhieu);
                var chiTiet = db.ChiTietPhieuMuonDatabase
                    .Where(c => c.MaPhieuMuon == maPhieu)
                    .ToList();

                if (phieu == null || chiTiet.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn hoặc chi tiết phiếu.");
                    return;
                }

                var tenDocGia = db.DocGiaDatabase
                    .Where(d => d.MaDocGia == phieu.MaDocGia)
                    .Select(d => d.HoTen)
                    .FirstOrDefault();

                // Lấy tên người lập phiếu từ tên đăng nhập
                var tenNguoiLap = db.NguoiDungDatabase
                    .Where(nd => nd.TenDangNhap == this.tenDangNhap)  // 'this.tenDangNhap' được truyền từ form cha
                    .Select(nd => nd.HoTen)
                    .FirstOrDefault();

                // Tạo file PDF
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"PhieuMuon_{maPhieu}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Document doc = new Document(PageSize.A4);
                    PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));

                    // 🔤 Sử dụng font Unicode Arial để hiển thị tiếng Việt
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    iTextSharp.text.Font unicodeFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);

                    doc.Open();

                    // Tiêu đề
                    Paragraph title = new Paragraph("PHIẾU MƯỢN SÁCH", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(new Paragraph("\n", unicodeFont));

                    // Thông tin chung
                    doc.Add(new Paragraph($"Mã phiếu mượn: {phieu.MaPhieuMuon}", unicodeFont));
                    doc.Add(new Paragraph($"Độc giả: {tenDocGia} ({phieu.MaDocGia})", unicodeFont));
                    doc.Add(new Paragraph($"Ngày mượn: {phieu.NgayMuon:dd/MM/yyyy}", unicodeFont));
                    doc.Add(new Paragraph($"Ngày trả dự kiến: {phieu.NgayTraDuKien:dd/MM/yyyy}", unicodeFont));
                    doc.Add(new Paragraph("\n", unicodeFont));

                    // Thêm phần người lập phiếu
                    doc.Add(new Paragraph($"Người lập phiếu: {tenNguoiLap}", unicodeFont));
                    doc.Add(new Paragraph("\n", unicodeFont));

                    // Chi tiết sách mượn
                    doc.Add(new Paragraph("Chi tiết sách mượn:", unicodeFont));

                    // Tạo bảng với 4 cột: STT, Tên sách, Mã sách, Số lượng mượn
                    PdfPTable table = new PdfPTable(4); // 4 cột
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 12f;
                    table.SpacingAfter = 12f;

                    // Đặt tiêu đề cho các cột
                    table.AddCell(new PdfPCell(new Phrase("STT", unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Tên sách", unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Mã sách", unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Số lượng mượn", unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    // Thêm các dòng chi tiết sách mượn vào bảng
                    int stt = 1;
                    foreach (var ct in chiTiet)
                    {
                        var sach = db.SachDatabase.FirstOrDefault(s => s.MaSach == ct.MaSach);
                        string tenSach = sach != null ? sach.TenSach : ct.MaSach;

                        table.AddCell(new PdfPCell(new Phrase(stt.ToString(), unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        table.AddCell(new PdfPCell(new Phrase(tenSach, unicodeFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        table.AddCell(new PdfPCell(new Phrase(ct.MaSach, unicodeFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        table.AddCell(new PdfPCell(new Phrase(ct.SoLuongMuon.ToString(), unicodeFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        stt++;
                    }


                    // Thêm bảng vào tài liệu
                    doc.Add(table);

                    doc.Add(new Paragraph("\n", unicodeFont));

                    doc.Add(new Paragraph("\n-------------------------------------------", unicodeFont) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Ngày in: {DateTime.Now:dd/MM/yyyy}", unicodeFont) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();

                    MessageBox.Show("Xuất file PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPageData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadPageData();
        }

        private void btnThemSach_Click(object sender, EventArgs e)
        {
            try {
                string maSach = cboMaSach.SelectedValue?.ToString();
                string tenSach = cboMaSach.Text;
                int soLuong = (int)nudSoLuong.Value;

                if (string.IsNullOrWhiteSpace(maSach))
                {
                    MessageBox.Show("Vui lòng chọn sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng mượn phải lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra sách đã có trong danh sách tạm hay chưa
                var sachDaCo = danhSachTam.FirstOrDefault(s => s.MaSach == maSach);
                if (sachDaCo != null)
                {
                    sachDaCo.SoLuongMuon += soLuong;
                }
                else
                {
                    danhSachTam.Add(new SachMuonTam
                    {
                        MaSach = maSach,
                        TenSach = tenSach,
                        SoLuongMuon = soLuong
                    });
                }

                LoadDanhSachSachMuon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDanhSachSachMuon()
        {
            try {
                dgvDanhSachSachMuon.DataSource = null;
                dgvDanhSachSachMuon.DataSource = danhSachTam;

                dgvDanhSachSachMuon.Columns["MaSach"].HeaderText = "Mã sách";
                dgvDanhSachSachMuon.Columns["TenSach"].HeaderText = "Tên sách";
                dgvDanhSachSachMuon.Columns["SoLuongMuon"].HeaderText = "Số lượng";

                dgvDanhSachSachMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDanhSachSachMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvDanhSachSachMuon.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvDanhSachSachMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                if (e.RowIndex >= 0 && MessageBox.Show("Bạn có chắc muốn xóa sách này khỏi danh sách?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string maSach = dgvDanhSachSachMuon.Rows[e.RowIndex].Cells["MaSach"].Value.ToString();
                    danhSachTam.RemoveAll(s => s.MaSach == maSach);
                    LoadDanhSachSachMuon();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
