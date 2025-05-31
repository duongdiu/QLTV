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
    public partial class FormTimKiemSach : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPageSach = 1;
        private int pageSizeSach = 5;
        private int totalPagesSach = 1;
        public FormTimKiemSach()
        {
            InitializeComponent();
            LoadDanhTimKiemSach();
            LoadComboBoxes(); // Tải dữ liệu cho ComboBox khi khởi động form  
            dgvKetQua.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void LoadComboBoxes()
        {
            var theLoaiList = db.TheLoaiDatabase.ToList();

            // Thêm mục "Tất cả" vào đầu danh sách
            theLoaiList.Insert(0, new QLTV_Database.TheLoai
            {
                MaTheLoai = "", // hoặc null nếu bạn dùng kiểu int
                TenTheLoai = "-- Tất cả thể loại --"
            });

            cmbTheLoai.DataSource = theLoaiList;
            cmbTheLoai.DisplayMember = "TenTheLoai";
            cmbTheLoai.ValueMember = "MaTheLoai";
        }

        private void LoadDanhTimKiemSach(string tuKhoa = "")
        {
            try
            {
                var query = from s in db.SachDatabase
                            select new
                            {
                                s.MaSach,
                                s.TenSach,
                                s.MaTheLoai,
                                TacGia = db.TacGiaDatabase.FirstOrDefault(t => t.MaTacGia == s.MaTacGia).TenTacGia,
                                TheLoai = db.TheLoaiDatabase.FirstOrDefault(tl => tl.MaTheLoai == s.MaTheLoai).TenTheLoai,
                                NhaXB = db.NhaXuatBanDatabase.FirstOrDefault(nxb => nxb.MaNXB == s.MaNXB).TenNXB,
                                s.NamXuatBan,
                                s.SoLuong,
                                s.AnhBia
                            };

                // Lọc từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(s =>
                        s.TenSach.ToLower().Contains(tuKhoa) ||
                        s.MaSach.ToString().Contains(tuKhoa));
                }

                // Lọc thể loại
                string maTheLoai = cmbTheLoai.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(maTheLoai))
                {
                    query = query.Where(s => s.MaTheLoai == maTheLoai);
                }

                var allResults = query.ToList();

                // Tính tổng trang
                int totalRecords = allResults.Count();
                totalPagesSach = (int)Math.Ceiling(totalRecords / (double)pageSizeSach);

                // Đảm bảo trang hiện tại nằm trong khoảng
                if (currentPageSach < 1) currentPageSach = 1;
                if (currentPageSach > totalPagesSach) currentPageSach = totalPagesSach;

                // Lấy dữ liệu theo trang
                var dataPage = allResults
                    .Skip((currentPageSach - 1) * pageSizeSach)
                    .Take(pageSizeSach)
                    .ToList();

                // Hiển thị dữ liệu
                dgvKetQua.DataSource = dataPage;
                ResizeRowsToFill();

                if (dgvKetQua.Columns.Count > 0)
                {
                    dgvKetQua.Columns["MaSach"].HeaderText = "Mã Sách";
                    dgvKetQua.Columns["TenSach"].HeaderText = "Tên Sách";
                    dgvKetQua.Columns["TacGia"].HeaderText = "Tác Giả";
                    dgvKetQua.Columns["TheLoai"].HeaderText = "Thể Loại";
                    dgvKetQua.Columns["NhaXB"].HeaderText = "Nhà XB";
                    dgvKetQua.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
                    dgvKetQua.Columns["SoLuong"].HeaderText = "Số Lượng";
                    dgvKetQua.Columns["AnhBia"].Visible = false;
                    dgvKetQua.Columns["MaTheLoai"].Visible = false;
                }

                dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvKetQua.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label phân trang (nếu có)
                lblPage.Text = $"Trang {currentPageSach}/{totalPagesSach}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvKetQua.ColumnHeadersHeight;
            int availableHeight = dgvKetQua.ClientSize.Height - headerHeight;
            int rowCount = dgvKetQua.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvKetQua.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void dgvKetQua_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim(); // Lấy từ khóa tìm kiếm  

            // Gọi hàm lọc sách với từ khóa  
            LoadDanhTimKiemSach(tuKhoa);
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDanhTimKiemSach();
            ResizeRowsToFill();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Xóa từ khóa tìm kiếm
            txtTimKiem.Text = "";

            // Đặt lại combobox thể loại về item đầu tiên
            if (cmbTheLoai.Items.Count > 0)
                cmbTheLoai.SelectedIndex = 0;

            // Tải lại toàn bộ danh sách sách (không lọc)

            LoadDanhTimKiemSach();
            ResizeRowsToFill();
        }

        private void FormTimKiemSach_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPageSach > 1)
            {
                currentPageSach--;
                LoadDanhTimKiemSach();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPageSach < totalPagesSach)
            {
                currentPageSach++;
                LoadDanhTimKiemSach(txtTimKiem.Text);
            }
        }
    }
}
