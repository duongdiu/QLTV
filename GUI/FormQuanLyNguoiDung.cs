using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormQuanLyNguoiDung : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;      // Số bản ghi mỗi trang
        private int currentPage = 1;   // Trang hiện tại
        private int totalPages = 1;
        public FormQuanLyNguoiDung()
        {
            InitializeComponent();
            LoadNguoiDung();
            cboVaiTro.Items.Add("Quản trị viên");  // Admin
            cboVaiTro.Items.Add("Thủ thư");         // Librarian
            cboVaiTro.Items.Add("Độc giả");
            dgvNguoiDung.CellClick += dgvNguoiDung_CellClick;
            LoadMaDocGia();
            dgvNguoiDung.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadMaDocGia()
        {
            var docGiasDaCoTaiKhoan = db.NguoiDungDatabase
                .Where(nd => nd.MaDocGia != null)
                .Select(nd => nd.MaDocGia)
                .ToList();

            var docGiasChuaCoTaiKhoan = db.DocGiaDatabase
                .Where(dg => !docGiasDaCoTaiKhoan.Contains(dg.MaDocGia))
                .ToList();

            cboMaDocGia.DataSource = docGiasChuaCoTaiKhoan;
            cboMaDocGia.DisplayMember = "MaDocGia";
            cboMaDocGia.ValueMember = "MaDocGia";
            cboMaDocGia.SelectedIndex = -1;
        }
        private void LoadNguoiDung()
        {
            try
            {
                // 1. Lấy query gốc
                var query = db.NguoiDungDatabase.AsQueryable();

                // 2. Tính tổng số bản ghi và tổng số trang
                int totalRecords = query.Count();
                if (totalRecords == 0)
                {
                    MessageBox.Show("Không có người dùng nào trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 3. Sắp xếp trước khi Skip/Take
                query = query.OrderBy(u => u.TenDangNhap);

                // 4. Lấy dữ liệu phân trang
                var pagedData = query
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 5. Gán vào DataGridView
                dgvNguoiDung.DataSource = pagedData;
                ResizeRowsToFill();
                dgvNguoiDung.Columns["TenDangNhap"].HeaderText = "Tên Đăng Nhập";
                dgvNguoiDung.Columns["HoTen"].HeaderText = "Họ và Tên";
                dgvNguoiDung.Columns["VaiTro"].HeaderText = "Vai Trò";
                dgvNguoiDung.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                dgvNguoiDung.Columns["MatKhau"].Visible = false;
                dgvNguoiDung.Columns["DocGia"].Visible = false;
                dgvNguoiDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvNguoiDung.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 6. Cập nhật label hiển thị trang
                lblPage.Text = $"Trang {currentPage} / {totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvNguoiDung.ColumnHeadersHeight;
            int availableHeight = dgvNguoiDung.ClientSize.Height - headerHeight;
            int rowCount = dgvNguoiDung.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvNguoiDung.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void ClearFields()
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            cboVaiTro.SelectedIndex = -1;
            cboMaDocGia.SelectedIndex = -1; // hoặc cboMaDocGia.Text = "";
            chkIsActive.Checked = false;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
          
            try
            {
                // Validate rỗng
                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtMatKhau.Text.Length < 8)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtHoTen.Text, @"^[\p{L}\p{M} ]+$"))
                {
                    MessageBox.Show("Họ tên không hợp lệ. Chỉ được chứa chữ cái và khoảng trắng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cboVaiTro.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn vai trò.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string vaiTro = cboVaiTro.SelectedItem.ToString();

                // Nếu là độc giả thì kiểm tra mã độc giả
                if (vaiTro == "Độc giả")
                {
                    if (string.IsNullOrWhiteSpace(cboMaDocGia.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mã độc giả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(cboMaDocGia.Text, @"^DG\d{2,}$"))
                    {
                        MessageBox.Show("Mã độc giả phải đúng định dạng như: DG01, DG02,...", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Ánh xạ từ tiếng Việt sang tiếng Anh
                if (vaiTro == "Quản trị viên") vaiTro = "Admin";
                else if (vaiTro == "Thủ thư") vaiTro = "Librarian";
                else if (vaiTro == "Độc giả") vaiTro = "Reader";

                // Mã hóa mật khẩu
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtMatKhau.Text);

                var nguoiDung = new NguoiDung
                {
                    TenDangNhap = txtTenDangNhap.Text,
                    MatKhau = hashedPassword,
                    HoTen = txtHoTen.Text.Trim(),
                    VaiTro = vaiTro,
                    MaDocGia = vaiTro == "Reader" ? cboMaDocGia.Text.Trim() : null,
                    IsActive = chkIsActive.Checked
                };

                db.NguoiDungDatabase.Add(nguoiDung);
                db.SaveChanges();
                MessageBox.Show("Thêm người dùng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNguoiDung();
             
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void btnSua_Click(object sender, EventArgs e)
        {
            try 
            { 

                // Validate rỗng
                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtMatKhau.Text.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtHoTen.Text, @"^[\p{L}\p{M} ]+$"))
            {
                MessageBox.Show("Họ tên không hợp lệ. Chỉ được chứa chữ cái và khoảng trắng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboVaiTro.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string vaiTro = cboVaiTro.SelectedItem.ToString();

                // Ánh xạ từ tiếng Việt sang tiếng Anh
                if (vaiTro == "Quản trị viên") vaiTro = "Admin";
                else if (vaiTro == "Thủ thư") vaiTro = "Librarian";
                else if (vaiTro == "Độc giả") vaiTro = "Reader";

                // Kiểm tra nếu vai trò là "Reader" (Độc giả), không cho phép sửa mật khẩu
                if (vaiTro == "Reader")
                {
                    MessageBox.Show("Admin không thể sửa mật khẩu của độc giả.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra nếu mật khẩu có thay đổi
                string newPassword = txtMatKhau.Text;
                string hashedPassword = newPassword;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword); // Mã hóa mật khẩu nếu có thay đổi
                }

                bool isActive = chkIsActive.Checked; // Trạng thái tài khoản (tích hoặc bỏ checkbox)

                var nguoiDung = new NguoiDung
                {
                    TenDangNhap = txtTenDangNhap.Text,
                    MatKhau = hashedPassword, // Mật khẩu đã mã hóa
                    HoTen = txtHoTen.Text,
                    VaiTro = vaiTro,
                    MaDocGia = vaiTro == "Reader" ? cboMaDocGia.Text : null, // Nếu là độc giả
                    IsActive = isActive // Trạng thái tài khoản
                };

                var existing = db.NguoiDungDatabase.Find(nguoiDung.TenDangNhap);
                if (existing != null)
                {
                    db.Entry(existing).CurrentValues.SetValues(nguoiDung);
                    db.SaveChanges();
                    MessageBox.Show("Cập nhật người dùng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNguoiDung();
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var tenDangNhap = txtTenDangNhap.Text;
                if (string.IsNullOrEmpty(tenDangNhap))
                {
                    MessageBox.Show("Vui lòng chọn người dùng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nguoiDung = db.NguoiDungDatabase.Find(tenDangNhap);
                if (nguoiDung != null)
                {
                    // Hộp thoại xác nhận
                    DialogResult result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa người dùng \"{tenDangNhap}\" không?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        db.NguoiDungDatabase.Remove(nguoiDung);
                        db.SaveChanges();
                        MessageBox.Show("Xóa người dùng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNguoiDung();
                        ClearFields();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNguoiDung_CelClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private string HienThiVaiTroTiengViet(string vaiTro)
        {
            switch (vaiTro)
            {
                case "Admin":
                    return "Quản trị viên";
                case "Librarian":
                    return "Thủ thư";
                case "Reader":
                    return "Độc giả";
                default:
                    return vaiTro;
            }
        }

        private void dgvNguoiDung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvNguoiDung.Rows[e.RowIndex];

                    txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString();
                    txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                    cboVaiTro.SelectedItem = HienThiVaiTroTiengViet(row.Cells["VaiTro"].Value?.ToString());
                    cboMaDocGia.Text = row.Cells["MaDocGia"].Value?.ToString();
                    chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Làm mới các trường nhập liệu
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            cboVaiTro.SelectedIndex = -1;  // Đặt lại ComboBox về trạng thái không chọn
            cboMaDocGia.SelectedIndex = -1;

            // Làm mới lại DataGridView (tải lại dữ liệu từ cơ sở dữ liệu)
            LoadNguoiDung();

        }

        private void FormQuanLyNguoiDung_Load(object sender, EventArgs e)
        {

        }

        private void cboMaDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboMaDocGia.SelectedIndex != -1)
            {
                var ma = cboMaDocGia.SelectedValue.ToString();
                var docGia = db.DocGiaDatabase.FirstOrDefault(dg => dg.MaDocGia == ma);
                if (docGia != null)
                {
                    txtHoTen.Text = docGia.HoTen;
                }
            }
        }

        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboVaiTro.SelectedItem.ToString() == "Độc giả")
            {
                txtHoTen.Enabled = false;
                cboMaDocGia.Visible = true;
                LoadMaDocGia();
            }
            else
            {
                txtHoTen.Enabled = true;
                cboMaDocGia.Visible = false;
                txtHoTen.Clear();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadNguoiDung();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadNguoiDung();
            }
        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
    }

