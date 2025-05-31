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
    public partial class FormTimKiemPhieuMuon : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5; // Số độc giả mỗi trang
        private int totalPages = 1;
        public FormTimKiemPhieuMuon()
        {
            InitializeComponent();
            dtpTuNgay.Enabled = false;
            dtpDenNgay.Enabled = false;

            dtpTuNgay.Value = DateTime.Today.AddDays(-7);
            dtpDenNgay.Value = DateTime.Today;

            LoadDanhSachPhieuMuon();
            dgvPhieuMuon.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadDanhSachPhieuMuon(string tuKhoa = "")
        {
            try
            {
                var danhSach = db.PhieuMuonDatabase
                                 .Join(db.DocGiaDatabase,
                                       pm => pm.MaDocGia,
                                       dg => dg.MaDocGia,
                                       (pm, dg) => new
                                       {
                                           pm.MaPhieuMuon,
                                           dg.HoTen,
                                           pm.NgayMuon,
                                           pm.NgayTraDuKien
                                       })
                                 .Select(x => new
                                 {
                                     x.MaPhieuMuon,
                                     x.HoTen,
                                     x.NgayMuon,
                                     x.NgayTraDuKien,

                                     // Lấy ngày trả mới nhất từ ChiTietPhieuMuon
                                     NgayTra = db.ChiTietPhieuMuonDatabase
                                                 .Where(ct => ct.MaPhieuMuon == x.MaPhieuMuon && ct.NgayTra != null)
                                                 .OrderByDescending(ct => ct.NgayTra)
                                                 .Select(ct => ct.NgayTra)
                                                 .FirstOrDefault()
                                 })
                                 .ToList();

                var data = danhSach.Select(pm => new
                {
                    pm.MaPhieuMuon,
                    pm.HoTen,
                    pm.NgayMuon,
                    pm.NgayTraDuKien,
                    NgayTra = pm.NgayTra?.ToString("dd/MM/yyyy") ?? "Chưa trả",
                    TinhTrang = GetTinhTrang(pm.NgayTra, pm.NgayTraDuKien)
                });

                // Lọc từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    data = data.Where(pm =>
                        pm.MaPhieuMuon.ToLower().Contains(tuKhoa) ||
                        pm.HoTen.ToLower().Contains(tuKhoa));
                }

                // Lọc theo ngày
                if (chkLocTheoNgay.Checked)
                {
                    DateTime tuNgay = dtpTuNgay.Value.Date;
                    DateTime denNgay = dtpDenNgay.Value.Date;
                    data = data.Where(pm =>
                        pm.NgayMuon >= tuNgay && pm.NgayMuon <= denNgay);
                }

                // Phân trang
                int totalRecords = data.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                var dataPage = data
                    .OrderBy(pm => pm.MaPhieuMuon)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                dgvPhieuMuon.DataSource = dataPage;
                ResizeRowsToFill();

                // Cập nhật tiêu đề cột
                if (dgvPhieuMuon.Columns.Count > 0)
                {
                    dgvPhieuMuon.Columns["MaPhieuMuon"].HeaderText = "Mã Phiếu Mượn";
                    dgvPhieuMuon.Columns["HoTen"].HeaderText = "Họ và Tên";
                    dgvPhieuMuon.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
                    dgvPhieuMuon.Columns["NgayTraDuKien"].HeaderText = "Hạn Trả";
                    dgvPhieuMuon.Columns["NgayTra"].HeaderText = "Ngày Trả";
                    dgvPhieuMuon.Columns["TinhTrang"].HeaderText = "Tình Trạng";
                }

                dgvPhieuMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvPhieuMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label trang
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load phiếu mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResizeRowsToFill()
        {
            int headerHeight = dgvPhieuMuon.ColumnHeadersHeight;
            int availableHeight = dgvPhieuMuon.ClientSize.Height - headerHeight;
            int rowCount = dgvPhieuMuon.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvPhieuMuon.Rows)
            {
                row.Height = rowHeight;
            }
        }



        private string GetTinhTrang(DateTime? ngayTra, DateTime? ngayTraDuKien)
        {
            if (ngayTra != null)
                return "Đã trả";
            else if (ngayTraDuKien.HasValue && ngayTraDuKien.Value < DateTime.Now)
                return "Quá hạn";
            else
                return "Đang mượn";
        }

        private void btnTimKiemPhieuMuon_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiemPhieuMuon.Text.Trim();
            LoadDanhSachPhieuMuon(tuKhoa);
            ResizeRowsToFill();
        }

        private void chkLocTheoNgay_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkLocTheoNgay.Checked;
            dtpTuNgay.Enabled = isChecked;
            dtpDenNgay.Enabled = isChecked;
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiemPhieuMuon.Text = "";
            chkLocTheoNgay.Checked = false;
            LoadDanhSachPhieuMuon();
            ResizeRowsToFill();
        }

        private void FormTimKiemPhieuMuon_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachPhieuMuon();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachPhieuMuon();
            }
        }
    }
}
