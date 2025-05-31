using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormCapNhatThongTin : Form
    {
        private string maDocGia;
        private LibraryDBContext db = new LibraryDBContext();
        public FormCapNhatThongTin(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
        }

        private void FormCapNhatThongTin_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy"; // Định dạng ngày dd/MM/yyyy  
            dtpNgaySinh.Value = DateTime.Now; // Đặt giá trị mặc định (nếu cần)  

            using (LibraryDBContext db = new LibraryDBContext())
            {
                var docGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == maDocGia);
                if (docGia != null)
                {
                    txtHoTen.Text = docGia.HoTen;
                    dtpNgaySinh.Value = docGia.NgaySinh ?? DateTime.Now;

                    if (docGia.GioiTinh == "Nam")
                        rdoNam.Checked = true;
                    else
                        rdoNu.Checked = true;

                    rtbDiaChi.Text = docGia.DiaChi;
                    txtEmail.Text = docGia.Email;

                    // ✅ Thêm dòng này để lấy và hiển thị số điện thoại  
                    txtSoDienThoai.Text = docGia.SoDienThoai; // Hiển thị số điện thoại  
                }
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                using (LibraryDBContext db = new LibraryDBContext())
                {
                    var docGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == maDocGia);
                    if (docGia == null)
                    {
                        MessageBox.Show("Không tìm thấy độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var nguoiDung = db.NguoiDungDatabase.FirstOrDefault(nd => nd.MaDocGia == maDocGia);
                    if (nguoiDung == null)
                    {
                        MessageBox.Show("Không tìm thấy tài khoản đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // === Lấy thông tin từ giao diện ===
                    string hoTen = txtHoTen.Text.Trim();
                    string diaChi = rtbDiaChi.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string soDienThoai = txtSoDienThoai.Text.Trim();
                    DateTime ngaySinh = dtpNgaySinh.Value;

                    // === Validate Họ tên ===
                    if (string.IsNullOrWhiteSpace(hoTen))
                    {
                        MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtHoTen.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(hoTen, @"^[\p{L} ]+$"))
                    {
                        MessageBox.Show("Họ tên không được chứa số hoặc ký tự đặc biệt!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtHoTen.Focus();
                        return;
                    }

                    // === Validate Ngày sinh ===
                    if (ngaySinh >= DateTime.Now.Date)
                    {
                        MessageBox.Show("Ngày sinh không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dtpNgaySinh.Focus();
                        return;
                    }

                    // === Validate Địa chỉ ===
                    if (string.IsNullOrWhiteSpace(diaChi))
                    {
                        MessageBox.Show("Vui lòng nhập địa chỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        rtbDiaChi.Focus();
                        return;
                    }

                    // === Validate Email ===
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        MessageBox.Show("Vui lòng nhập email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEmail.Focus();
                        return;
                    }

                    // === Validate Số điện thoại ===
                    if (string.IsNullOrWhiteSpace(soDienThoai))
                    {
                        MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoDienThoai.Focus();
                        return;
                    }
                    if (!Regex.IsMatch(soDienThoai, @"^0\d{9}$"))
                    {
                        MessageBox.Show("Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSoDienThoai.Focus();
                        return;
                    }

                    // === Cập nhật thông tin độc giả ===
                    docGia.HoTen = hoTen;
                    docGia.NgaySinh = ngaySinh;
                    docGia.GioiTinh = rdoNam.Checked ? "Nam" : "Nữ";
                    docGia.DiaChi = diaChi;
                    docGia.Email = email;
                    docGia.SoDienThoai = soDienThoai;

                    // === Cập nhật mật khẩu nếu có ===
                    string matKhauCu = txtMatKhauCu.Text;
                    string matKhauMoi = txtMatKhauMoi.Text;
                    string xacNhanMK = txtXacNhanMatKhau.Text;

                    if (!string.IsNullOrEmpty(matKhauCu) || !string.IsNullOrEmpty(matKhauMoi) || !string.IsNullOrEmpty(xacNhanMK))
                    {
                        if (string.IsNullOrEmpty(matKhauCu))
                        {
                            MessageBox.Show("Vui lòng nhập mật khẩu cũ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMatKhauCu.Focus();
                            return;
                        }

                        if (nguoiDung.MatKhau != matKhauCu) // So sánh đơn giản, bạn có thể thay bằng mã hóa nếu có
                        {
                            MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMatKhauCu.Focus();
                            return;
                        }

                        if (matKhauMoi.Length < 8)
                        {
                            MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMatKhauMoi.Focus();
                            return;
                        }

                        if (matKhauMoi != xacNhanMK)
                        {
                            MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtXacNhanMatKhau.Focus();
                            return;
                        }

                        // ✅ Cập nhật mật khẩu mới
                        nguoiDung.MatKhau = matKhauMoi;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtMatKhauCu_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
