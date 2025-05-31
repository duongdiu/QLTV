using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormMuonSach : Form
    {
        private string maDocGia;
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allSach = new List<object>();

        public FormMuonSach(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
            dgvSach.CellClick += dgvSach_CellClick;
            LoadDanhSachSach();
            dgvSach.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void FormMuonSach_Load(object sender, EventArgs e)
        {
            LoadDanhSachSach();
            dtpNgay.Value = DateTime.Today;
            dtpGio.Format = DateTimePickerFormat.Time;
            dtpGio.ShowUpDown = true;
        }

        private void LoadDanhSachSach(string tuKhoa = "")
        {
            var query = db.SachDatabase
        .Select(s => new
        {
            s.MaSach,
            s.TenSach,
            TacGia = db.TacGiaDatabase.FirstOrDefault(t => t.MaTacGia == s.MaTacGia).TenTacGia,
            TheLoai = db.TheLoaiDatabase.FirstOrDefault(tl => tl.MaTheLoai == s.MaTheLoai).TenTheLoai,
            NhaXB = db.NhaXuatBanDatabase.FirstOrDefault(nxb => nxb.MaNXB == s.MaNXB).TenNXB,

            // Tính số sách đang được mượn (chưa trả)
            SoLuongDangMuon = db.ChiTietPhieuMuonDatabase
                .Where(ct => ct.MaSach == s.MaSach && ct.NgayTra == null)
                .Count(),

            s.SoLuong,
            s.AnhBia
        })
        .AsEnumerable() // chuyển sang LINQ to Object để dùng biến tính toán
        .Select(s => new
        {
            s.MaSach,
            s.TenSach,
            s.TacGia,
            s.TheLoai,
            s.NhaXB,
            SoLuongConLai = s.SoLuong - s.SoLuongDangMuon,
            s.AnhBia
        })
        .Where(s => s.SoLuongConLai > 0); // chỉ lấy sách còn trong kho

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                tuKhoa = tuKhoa.ToLower();
                query = query.Where(s =>
                    s.TenSach.ToLower().Contains(tuKhoa) ||
                    s.TacGia.ToLower().Contains(tuKhoa) ||
                    s.TheLoai.ToLower().Contains(tuKhoa) ||
                    s.NhaXB.ToLower().Contains(tuKhoa));
            }

            allSach = query.ToList<object>();
            totalPages = (int)Math.Ceiling((double)allSach.Count / pageSize);
            currentPage = 1;
            LoadPageData();

        }

        private void LoadPageData()
        {
            var pageData = allSach
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            dgvSach.DataSource = pageData;
            ResizeRowsToFill();

            dgvSach.Columns["MaSach"].HeaderText = "Mã Sách";
            dgvSach.Columns["TenSach"].HeaderText = "Tên Sách";
            dgvSach.Columns["TacGia"].HeaderText = "Tác Giả";
            dgvSach.Columns["TheLoai"].HeaderText = "Thể Loại";
            dgvSach.Columns["NhaXB"].HeaderText = "Nhà XB";
            dgvSach.Columns["SoLuongConLai"].HeaderText = "Số Lượng";
            dgvSach.Columns["AnhBia"].Visible = false;

            dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSach.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            lblPage.Text = $"Trang {currentPage}/{totalPages}";
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvSach.ColumnHeadersHeight;
            int availableHeight = dgvSach.ClientSize.Height - headerHeight;
            int rowCount = dgvSach.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvSach.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private string GenerateMaLichMuon()
        {
            int count = db.DatLichMuonDatabase.Count() + 1;
            return "LM" + count.ToString("D2");
        }

        private string GenerateMaChiTiet()
        {
            int count = db.ChiTietDatLichMuonDatabase.Count() + 1;
            return "CTLM" + count.ToString("D3");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            LoadDanhSachSach(keyword);
            ResizeRowsToFill();
        }

        private void dgvSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSach.Rows[e.RowIndex];
                txtTenSach.Text = row.Cells["TenSach"].Value?.ToString() ?? "";
                txtTacGia.Text = row.Cells["TacGia"].Value?.ToString() ?? "";
                txtTheLoai.Text = row.Cells["TheLoai"].Value?.ToString() ?? "";
                txtNhaXB.Text = row.Cells["NhaXB"].Value?.ToString() ?? "";
                numericSoLuong.Value = 1;

                // Load ảnh bìa
                string anhBiaPath = row.Cells["AnhBia"].Value?.ToString();
                if (!string.IsNullOrEmpty(anhBiaPath) && System.IO.File.Exists(anhBiaPath))
                {
                    using (var stream = new System.IO.FileStream(anhBiaPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        picAnhBia.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    picAnhBia.Image = null;
                }
            }
        }
        

        

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvSach.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string maSach = dgvSach.CurrentRow.Cells["MaSach"].Value.ToString();
                int soLuongMuon = (int)numericSoLuong.Value;
                var sach = db.SachDatabase.FirstOrDefault(s => s.MaSach == maSach);

                if (sach == null || sach.SoLuong < soLuongMuon)
                {
                    MessageBox.Show("Số lượng sách không đủ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime ngayDen = dtpNgay.Value.Date;
                TimeSpan gioDen = dtpGio.Value.TimeOfDay;

                string maLichMuon = GenerateMaLichMuon();
                string maChiTiet = GenerateMaChiTiet();

                var lich = new DatLichMuon
                {
                    MaLichMuon = maLichMuon,
                    MaDocGia = maDocGia,
                    NgayDenMuon = ngayDen,
                    GioDenMuon = gioDen,
                    TrangThai = "Chờ xử lý",
                    GhiChu = txtGhiChu.Text.Trim()
                };

                var chiTiet = new ChiTietDatLichMuon
                {
                    MaChiTiet = maChiTiet,
                    MaLichMuon = maLichMuon,
                    MaSach = maSach,
                    SoLuong = soLuongMuon
                };

                db.DatLichMuonDatabase.Add(lich);
                db.ChiTietDatLichMuonDatabase.Add(chiTiet);

                db.SaveChanges();

                MessageBox.Show("Đặt lịch mượn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSach();
                ResizeRowsToFill();

                // --------- XÓA DỮ LIỆU VỪA NHẬP ----------
                numericSoLuong.Value = 1;
                txtGhiChu.Clear();
                dtpNgay.Value = DateTime.Today;
                dtpGio.Value = DateTime.Now.TimeOfDay < new TimeSpan(23, 59, 59) ? DateTime.Now : DateTime.Today.AddHours(8); // nếu cần reset giờ
                dgvSach.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đặt lịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
             
            this.Hide();
        }

        private void dtpNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPageData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPageData();
            }
        }

        private void FormMuonSach_Load_1(object sender, EventArgs e)
        {

        }
    }
}
