using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormBinhLuan : Form
    {
        private string maDocGia;
        private LibraryDBContext db = new LibraryDBContext();
        public FormBinhLuan(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
            LoadSachDaMuon();
            listViewSach.DoubleClick += listViewSach_DoubleClick;
        }

        // Load danh sách sách đã mượn
        private void LoadSachDaMuon()
        {
            try
            {
                var sachDaMuon = db.PhieuMuonDatabase
                    .Where(pm => pm.MaDocGia == maDocGia)
                    .Join(db.ChiTietPhieuMuonDatabase, pm => pm.MaPhieuMuon, ctp => ctp.MaPhieuMuon, (pm, ctp) => new { ctp.MaSach, pm.NgayMuon })
                    .Join(db.SachDatabase, ctp => ctp.MaSach, s => s.MaSach, (ctp, s) => new { s.MaSach, s.TenSach, s.MaTacGia, ctp.NgayMuon })
                    .Distinct()
                    .ToList();

                listViewSach.Items.Clear();
                foreach (var sach in sachDaMuon)
                {
                    var item = new ListViewItem(sach.TenSach);
                    item.SubItems.Add(sach.MaTacGia);
                    item.SubItems.Add(sach.NgayMuon.HasValue ? sach.NgayMuon.Value.ToString("dd/MM/yyyy") : "N/A");
                    item.Tag = sach.MaSach;
                    listViewSach.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tải sách đã mượn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        private void FormBinhLuan_Load(object sender, EventArgs e)
        {

        }

        private void listViewSach_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listViewSach.SelectedItems.Count > 0)
                {
                    var selectedItem = listViewSach.SelectedItems[0];
                    string maSach = selectedItem.Tag.ToString();
                    string tenSach = selectedItem.Text;

                    // Gán tên sách vào label
                    lblSachDaChon.Text = "Sách đang chọn: " + tenSach;

                    var phanHoi = db.PhanHoiDatabase.FirstOrDefault(ph => ph.MaDocGia == maDocGia && ph.MaSach == maSach);

                    if (phanHoi != null)
                    {
                        richTextBoxNoiDung.Text = phanHoi.NoiDung;
                    }
                    else
                    {
                        richTextBoxNoiDung.Clear();
                    }

                    btnGuiBinhLuan.Tag = maSach;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xử lý sự kiện click: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GenerateMaPhanHoi()
        {
            // Lấy mã phản hồi cuối cùng từ cơ sở dữ liệu
            var lastPhanHoi = db.PhanHoiDatabase
                .OrderByDescending(ph => ph.MaPhanHoi)
                .FirstOrDefault();

            // Nếu không có dữ liệu nào, bắt đầu từ PH01
            if (lastPhanHoi == null)
            {
                return "PH01";
            }

            // Nếu đã có dữ liệu, lấy số cuối của mã phản hồi và cộng thêm 1
            int lastNumber = int.Parse(lastPhanHoi.MaPhanHoi.Substring(2)); // Bỏ "PH" và lấy số
            int newNumber = lastNumber + 1;

            // Sinh mã mới theo định dạng PHxx
            return "PH" + newNumber.ToString("D2");  // D2 để đảm bảo là 2 chữ số (ví dụ: PH01, PH02, ...)
        }


        

        private void btnDong_Click(object sender, EventArgs e)
        { 
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBoxNoiDung_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnGuiBinhLuan_Click_1(object sender, EventArgs e)
        {

        }

        private void btnGuiBinhLuan_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGuiBinhLuan.Tag == null)
                {
                    MessageBox.Show("Vui lòng chọn sách để bình luận.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maSach = btnGuiBinhLuan.Tag.ToString();
                string noiDung = richTextBoxNoiDung.Text.Trim();

                if (string.IsNullOrWhiteSpace(noiDung))
                {
                    MessageBox.Show("Nội dung bình luận không được để trống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var phanHoi = db.PhanHoiDatabase.FirstOrDefault(ph => ph.MaDocGia == maDocGia && ph.MaSach == maSach);

                if (phanHoi != null)
                {
                    // Nếu đã có phản hồi thì cập nhật nội dung
                    phanHoi.NoiDung = noiDung;
                    phanHoi.NgayGui = DateTime.Now;
                    db.SaveChanges();
                    MessageBox.Show("Đã cập nhật bình luận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Tạo mới phản hồi
                    string maPhanHoi = GenerateMaPhanHoi();

                    var newPhanHoi = new PhanHoi
                    {
                        MaPhanHoi = maPhanHoi,
                        MaDocGia = maDocGia,
                        MaSach = maSach,
                        NoiDung = noiDung,
                        NgayGui = DateTime.Now
                    };

                    db.PhanHoiDatabase.Add(newPhanHoi);
                    db.SaveChanges();
                    MessageBox.Show("Gửi bình luận thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                richTextBoxNoiDung.Clear();
                btnGuiBinhLuan.Tag = null;
                listViewSach.SelectedItems.Clear();
                lblSachDaChon.Text = "Sách đang chọn: ";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi bình luận: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewSach_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }

