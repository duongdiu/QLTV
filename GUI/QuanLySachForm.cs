using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using QLTV_DTO;

namespace GUI
{
    public partial class QuanLySachForm : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private QLTV_Database.Sach currentBook;
        private int currentPage = 1;
        private int pageSize = 5;    // Số dòng mỗi trang
        private int totalPages = 1;

        public QuanLySachForm()
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadDanhSachSach();
            dgvSach.CellClick += dgvSach_CellClick;
            dgvSach.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadDanhSachSach(string tuKhoa = "")
        {
            try
            {
                // 1. Lấy query gốc, chưa ToList()
                var query = db.SachDatabase
                    .Select(s => new
                    {
                        s.MaSach,
                        s.TenSach,
                        TacGia = db.TacGiaDatabase.FirstOrDefault(t => t.MaTacGia == s.MaTacGia).TenTacGia,
                        TheLoai = db.TheLoaiDatabase.FirstOrDefault(tl => tl.MaTheLoai == s.MaTheLoai).TenTheLoai,
                        NhaXB = db.NhaXuatBanDatabase.FirstOrDefault(nxb => nxb.MaNXB == s.MaNXB).TenNXB,
                        s.NamXuatBan,
                        s.SoLuong,
                        s.SoLanTaiBan,
                        s.GiaTien,
                        SoLuotMuon = db.ChiTietPhieuMuonDatabase
                       .Count(ct => ct.MaSach == s.MaSach),
                        SoLuongConLai = s.SoLuong - (
        db.ChiTietPhieuMuonDatabase
            .Where(ct => ct.MaSach == s.MaSach && ct.PhieuMuon.TinhTrang != "Đã trả")
            .Sum(ct => (int?)ct.SoLuongMuon) ?? 0),
                        ViTri = db.ViTriDatabase.FirstOrDefault(v => v.MaViTri == s.MaViTri).Kho + " - " +
                db.ViTriDatabase.FirstOrDefault(v => v.MaViTri == s.MaViTri).Tu + " - " +
                db.ViTriDatabase.FirstOrDefault(v => v.MaViTri == s.MaViTri).Ke + " - " +
                db.ViTriDatabase.FirstOrDefault(v => v.MaViTri == s.MaViTri).Tang,
                        s.AnhBia
                    })
                    .AsQueryable();

                // 2. Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(s =>
                        s.TenSach.ToLower().Contains(tuKhoa) ||
                        s.TacGia.ToLower().Contains(tuKhoa) ||
                        s.TheLoai.ToLower().Contains(tuKhoa) ||
                        s.NhaXB.ToLower().Contains(tuKhoa));
                }

                // 3. Tính tổng bản ghi và tổng trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 4. Đảm bảo currentPage nằm trong [1…totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // 5. Lấy dữ liệu trang hiện tại
                var pageData = query
                    .OrderBy(s => s.MaSach)      // Bắt buộc OrderBy trước Skip
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 6. Bind vào DataGridView
                dgvSach.DataSource = pageData;
                ResizeRowsToFill();

                // 7. Format lại các cột
                dgvSach.Columns["MaSach"].HeaderText = "Mã Sách";
                dgvSach.Columns["TenSach"].HeaderText = "Tên Sách";
                dgvSach.Columns["TacGia"].HeaderText = "Tác Giả";
                dgvSach.Columns["TheLoai"].HeaderText = "Thể Loại";
                dgvSach.Columns["NhaXB"].HeaderText = "Nhà XB";
                dgvSach.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
                dgvSach.Columns["SoLanTaiBan"].HeaderText = "Số Lần Tái Bản";
                dgvSach.Columns["SoLuong"].HeaderText = "Tổng số lượng";
                dgvSach.Columns["SoLuongConLai"].HeaderText = "Số sách trong kho";
                dgvSach.Columns["SoLuotMuon"].HeaderText = "Số Lượt Mượn";
                dgvSach.Columns["GiaTien"].HeaderText = "Giá Tiền";
                dgvSach.Columns["ViTri"].HeaderText = "Vị Trí";
                dgvSach.Columns["AnhBia"].Visible = false;  // Ẩn cột ảnh bìa  

                dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSach.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 8. Cập nhật label hiển thị trang (giả sử bạn có lblPage)
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResizeRowsToFill()
        {
            int headerHeight = dgvSach.ColumnHeadersHeight;
            int availableHeight = dgvSach.ClientSize.Height - headerHeight;
            int rowCount = dgvSach.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // Đảm bảo rằng người dùng đã chọn hàng hợp lệ
                {
                    // Lấy thông tin sách từ hàng được chọn
                    var maSach = dgvSach.Rows[e.RowIndex].Cells["MaSach"].Value.ToString();
                    currentBook = db.SachDatabase.Find(maSach); // Tìm sách trong cơ sở dữ liệu

                    if (currentBook != null)
                    {
                        // Cập nhật thông tin vào các điều khiển
                        txtMaSach.Text = currentBook.MaSach;
                        txtTenSach.Text = currentBook.TenSach;
                        cbTacGia.SelectedValue = currentBook.MaTacGia;
                        cbTheLoai.SelectedValue = currentBook.MaTheLoai;
                        cbNXB.SelectedValue = currentBook.MaNXB;
                        txtNamXB.Text = currentBook.NamXuatBan.ToString();
                        txtSoLanTaiBan.Text = currentBook.SoLanTaiBan.ToString() ?? "";
                        numericSoLuong.Value = currentBook.SoLuong;
                        txtGia.Text = currentBook.GiaTien.HasValue
        ? currentBook.GiaTien.Value.ToString("0.##")
        : "";
                        cbViTri.SelectedValue = currentBook.MaViTri ?? "";

                        txtAnhBia.Text = currentBook.AnhBia;


                        // Hiển thị ảnh bìa
                        var anhBiaPath = dgvSach.Rows[e.RowIndex].Cells["AnhBia"].Value?.ToString();

                        if (!string.IsNullOrEmpty(anhBiaPath) && System.IO.File.Exists(anhBiaPath))
                        {
                            try
                            {
                                // Đọc file ảnh mà không bị lỗi do file đang mở
                                using (var stream = new System.IO.FileStream(anhBiaPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                                {
                                    picBiaSach.Image = Image.FromStream(stream);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Không thể hiển thị ảnh bìa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                picBiaSach.Image = null;
                            }
                        }
                        else
                        {
                            picBiaSach.Image = null;
                            MessageBox.Show("Không tìm thấy ảnh bìa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi click: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadComboBoxes()
        {
            // Load Tác Giả
            var listTacGia = db.TacGiaDatabase.ToList();
            listTacGia.Insert(0, new QLTV_Database.TacGia { MaTacGia = "", TenTacGia = "-- Chọn tác giả --" });
            cbTacGia.DataSource = listTacGia;
            cbTacGia.DisplayMember = "TenTacGia";
            cbTacGia.ValueMember = "MaTacGia";

            // Load Nhà Xuất Bản
            var listNXB = db.NhaXuatBanDatabase.ToList();
            listNXB.Insert(0, new QLTV_Database.NhaXuatBan { MaNXB = "", TenNXB = "-- Chọn NXB --" });
            cbNXB.DataSource = listNXB;
            cbNXB.DisplayMember = "TenNXB";
            cbNXB.ValueMember = "MaNXB";

            // Load Thể Loại
            var listTheLoai = db.TheLoaiDatabase.ToList();
            listTheLoai.Insert(0, new QLTV_Database.TheLoai { MaTheLoai = "", TenTheLoai = "-- Chọn thể loại --" });
            cbTheLoai.DataSource = listTheLoai;
            cbTheLoai.DisplayMember = "TenTheLoai";
            cbTheLoai.ValueMember = "MaTheLoai";

            // Load Vị Trí
            var listViTri = db.ViTriDatabase
                .Select(v => new
                {
                    MaViTri = v.MaViTri,
                    TenViTri = v.Kho + " - " + v.Tu + " - " + v.Ke + " - " + v.Tang
                })
                .ToList();

            // Thêm dòng "-- Chọn vị trí --" vào đầu
            listViTri.Insert(0, new
            {
                MaViTri = "",
                TenViTri = "-- Chọn vị trí --"
            });

            cbViTri.DataSource = listViTri;
            cbViTri.DisplayMember = "TenViTri";
            cbViTri.ValueMember = "MaViTri";

            // Chọn mặc định
            cbTacGia.SelectedIndex = 0;
            cbNXB.SelectedIndex = 0;
            cbTheLoai.SelectedIndex = 0;
            cbViTri.SelectedIndex = 0;
        }


        private void picBiaSach_Click(object sender, EventArgs e)
        {

        }

        private void txtMaSach_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenSach_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbNXB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbTacGia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtNamXB_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbTheLoai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericSoLuong_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAnhBia_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Chọn ảnh bìa";
                    ofd.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                    ofd.Multiselect = false;

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFilePath = ofd.FileName; // Lấy đường dẫn file đã chọn
                        try
                        {
                            // Load ảnh vào PictureBox
                            picBiaSach.Image = Image.FromFile(selectedFilePath);

                            // Lưu đường dẫn ảnh vào một biến hoặc cơ sở dữ liệu
                            txtAnhBia.Text = selectedFilePath; // Giả sử có một TextBox để lưu đường dẫn
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi upload: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private string TaoMaSachTuDong()
        {
            var lastBook = db.SachDatabase
                .OrderByDescending(s => s.MaSach)
                .FirstOrDefault();

            if (lastBook == null || string.IsNullOrEmpty(lastBook.MaSach))
                return "S001";

            string so = lastBook.MaSach.Substring(1); // Bỏ chữ S
            int soMoi = int.Parse(so) + 1;
            return "S" + soMoi.ToString("D3"); // D3 = định dạng 3 chữ số, ví dụ: 001
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maSachMoi = TaoMaSachTuDong(); // ← Sinh mã sách mới

                currentBook = new QLTV_Database.Sach
                {
                    MaSach = maSachMoi,
                    TenSach = txtTenSach.Text,
                    MaTacGia = cbTacGia.SelectedValue.ToString(),
                    MaTheLoai = cbTheLoai.SelectedValue.ToString(),
                    MaNXB = cbNXB.SelectedValue.ToString(),
                    NamXuatBan = int.Parse(txtNamXB.Text),
                    SoLanTaiBan = int.Parse(txtSoLanTaiBan.Text),
                    SoLuong = (int)numericSoLuong.Value,
                    GiaTien = decimal.Parse(txtGia.Text),
                    MaViTri = cbViTri.SelectedValue.ToString(),
                    AnhBia = txtAnhBia.Text
                };

                db.SachDatabase.Add(currentBook);
                db.SaveChanges();
                MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSach();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBook == null)
                {
                    // Nếu không có sách hiện tại, thì thêm mới
                    currentBook = new QLTV_Database.Sach
                    {
                        MaSach = txtMaSach.Text,
                        TenSach = txtTenSach.Text,
                        MaTacGia = db.TacGiaDatabase.First(t => t.TenTacGia == cbTacGia.Text).MaTacGia,
                        MaTheLoai = db.TheLoaiDatabase.First(tl => tl.TenTheLoai == cbTheLoai.Text).MaTheLoai,
                        MaNXB = db.NhaXuatBanDatabase.First(nxb => nxb.TenNXB == cbNXB.Text).MaNXB,
                        NamXuatBan = int.Parse(txtNamXB.Text),
                        SoLanTaiBan = int.Parse(txtSoLanTaiBan.Text),
                        SoLuong = (int)numericSoLuong.Value,
                        GiaTien = decimal.Parse(txtGia.Text),
                        MaViTri = cbViTri.SelectedValue?.ToString(),

                        AnhBia = txtAnhBia.Text // Đường dẫn ảnh bìa
                    };

                    db.SachDatabase.Add(currentBook);
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Kiểm tra xem có thay đổi gì không
                    bool isChanged = false;

                    if (currentBook.TenSach != txtTenSach.Text ||
                        currentBook.MaTacGia != cbTacGia.SelectedValue.ToString() ||
                        currentBook.MaTheLoai != cbTheLoai.SelectedValue.ToString() ||
                        currentBook.MaNXB != cbNXB.SelectedValue.ToString() ||
                        currentBook.NamXuatBan != int.Parse(txtNamXB.Text) ||
                        currentBook.SoLanTaiBan != int.Parse(txtSoLanTaiBan.Text) ||
                        currentBook.SoLuong != (int)numericSoLuong.Value ||
                        currentBook.GiaTien != decimal.Parse(txtGia.Text) ||
                        currentBook.MaViTri != cbViTri.SelectedValue.ToString() ||
                        currentBook.AnhBia != txtAnhBia.Text)
                    {
                        isChanged = true;
                    }

                    if (!isChanged)
                    {
                        MessageBox.Show("Không có thay đổi nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // Không thực hiện lưu nếu không có thay đổi
                    }

                    // Nếu đã có thay đổi, tiến hành sửa
                    currentBook.TenSach = txtTenSach.Text;
                    currentBook.MaTacGia = db.TacGiaDatabase.First(t => t.TenTacGia == cbTacGia.Text).MaTacGia;
                    currentBook.MaTheLoai = db.TheLoaiDatabase.First(tl => tl.TenTheLoai == cbTheLoai.Text).MaTheLoai;
                    currentBook.MaNXB = db.NhaXuatBanDatabase.First(nxb => nxb.TenNXB == cbNXB.Text).MaNXB;
                    currentBook.NamXuatBan = int.Parse(txtNamXB.Text);
                    currentBook.SoLanTaiBan = int.Parse(txtSoLanTaiBan.Text);
                    currentBook.SoLuong = (int)numericSoLuong.Value;
                    currentBook.GiaTien = decimal.Parse(txtGia.Text);
                    currentBook.MaViTri = cbViTri.SelectedValue?.ToString();
                    currentBook.AnhBia = txtAnhBia.Text; // Cập nhật đường dẫn ảnh bìa

                    MessageBox.Show("Sửa thông tin sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                LoadDanhSachSach();
                ResizeRowsToFill();// Tải lại danh sách sách
                ClearFields(); // Xóa các trường nhập

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentBook == null)
                {
                    MessageBox.Show("Vui lòng chọn sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sách này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    db.SachDatabase.Remove(currentBook);
                    db.SaveChanges();
                    MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach(); // Tải lại danh sách sách
                    ResizeRowsToFill();
                    ClearFields(); // Xóa các trường nhập
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFields()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            cbTacGia.SelectedIndex = -1; // Hoặc cbTacGia.SelectedItem = null;  
            cbTheLoai.SelectedIndex = -1; // Hoặc cbTheLoai.SelectedItem = null;  
            cbNXB.SelectedIndex = -1;
            txtNamXB.Clear();
            txtSoLanTaiBan.Clear();
            numericSoLuong.Value = 0;
            txtGia.Clear();
            txtAnhBia.Clear();
            picBiaSach.Image = null;
            currentBook = null; // Reset đối tượng sách hiện tại  
        }

        
        private void QuanLySachForm_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Xóa nội dung trong TextBox
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtNamXB.Clear();
            txtSoLanTaiBan.Clear();
            txtAnhBia.Clear();
            txtGia.Clear();

            // Đặt lại giá trị mặc định cho ComboBox
            cbTacGia.SelectedIndex = -1;
            cbTheLoai.SelectedIndex = -1;
            cbNXB.SelectedIndex = -1;

            // Đặt lại giá trị cho NumericUpDown (nếu có)
            numericSoLuong.Value = 1;

            // Xóa ảnh trong PictureBox
            picBiaSach.Image = null;

            // Đặt focus vào TextBox đầu tiên (nếu cần)
            txtMaSach.Focus();
            
        }

        private void QuanLySachForm_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachSach();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachSach();
            }
        }
    }
}
