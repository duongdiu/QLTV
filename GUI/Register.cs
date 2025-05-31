using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using QLTV_BUS;
using BCrypt.Net;

namespace GUI
{
    public partial class Register : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public Register()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            DateTime ngaySinh = dtpNgaySinh.Value;
            string gioiTinh = radNam.Checked ? "Nam" : "Nữ";
            string diaChi = txtDiaChi.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();  // Lấy số điện thoại từ TextBox  
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string xacNhanMatKhau = txtXacNhanMatKhau.Text;

            // Họ tên không rỗng, không có số hoặc ký tự đặc biệt
            if (string.IsNullOrWhiteSpace(hoTen) ||
                !System.Text.RegularExpressions.Regex.IsMatch(hoTen, @"^[\p{L}\s]+$"))
            {
                MessageBox.Show("Họ tên không hợp lệ. Chỉ được chứa chữ cái và khoảng trắng, không có số hoặc ký tự đặc biệt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return;
            }

            // Ngày sinh phải nhỏ hơn ngày hiện tại
            if (ngaySinh >= DateTime.Today)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpNgaySinh.Focus();
                return;
            }

            // Email hợp lệ
            if (string.IsNullOrWhiteSpace(email) ||
                !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return;
            }

            // Địa chỉ không rỗng
            if (string.IsNullOrWhiteSpace(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            // Số điện thoại đúng 10 số và bắt đầu bằng 0
            if (!System.Text.RegularExpressions.Regex.IsMatch(soDienThoai, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Phải gồm 10 chữ số và bắt đầu bằng số 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDienThoai.Focus();
                return;
            }

            // Tên đăng nhập không rỗng và không bị trùng
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhap.Focus();
                return;
            }

            if (db.NguoiDungDatabase.Any(nd => nd.TenDangNhap == tenDangNhap))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhap.Focus();
                return;
            }

            // Mật khẩu không rỗng và xác nhận phải khớp
            if (string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }

            // Mật khẩu không rỗng, tối thiểu 8 ký tự
            if (string.IsNullOrWhiteSpace(matKhau) || matKhau.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Focus();
                return;
            }

            // Mật khẩu xác nhận phải khớp
            if (matKhau != xacNhanMatKhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtXacNhanMatKhau.Focus();
                return;
            }


            try
            {
                int soThuTu = 1; // Giá trị này cần được lưu trữ và cập nhật mỗi khi thêm độc giả mới
                string maDocGia = "DG" + soThuTu.ToString("D2"); 

                                // Thêm độc giả vào bảng DocGia  
                                var docGia = new DocGia
                {
                    MaDocGia = maDocGia,
                    HoTen = hoTen,
                    NgaySinh = ngaySinh,
                    GioiTinh = gioiTinh,
                    DiaChi = diaChi,
                    Email = email,
                    SoDienThoai = soDienThoai,  // Thêm số điện thoại vào đối tượng DocGia  
                    NgayDangKy = DateTime.Now
                };
                db.DocGiaDatabase.Add(docGia);

                // Nếu có đăng ký tài khoản, tạo tài khoản cho người dùng  
                if (!string.IsNullOrEmpty(tenDangNhap) && !string.IsNullOrEmpty(matKhau))
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);

                    var nguoiDung = new NguoiDung
                    {
                        TenDangNhap = tenDangNhap,
                        MatKhau = hashedPassword,
                        HoTen = hoTen,
                        VaiTro = "Reader",
                        MaDocGia = maDocGia
                    };
                    db.NguoiDungDatabase.Add(nguoiDung);
                }

                db.SaveChanges();
                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở giao diện DocGiaForm sau khi đăng ký thành công  
                string tenDangNhapMoi = txtTenDangNhap.Text.Trim();
                DocGiaForm docGiaForm = new DocGiaForm(tenDangNhapMoi); // Truyền tên đăng nhập  
                docGiaForm.Show();

                // Đóng form đăng ký  
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.InnerException?.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSoDienThoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
