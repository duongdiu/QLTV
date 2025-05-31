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
using QLTV_BUS;
using QLTV_DAO;
using System.Data.Entity;
using BCrypt.Net;
namespace GUI
{
    public partial class Login : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public Login()
        {
            InitializeComponent();
            UpdatePasswordsToHashed();
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
           

            ValidateLogin(tenDangNhap, matKhau);
            MainForm mainForm = new MainForm(); // Form chính không cần đăng nhập lại
            mainForm.Show();

            this.Hide();
        }
        private void ValidateLogin(string tenDangNhap, string matKhau)
        {
            var nguoiDung = db.NguoiDungDatabase.FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap);

            if (nguoiDung != null)
            {
                if (BCrypt.Net.BCrypt.Verify(matKhau, nguoiDung.MatKhau))
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    OpenDashboard(nguoiDung.VaiTro, tenDangNhap);
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Mở giao diện theo vai trò
        private void OpenDashboard(string role, string tenDangNhap)
        {
            Form dashboard = null;

            switch (role)
            {
                case "Admin":
                    dashboard = new AdminForm(tenDangNhap);
                    break;
                case "Librarian":
                    dashboard = new ThuThuForm(tenDangNhap);
                    break;
                case "Reader":
                    dashboard = new DocGiaForm(tenDangNhap); // Truyền tên đăng nhập
                    break;
                default:
                    MessageBox.Show("Vai trò không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            dashboard.ShowDialog();
        }

        // Cập nhật tất cả mật khẩu chưa mã hóa sang dạng BCrypt
        private void UpdatePasswordsToHashed()
        {
            var users = db.NguoiDungDatabase.Where(u => !u.MatKhau.StartsWith("$2a$")).ToList();

            if (users.Count == 0)
            {
                Console.WriteLine("Tất cả mật khẩu đã được mã hóa. Không cần cập nhật.");
                return;
            }

            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.MatKhau))
                {
                    string oldPassword = user.MatKhau.Trim();  // Lấy mật khẩu cũ  
                    user.MatKhau = BCrypt.Net.BCrypt.HashPassword(oldPassword); // Mã hóa  
                    Console.WriteLine($"Mật khẩu cũ: {oldPassword} -> Mật khẩu mới: {user.MatKhau}");
                }
            }

            db.SaveChanges(); // Lưu thay đổi vào database  
            Console.WriteLine("Đã cập nhật tất cả mật khẩu vào dạng mã hóa.");
        }

        private void linkQuenMK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormQuenMatKhau frm = new FormQuenMatKhau();
            frm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
