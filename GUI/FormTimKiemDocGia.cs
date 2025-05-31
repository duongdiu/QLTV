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
    public partial class FormTimKiemDocGia : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5; // Số độc giả mỗi trang
        private int totalPages = 1;
        public FormTimKiemDocGia()
        {
            InitializeComponent();
            cmbTinhTrang.Items.Add("-- Tất cả --");
            cmbTinhTrang.Items.Add("Đang mượn");
            cmbTinhTrang.Items.Add("Quá hạn");
            cmbTinhTrang.SelectedIndex = 0;

            LoadDanhSachDocGia();
            dgvKetQuaDocGia.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadDanhSachDocGia(string tuKhoa = "")
        {
            try
            {
                var docGias = db.DocGiaDatabase.ToList(); // Load tất cả độc giả

                var query = from dg in docGias
                            select new
                            {
                                dg.MaDocGia,
                                dg.HoTen,
                                dg.Email,
                                dg.SoDienThoai,
                                dg.NgayDangKy,
                                TinhTrang = GetTinhTrangDocGia(dg.MaDocGia)
                            };

                // Lọc theo từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(d =>
                        d.MaDocGia.ToLower().Contains(tuKhoa) ||
                        d.HoTen.ToLower().Contains(tuKhoa));
                }

                // Lọc theo tình trạng
                string selectedTinhTrang = cmbTinhTrang.SelectedItem?.ToString();
                if (selectedTinhTrang == "Đang mượn")
                    query = query.Where(d => d.TinhTrang == "Đang mượn");
                else if (selectedTinhTrang == "Quá hạn")
                    query = query.Where(d => d.TinhTrang == "Quá hạn");

                // Tính tổng số trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Lấy dữ liệu trang hiện tại
                var dataPage = query
                    .OrderBy(d => d.MaDocGia)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                dgvKetQuaDocGia.DataSource = dataPage;

                // Đặt tiêu đề cột
                if (dgvKetQuaDocGia.Columns.Count > 0)
                {
                    dgvKetQuaDocGia.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                    dgvKetQuaDocGia.Columns["HoTen"].HeaderText = "Họ và Tên";
                    dgvKetQuaDocGia.Columns["Email"].HeaderText = "Email";
                    dgvKetQuaDocGia.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                    dgvKetQuaDocGia.Columns["NgayDangKy"].HeaderText = "Ngày Đăng Ký";
                    dgvKetQuaDocGia.Columns["TinhTrang"].HeaderText = "Tình Trạng";
                }

                dgvKetQuaDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvKetQuaDocGia.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label phân trang
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvKetQuaDocGia.ColumnHeadersHeight;
            int availableHeight = dgvKetQuaDocGia.ClientSize.Height - headerHeight;
            int rowCount = dgvKetQuaDocGia.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvKetQuaDocGia.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private string GetTinhTrangDocGia(string maDocGia)
        {
            var phieuMuon = db.PhieuMuonDatabase
                              .Where(p => p.MaDocGia == maDocGia)
                              .OrderByDescending(p => p.NgayMuon)
                              .ToList();

            foreach (var pm in phieuMuon)
            {
                if (pm.TinhTrang == "Đang mượn")
                {
                    if (pm.NgayTraDuKien.HasValue && pm.NgayTraDuKien.Value < DateTime.Now)
                        return "Quá hạn";
                    else
                        return "Đang mượn";
                }
            }

            return "Không mượn";
        }
        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDanhSachDocGia();
            ResizeRowsToFill();

        }

        private void btnTimKiemDocGia_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiemDocGia.Text.Trim();
            LoadDanhSachDocGia(tuKhoa);
            ResizeRowsToFill();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiemDocGia.Text = "";
            cmbTinhTrang.SelectedIndex = 0; // Tất cả
            LoadDanhSachDocGia();
            ResizeRowsToFill();
        }

        private void FormTimKiemDocGia_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachDocGia(); // Gọi lại không cần truyền từ khóa vì đang phân trang kết quả hiện tại
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachDocGia();
            }
        }
    }
}
