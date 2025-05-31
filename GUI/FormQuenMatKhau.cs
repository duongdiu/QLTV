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
    public partial class FormQuenMatKhau : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public FormQuenMatKhau()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnCapNhatMatKhau_Click(object sender, EventArgs e)
        {
            try
            {
                string tenDangNhap = txtTenDangNhap.Text.Trim();
                string matKhauMoi = txtMatKhauMoi.Text.Trim();
                string xacNhanMatKhauMoi = txtXacNhanMatKhauMoi.Text.Trim();

                if (string.IsNullOrEmpty(tenDangNhap))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem tên đăng nhập có tồn tại trong hệ thống không
                var nguoiDung = db.NguoiDungDatabase.FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap);
                if (nguoiDung == null)
                {
                    MessageBox.Show("Tên đăng nhập không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(xacNhanMatKhauMoi))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (matKhauMoi != xacNhanMatKhauMoi)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Cập nhật mật khẩu trong CSDL
                nguoiDung.MatKhau = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
                db.SaveChanges();

                MessageBox.Show("Mật khẩu đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


