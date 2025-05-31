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
    public partial class ChiTietMuonTraSach : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData = new List<object>();
        private string tenDangNhap;
        public ChiTietMuonTraSach(string tenDangNhap)
        {
            InitializeComponent();
            this.tenDangNhap = tenDangNhap;

            dgvChiTiet.CellClick += dgvChiTiet_CellClick;
            dgvChiTiet.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = cboMaPhieuMuon.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(maPhieu)) return;

                if (dgvChiTiet.SelectedRows.Count > 0)
                {
                    string maSach = dgvChiTiet.SelectedRows[0].Cells["MaSach"].Value?.ToString();
                    var ct = db.ChiTietPhieuMuonDatabase.FirstOrDefault(c => c.MaPhieuMuon == maPhieu && c.MaSach == maSach);
                    if (ct != null)
                    {
                        db.ChiTietPhieuMuonDatabase.Remove(ct);
                        db.SaveChanges();
                        MessageBox.Show("Đã xóa sách khỏi phiếu mượn.");
                        cboMaPhieuMuon_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sách khỏi phiếu mượn:\n" + ex.Message);
            }
        }

        private void ChiTietMuonTraSach_Load(object sender, EventArgs e)
        {
            LoadTinhTrangSach();
            LoadMaPhieuMuon();
            if (cboMaPhieuMuon.Items.Count > 0)
                cboMaPhieuMuon.SelectedIndex = 0;
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvChiTiet.ColumnHeadersHeight;
            int availableHeight = dgvChiTiet.ClientSize.Height - headerHeight;
            int rowCount = dgvChiTiet.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void LoadMaPhieuMuon()
        {
            var phieus = db.PhieuMuonDatabase
        .Select(p => p.MaPhieuMuon)
        .ToList();
            cboMaPhieuMuon.DataSource = phieus;
        }
        private void LoadTinhTrangSach()
        {
            cboTinhTrang.Items.Clear();
            cboTinhTrang.Items.AddRange(new string[]
            {
                "Bình thường",
                "Rách nhẹ",
                "Hỏng",
                "Mất"
            });
        }

        private void LoadChiTiet()
        {
            
            // Truy vấn chi tiết sách kèm thông tin phạt từ PhieuMuon
            string maPhieu = cboMaPhieuMuon.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(maPhieu))
            {
                dgvChiTiet.DataSource = null;
                return;
            }

            var data = (from ct in db.ChiTietPhieuMuonDatabase
                        join pm in db.PhieuMuonDatabase
                            on ct.MaPhieuMuon equals pm.MaPhieuMuon
                        where ct.MaPhieuMuon == maPhieu
                        select new
                        {
                            ct.MaPhieuMuon,
                            ct.MaSach,
                         
                            SoLuongMuon = ct.SoLuongMuon ?? 0,
                            TinhTrangSach = ct.TinhTrangSach,
                            TienPhat = pm.TienPhat ?? 0m,
                            TinhTrang = pm.TinhTrang,
                            NgayTraDuKien = pm.NgayTraDuKien,
                            NgayTra = ct.NgayTra // Giữ nguyên kiểu DateTime? mà không chuyển đổi
                        })
           .ToList();

            // Chuyển đổi kiểu sau khi truy vấn dữ liệu về (Không sử dụng ToString() trong LINQ)
            var formattedData = data.Select(x => new
            {
                x.MaPhieuMuon,
                x.MaSach,
               
                x.SoLuongMuon,
                TinhTrangSach = string.IsNullOrEmpty(x.TinhTrangSach) ? "Chưa cập nhật" : x.TinhTrangSach,
                TienPhat = x.TienPhat,
                NgayTra = x.NgayTra.HasValue
                          ? x.NgayTra.Value.ToString("dd/MM/yyyy") // Chuyển đổi ngày sau khi dữ liệu đã được lấy ra
                          : "Chưa trả",
                TinhTrang = x.TinhTrang == "Đã trả" ? "Đã trả" :
                            (x.NgayTraDuKien.HasValue && DateTime.Now > x.NgayTraDuKien.Value && x.TinhTrang != "Đã trả" ? "Quá hạn" : "Đang mượn")
            }).ToList();

            dgvChiTiet.DataSource = formattedData;
        }
        private void LoadPageData()
        {
            var pageData = allData
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dgvChiTiet.DataSource = pageData;
            ResizeRowsToFill();
            dgvChiTiet.Columns["MaPhieuMuon"].HeaderText = "Mã phiếu mượn";
            dgvChiTiet.Columns["MaSach"].HeaderText = "Mã sách";
            dgvChiTiet.Columns["SoLuongMuon"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["TinhTrangSach"].HeaderText = "Tình trạng sách";
            dgvChiTiet.Columns["TienPhat"].HeaderText = "Tiền phạt";
            dgvChiTiet.Columns["NgayTra"].HeaderText = "Ngày trả";
            dgvChiTiet.Columns["TinhTrang"].HeaderText = "Tình trạng";

            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            lblPage.Text = $"Trang {currentPage}/{totalPages}";
        }
        private void LoadTatCaMaPhieuMuon()
        {
            var danhSachPhieu = db.PhieuMuonDatabase.Select(pm => pm.MaPhieuMuon).ToList();
            cboMaPhieuMuon.DataSource = danhSachPhieu;
        }

        

        private void cboMaPhieuMuon_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            
        }

        private void btnTraSach_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = cboMaPhieuMuon.SelectedItem?.ToString();
                string maSach = txtSelectedMaSach.Text;

                if (string.IsNullOrEmpty(maPhieu) || string.IsNullOrEmpty(maSach))
                {
                    MessageBox.Show("Vui lòng chọn phiếu và sách để trả.");
                    return;
                }

                var phieu = db.PhieuMuonDatabase.FirstOrDefault(p => p.MaPhieuMuon == maPhieu);
                if (phieu == null)
                {
                    MessageBox.Show("Không tìm thấy phiếu mượn.");
                    return;
                }

                int returnQty = (int)nudReturnQty.Value;

                var ct = db.ChiTietPhieuMuonDatabase
                            .FirstOrDefault(c => c.MaPhieuMuon == maPhieu && c.MaSach == maSach);
                if (ct == null)
                {
                    MessageBox.Show("Không tìm thấy chi tiết mượn.");
                    return;
                }

                if (returnQty <= 0 || returnQty > ct.SoLuongMuon)
                {
                    MessageBox.Show("Số lượng trả không hợp lệ.");
                    return;
                }

                var sach = db.SachDatabase.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null)
                {
                    MessageBox.Show("Không tìm thấy sách với mã " + maSach);
                    return;
                }

                string tinhTrangTra = cboTinhTrang.SelectedItem?.ToString() ?? "Bình thường";
                ct.TinhTrangSach = tinhTrangTra;

                DateTime ngayTra = DateTime.Now;
                DateTime ngayHenTra = phieu.NgayTraDuKien ?? phieu.NgayMuon.Value.AddDays(7);
                bool isOverdue = ngayTra > ngayHenTra;

                decimal tienPhat = 0m;
                if (isOverdue)
                {
                    int ngayTre = (ngayTra - ngayHenTra).Days;
                    tienPhat += ngayTre * 5000m * returnQty;
                }

                switch (tinhTrangTra)
                {
                    case "Rách nhẹ":
                        tienPhat += 10000m * returnQty;
                        break;
                    case "Hỏng":
                    case "Mất":
                        tienPhat += (sach.GiaTien ?? 0m) * returnQty;
                        break;
                }

              

                // ✅ Cập nhật ngày trả và phạt
                ct.NgayTra = ngayTra;
                phieu.TienPhat = (phieu.TienPhat ?? 0m) + tienPhat;

                if (tinhTrangTra == "Hỏng" || tinhTrangTra == "Mất")
                {
                    sach.SoLuong -= returnQty;
                    if (sach.SoLuong < 0) sach.SoLuong = 0; // đảm bảo không âm
                }
                // ✅ Lưu trước để đảm bảo NgayTra được cập nhật
                db.SaveChanges();

                // ✅ Kiểm tra lại sau khi đã lưu
                bool conSachChuaTra = db.ChiTietPhieuMuonDatabase
                    .Where(c => c.MaPhieuMuon == maPhieu)
                    .Any(c => c.NgayTra == null);

                if (!conSachChuaTra)
                {
                    phieu.TinhTrang = "Đã trả";
                    db.SaveChanges(); // Lưu lần 2 nếu có thay đổi tình trạng
                }

                MessageBox.Show($"Trả thành công {returnQty} cuốn.\nTiền phạt: {tienPhat:N0} đ", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadMaPhieuMuon();
                cboMaPhieuMuon.SelectedItem = maPhieu;
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboMaPhieuMuon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboMaPhieuMuon.SelectedItem == null) return;

                string selectedMaPhieuMuon = cboMaPhieuMuon.SelectedItem.ToString();

                // Truy vấn dữ liệu từ database
                var rawData = (from ct in db.ChiTietPhieuMuonDatabase
                               join pm in db.PhieuMuonDatabase on ct.MaPhieuMuon equals pm.MaPhieuMuon
                               where ct.MaPhieuMuon == selectedMaPhieuMuon
                               select new
                               {
                                   ct.MaPhieuMuon,
                                   ct.MaSach,
                                   SoLuongMuon = ct.SoLuongMuon ?? 0,
                                   TinhTrangSach = ct.TinhTrangSach,
                                   TienPhat = pm.TienPhat,
                                   NgayTra = ct.NgayTra,
                                   NgayTraDuKien = pm.NgayTraDuKien,
                                   TinhTrang = pm.TinhTrang
                               }).ToList(); // <-- ToList() trước để xử lý dữ liệu trong bộ nhớ

                // Định dạng lại dữ liệu sau khi lấy về
                allData = rawData.Select(x => new
                {
                    x.MaPhieuMuon,
                    x.MaSach,
                    x.SoLuongMuon,
                    TinhTrangSach = string.IsNullOrEmpty(x.TinhTrangSach) ? "Chưa cập nhật" : x.TinhTrangSach,
                    TienPhat = x.TienPhat ?? 0,
                    NgayTra = x.NgayTra.HasValue ? x.NgayTra.Value.ToString("dd/MM/yyyy") : "Chưa trả",
                    TinhTrang = x.TinhTrang == "Đã trả" ? "Đã trả" :
                        (x.NgayTraDuKien.HasValue && DateTime.Now > x.NgayTraDuKien.Value && x.TinhTrang != "Đã trả" ? "Quá hạn" : "Đang mượn")
                }).Cast<object>().ToList();

                // Cập nhật phân trang
                currentPage = 1;
                totalPages = (int)Math.Ceiling(allData.Count / (double)pageSize);
                LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi tải dữ liệu phiếu mượn:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = cboMaPhieuMuon.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(maPhieu)) return;

                if (dgvChiTiet.SelectedRows.Count > 0)
                {
                    string maSach = dgvChiTiet.SelectedRows[0].Cells["MaSach"].Value?.ToString();
                    var ct = db.ChiTietPhieuMuonDatabase.FirstOrDefault(c => c.MaPhieuMuon == maPhieu && c.MaSach == maSach);
                    if (ct != null)
                    {
                        ct.TinhTrangSach = cboTinhTrang.SelectedItem?.ToString() ?? "Bình thường";
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật tình trạng thành công.");
                        cboMaPhieuMuon_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tình trạng sách:\n" + ex.Message);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

            try
            {
                cboTinhTrang.SelectedIndex = -1;
                txtSelectedMaSach.Clear();
                nudReturnQty.Value = 1;
                LoadChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới:\n" + ex.Message);
            }

        }

        private void dgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var row = dgvChiTiet.Rows[e.RowIndex];
                string maSach = row.Cells["MaSach"].Value?.ToString();

                if (string.IsNullOrEmpty(maSach))
                {
                    MessageBox.Show("Không có mã sách trong dòng được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtSelectedMaSach.Text = maSach;

                // Gán số lượng mượn vào NumericUpDown
                if (int.TryParse(row.Cells["SoLuongMuon"].Value?.ToString(), out int soLuong) && soLuong > 0)
                {
                    nudReturnQty.Maximum = soLuong;
                    nudReturnQty.Value = soLuong;
                }
                else
                {
                    MessageBox.Show("Số lượng mượn không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gán tình trạng nếu đã có, nếu chưa thì mặc định "Bình thường"
                string tinhTrang = row.Cells["TinhTrangSach"].Value?.ToString();
                cboTinhTrang.SelectedItem = string.IsNullOrEmpty(tinhTrang) ? "Bình thường" : tinhTrang;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng chi tiết:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = cboMaPhieuMuon.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(maPhieu)) return;

                var phieu = db.PhieuMuonDatabase.FirstOrDefault(p => p.MaPhieuMuon == maPhieu);
                if (phieu == null) return;

                var chiTietList = db.ChiTietPhieuMuonDatabase.Where(c => c.MaPhieuMuon == maPhieu).ToList();
                if (chiTietList.Count == 0)
                {
                    MessageBox.Show("Không có chi tiết mượn sách.");
                    return;
                }

                var tenDocGia = db.DocGiaDatabase
                    .Where(d => d.MaDocGia == phieu.MaDocGia)
                    .Select(d => d.HoTen)
                    .FirstOrDefault();
                var tenNguoiLap = db.NguoiDungDatabase
                   .Where(nd => nd.TenDangNhap == this.tenDangNhap)
                   .Select(nd => nd.HoTen)
                   .FirstOrDefault();

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"PhieuTraSach_{maPhieu}.pdf"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Document doc = new Document(PageSize.A4, 50, 50, 60, 40);
                    PdfWriter.GetInstance(doc, new FileStream(saveFileDialog.FileName, FileMode.Create));

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    var titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    var normalFont = new iTextSharp.text.Font(baseFont, 12);

                    doc.Open();

                    Paragraph title = new Paragraph("PHIẾU TRẢ SÁCH", titleFont) { Alignment = Element.ALIGN_CENTER };
                    doc.Add(title);
                    doc.Add(new Paragraph("\n", normalFont));

                    doc.Add(new Paragraph($"Mã phiếu mượn   : {phieu.MaPhieuMuon}", normalFont));
                    doc.Add(new Paragraph($"Độc giả         : {tenDocGia} ({phieu.MaDocGia})", normalFont));
                    doc.Add(new Paragraph($"Ngày trả        : {DateTime.Now:dd/MM/yyyy}", normalFont));
                    doc.Add(new Paragraph($"Tình trạng      : {phieu.TinhTrang}", normalFont));
                    doc.Add(new Paragraph($"Tiền phạt       : {phieu.TienPhat:N0} đ", normalFont));
                    doc.Add(new Paragraph("\n", normalFont));

                    doc.Add(new Paragraph("Danh sách sách trả:", normalFont));

                    PdfPTable table = new PdfPTable(5) { WidthPercentage = 100, SpacingBefore = 12f, SpacingAfter = 12f };

                    table.AddCell(new PdfPCell(new Phrase("STT", titleFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                    table.AddCell(new PdfPCell(new Phrase("Tên sách", titleFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                    table.AddCell(new PdfPCell(new Phrase("Mã sách", titleFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                    table.AddCell(new PdfPCell(new Phrase("Số lượng", titleFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                    table.AddCell(new PdfPCell(new Phrase("Tình trạng", titleFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });

                    int stt = 1;
                    foreach (var ct in chiTietList)
                    {
                        var sach = db.SachDatabase.FirstOrDefault(s => s.MaSach == ct.MaSach);
                        string tenSach = sach?.TenSach ?? "(Không rõ)";
                        string maSach = ct.MaSach;
                        string soLuong = ct.SoLuongMuon.ToString();
                        string tinhTrang = ct.TinhTrangSach;

                        table.AddCell(new PdfPCell(new Phrase(stt.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                        table.AddCell(new PdfPCell(new Phrase(tenSach, normalFont)) { HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 1 });
                        table.AddCell(new PdfPCell(new Phrase(maSach, normalFont)) { HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 1 });
                        table.AddCell(new PdfPCell(new Phrase(soLuong, normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1 });
                        table.AddCell(new PdfPCell(new Phrase(tinhTrang, normalFont)) { HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 1 });

                        stt++;
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph("\n", normalFont));
                    doc.Add(new Paragraph("\n-------------------------------------------", normalFont) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Ngày in: {DateTime.Now:dd/MM/yyyy}", normalFont) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new Paragraph($"Người lập phiếu: {tenNguoiLap}", normalFont) { Alignment = Element.ALIGN_CENTER });

                    doc.Close();

                    MessageBox.Show("Đã in phiếu trả sách ra PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi in phiếu trả sách:\n" + ex.Message);
            }
        }



        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    LoadPageData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển trang trước:\n" + ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentPage < totalPages)
                {
                    currentPage++;
                    LoadPageData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển trang sau:\n" + ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
