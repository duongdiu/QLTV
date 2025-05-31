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
    public partial class FormLienHe : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public FormLienHe()
        {
            InitializeComponent();
        }

        private void FormLienHe_Load(object sender, EventArgs e)
        {
            cmbChude.Items.Add("Khiếu nại");
            cmbChude.Items.Add("Góp ý");
            cmbChude.Items.Add("Đề xuất");
            cmbChude.Items.Add("Khác");

            // Chọn mặc định là chủ đề "Khiếu nại"
            cmbChude.SelectedIndex = 0;
        }

        private void btnGui_Click(object sender, EventArgs e)
        {
            try
            {
                string hoTen = txtHoTen.Text.Trim();
                string email = txtEmail.Text.Trim();
                string sdt = txtSoDienThoai.Text.Trim();
                string noiDung = richTextBoxNoiDung.Text.Trim();
                string chude = cmbChude.SelectedItem?.ToString();

                // Kiểm tra bắt buộc nhập
                if (string.IsNullOrWhiteSpace(hoTen) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(noiDung) ||
                    string.IsNullOrWhiteSpace(chude))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin liên hệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // ✅ Kiểm tra họ tên không chứa số hoặc ký tự đặc biệt
                if (!Regex.IsMatch(hoTen, @"^[a-zA-ZÀ-Ỵà-ỵ\s]+$"))
                {
                    MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trắng, không chứa số hoặc ký tự đặc biệt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra email hợp lệ
                if (!IsValidEmail(email))
                {
                    MessageBox.Show("Email không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số điện thoại (tùy quy định, VD: 10 số, bắt đầu bằng số 0)
                if (!string.IsNullOrEmpty(sdt) && !Regex.IsMatch(sdt, @"^0\d{9}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ! (Gồm 10 chữ số và bắt đầu bằng số 0)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra độ dài nội dung phản ánh
                if (noiDung.Length < 10)
                {
                    MessageBox.Show("Nội dung phải có ít nhất 10 ký tự.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
       
                    var lienHe = new LienHe
                    {
                        MaLienHe = GenerateMaLienHe(),
                        HoTen = hoTen,
                        Email = email,
                        Sdt = sdt,
                        Chude = chude,
                        NoiDung = noiDung,
                        ThoiGianGui = DateTime.Now
                    };

                    db.LienHeDatabase.Add(lienHe);
                    db.SaveChanges();

                    MessageBox.Show("Liên hệ của bạn đã được gửi thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                } catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi gửi liên hệ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Hàm kiểm tra email hợp lệ
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

        // Tạo mã liên hệ tự động
        private string GenerateMaLienHe()
        {
            var lastLienHe = db.LienHeDatabase
                .OrderByDescending(lh => lh.MaLienHe)
                .FirstOrDefault();

            if (lastLienHe == null)
            {
                return "LH01"; // Nếu chưa có liên hệ nào, bắt đầu từ LH01
            }

            int lastNumber = int.Parse(lastLienHe.MaLienHe.Substring(2)); // Lấy phần số của mã
            int newNumber = lastNumber + 1;

            return "LH" + newNumber.ToString("D2"); // Tạo mã mới (LH01, LH02, ...)
        }

        // Reset form
        private void ResetForm()
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            cmbChude.SelectedIndex = 0;
            richTextBoxNoiDung.Clear();
        }

        // Đóng form
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            cmbChude.SelectedIndex = 0; // Chọn mặc định chủ đề "Khiếu nại"
            richTextBoxNoiDung.Clear();
        }

        private void richTextBoxNoiDung_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
    

