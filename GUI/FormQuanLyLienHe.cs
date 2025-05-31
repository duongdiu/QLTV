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
    public partial class FormQuanLyLienHe : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 1;
        public FormQuanLyLienHe()
        {
            InitializeComponent();
            dgvLienHe.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void FormQuanLyLienHe_Load(object sender, EventArgs e)
        {
            cmbChuDeLoc.Items.Add("Tất cả");
            cmbChuDeLoc.Items.Add("Khiếu nại");
            cmbChuDeLoc.Items.Add("Góp ý");
            cmbChuDeLoc.Items.Add("Đề xuất");
            cmbChuDeLoc.Items.Add("Khác");
            cmbChuDeLoc.SelectedIndex = 0;
            LoadData();
            dgvLienHe.CellClick += dgvLienHe_CellClick;
        }
        private void LoadData(string keyword = "", string chude = "Tất cả")
        {
            try {
                var query = db.LienHeDatabase.AsQueryable();

                // Lọc theo từ khóa
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(lh =>
                        lh.HoTen.Contains(keyword) ||
                        lh.Email.Contains(keyword) ||
                        lh.Chude.Contains(keyword)
                    );
                }

                // Lọc theo chủ đề nếu không phải "Tất cả"
                if (!string.IsNullOrWhiteSpace(chude) && chude != "Tất cả")
                {
                    query = query.Where(lh => lh.Chude == chude);
                }

                // Tính tổng số trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Lấy dữ liệu trang hiện tại
                var data = query
                    .OrderByDescending(lh => lh.ThoiGianGui)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(lh => new
                    {
                        lh.MaLienHe,
                        lh.HoTen,
                        lh.Email,
                        lh.Sdt,
                        lh.Chude,
                        lh.ThoiGianGui
                    })
                    .ToList();

                dgvLienHe.DataSource = data;
                ResizeRowsToFill();

                // Cập nhật tiêu đề cột
                if (dgvLienHe.Columns.Count > 0)
                {
                    dgvLienHe.Columns["MaLienHe"].HeaderText = "Mã Liên Hệ";
                    dgvLienHe.Columns["HoTen"].HeaderText = "Họ Tên";
                    dgvLienHe.Columns["Email"].HeaderText = "Email";
                    dgvLienHe.Columns["Sdt"].HeaderText = "SĐT";
                    dgvLienHe.Columns["Chude"].HeaderText = "Chủ đề";
                    dgvLienHe.Columns["ThoiGianGui"].HeaderText = "Thời gian gửi";
                }

                dgvLienHe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLienHe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label trang (nếu có)
                lblPage.Text = $"Trang {currentPage} / {totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load liên hệ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvLienHe.ColumnHeadersHeight;
            int availableHeight = dgvLienHe.ClientSize.Height - headerHeight;
            int rowCount = dgvLienHe.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvLienHe.Rows)
            {
                row.Height = rowHeight;
            }
        }


        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            LoadData(keyword);
            ResizeRowsToFill();

        }

        private void dgvLienHe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string maLienHe = dgvLienHe.Rows[e.RowIndex].Cells["MaLienHe"].Value.ToString();
                    var lienHe = db.LienHeDatabase.FirstOrDefault(lh => lh.MaLienHe == maLienHe);

                    if (lienHe != null)
                    {
                        txtChiTiet.Text = $"📌 Họ tên: {lienHe.HoTen}\r\n" +
                                          $"📧 Email: {lienHe.Email}\r\n" +
                                          $"📞 SĐT: {lienHe.Sdt}\r\n" +
                                          $"📂 Chủ đề: {lienHe.Chude}\r\n" +
                                          $"🕒 Thời gian gửi: {lienHe.ThoiGianGui}\r\n\r\n" +
                                          $"✉ Nội dung:\r\n{lienHe.NoiDung}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn liên hệ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLienHe.SelectedRows.Count > 0)
                {
                    string maLienHe = dgvLienHe.SelectedRows[0].Cells["MaLienHe"].Value.ToString();
                    var lienHe = db.LienHeDatabase.FirstOrDefault(lh => lh.MaLienHe == maLienHe);

                    if (lienHe != null)
                    {
                        var result = MessageBox.Show("Bạn có chắc chắn muốn xóa liên hệ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            db.LienHeDatabase.Remove(lienHe);
                            db.SaveChanges();
                            LoadData();
                            ResizeRowsToFill();
                            txtChiTiet.Clear();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một liên hệ để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa liên hệ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string chude = cmbChuDeLoc.SelectedItem.ToString();
            LoadData("", chude);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try {
                txtTimKiem.Clear();
                txtChiTiet.Clear();
                cmbChuDeLoc.SelectedIndex = 0;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtChiTiet_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadData();
            }
        }
    }
}
