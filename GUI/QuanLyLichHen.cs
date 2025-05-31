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
    public partial class QuanLyLichHen : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5;    // Số dòng mỗi trang
        private int totalPages = 1;
        public QuanLyLichHen()
        {
            InitializeComponent();
            LoadLichMuon();
            LoadTrangThai();
            dgvLichMuon.CellClick += dgvLichMuon_CellClick;
            dgvLichMuon.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadLichMuon(string tuKhoa = "")
        {
            try
            {
                // 1. Lấy query gốc, chưa ToList()
                var query = db.DatLichMuonDatabase.AsQueryable();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    query = query.Where(l => l.DocGia.HoTen.Contains(tuKhoa) || l.MaLichMuon.Contains(tuKhoa));
                }

                // 2. Tính tổng bản ghi và tổng trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 3. Đảm bảo currentPage nằm trong [1…totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // 4. Lấy dữ liệu trang hiện tại
                var pageData = query
                    .OrderBy(l => l.MaLichMuon)                       // Bắt buộc OrderBy trước Skip
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(l => new
                    {
                        l.MaLichMuon,
                        l.NgayDenMuon,
                        l.GioDenMuon,
                        l.TrangThai,
                        l.GhiChu,
                        TenDocGia = l.DocGia.HoTen
                    })
                    .ToList();

                // 5. Bind vào DataGridView
                dgvLichMuon.DataSource = pageData;
                ResizeRowsToFill();

                // 6. Format lại các cột
                dgvLichMuon.Columns["MaLichMuon"].HeaderText = "Mã Lịch Mượn";
                dgvLichMuon.Columns["TenDocGia"].HeaderText = "Họ Tên";
                dgvLichMuon.Columns["NgayDenMuon"].HeaderText = "Ngày Đến Mượn";
                dgvLichMuon.Columns["GioDenMuon"].HeaderText = "Giờ Đến Mượn";
                dgvLichMuon.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dgvLichMuon.Columns["GhiChu"].HeaderText = "Ghi Chú";
                dgvLichMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLichMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 7. Cập nhật label hiển thị trang (giả sử bạn có lblPage)
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvLichMuon.ColumnHeadersHeight;
            int availableHeight = dgvLichMuon.ClientSize.Height - headerHeight;
            int rowCount = dgvLichMuon.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvLichMuon.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void LoadTrangThai()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new string[] { "Chờ xử lý", "Đã duyệt", "Đã hủy" });
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ngayChon = dtpNgayMuon.Value.Date;
                string trangThai = cboTrangThai.SelectedItem?.ToString();

                var query = db.DatLichMuonDatabase.Where(l => l.NgayDenMuon == ngayChon);

                if (!string.IsNullOrEmpty(trangThai))
                    query = query.Where(l => l.TrangThai == trangThai);

                dgvLichMuon.DataSource = query.Select(l => new
                {
                    l.MaLichMuon,
                    l.NgayDenMuon,
                    l.GioDenMuon,
                    l.TrangThai,
                    l.GhiChu,
                    TenDocGia = l.DocGia.HoTen
                }).ToList();
                ResizeRowsToFill();
                // Set HeaderText
                dgvLichMuon.Columns["MaLichMuon"].HeaderText = "Mã Lịch Mượn";
                dgvLichMuon.Columns["TenDocGia"].HeaderText = "Họ và Tên";
                dgvLichMuon.Columns["NgayDenMuon"].HeaderText = "Ngày Đến Mượn";
                dgvLichMuon.Columns["GioDenMuon"].HeaderText = "Giờ Đến Mượn";
                dgvLichMuon.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dgvLichMuon.Columns["GhiChu"].HeaderText = "Ghi Chú";
                dgvLichMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLichMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLichMuon.CurrentRow != null)
                {
                    string ma = dgvLichMuon.CurrentRow.Cells["MaLichMuon"].Value.ToString();
                    var lich = db.DatLichMuonDatabase.FirstOrDefault(x => x.MaLichMuon == ma);

                    if (lich != null && lich.TrangThai != "Đã duyệt")
                    {
                        lich.TrangThai = "Đã duyệt";
                        db.SaveChanges();

                        LoadLichMuon();
                        ResizeRowsToFill();
                        MessageBox.Show("Đã duyệt lịch mượn!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi duyệt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLichMuon.CurrentRow != null)
                {
                    string ma = dgvLichMuon.CurrentRow.Cells["MaLichMuon"].Value.ToString();
                    var lich = db.DatLichMuonDatabase.FirstOrDefault(x => x.MaLichMuon == ma);

                    if (lich != null)
                    {
                        lich.TrangThai = "Đã hủy";
                        db.SaveChanges();
                        LoadLichMuon();
                        ResizeRowsToFill();
                        MessageBox.Show("Đã hủy lịch mượn!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLichMuon.CurrentRow != null)
                {
                    string ma = dgvLichMuon.CurrentRow.Cells["MaLichMuon"].Value.ToString();
                    var lich = db.DatLichMuonDatabase.FirstOrDefault(x => x.MaLichMuon == ma);
                    var chiTiet = db.ChiTietDatLichMuonDatabase.Where(c => c.MaLichMuon == ma).ToList();

                    if (lich != null)
                    {
                        db.ChiTietDatLichMuonDatabase.RemoveRange(chiTiet);
                        db.DatLichMuonDatabase.Remove(lich);
                        db.SaveChanges();
                        LoadLichMuon();
                        ResizeRowsToFill();
                        MessageBox.Show("Đã xóa lịch mượn!");
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboTrangThai.SelectedIndex = -1;
            dtpNgayMuon.Value = DateTime.Now;
            dgvChiTiet.DataSource = null; // Xóa dữ liệu chi tiết
            LoadLichMuon();
            ResizeRowsToFill();
        }

        private void dgvLichMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvLichMuon.CurrentRow != null)
                {
                    string ma = dgvLichMuon.CurrentRow.Cells["MaLichMuon"].Value.ToString();
                    var chiTiet = db.ChiTietDatLichMuonDatabase
                        .Where(c => c.MaLichMuon == ma)
                        .Select(c => new
                        {
                            c.Sach.TenSach,
                            c.Sach.TacGia.TenTacGia,
                            c.Sach.TheLoai.TenTheLoai,
                            c.Sach.NhaXuatBan.TenNXB,
                            SoLuongMuon = c.SoLuong
                        }).ToList();

                    dgvChiTiet.DataSource = chiTiet;

                    // Đặt tên cột rõ ràng cho dgvChiTiet
                    dgvChiTiet.Columns["TenSach"].HeaderText = "Tên Sách";
                    dgvChiTiet.Columns["TenTacGia"].HeaderText = "Tác Giả";
                    dgvChiTiet.Columns["TenTheLoai"].HeaderText = "Thể Loại";
                    dgvChiTiet.Columns["TenNXB"].HeaderText = "Nhà Xuất Bản";
                    dgvChiTiet.Columns["SoLuongMuon"].HeaderText = "Số Lượng Mượn";
                    dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi click: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void QuanLyLichHen_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadLichMuon();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadLichMuon();
            }
        }
    }
}
