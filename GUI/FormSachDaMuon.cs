using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormSachDaMuon : Form
    {
        private string maDocGia;
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 1;

        public FormSachDaMuon(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
            cboLoc.Items.AddRange(new string[] { "Tất cả", "Đã trả", "Quá hạn", "Đang mượn" });
            cboLoc.SelectedIndex = 0;

            LoadSachDaMuon();
            dgvSachDaMuon.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void LoadSachDaMuon()
        {
            try
            {
                string luaChon = cboLoc.SelectedItem?.ToString();

                var query = from pm in db.PhieuMuonDatabase
                            join ctpm in db.ChiTietPhieuMuonDatabase on pm.MaPhieuMuon equals ctpm.MaPhieuMuon
                            join s in db.SachDatabase on ctpm.MaSach equals s.MaSach
                            where pm.MaDocGia == maDocGia
                            select new
                            {
                                s.MaSach,
                                s.TenSach,
                                s.MaTacGia,
                                ctpm.SoLuongMuon,
                                pm.NgayMuon,
                                pm.NgayTraDuKien,
                                ctpm.NgayTra,
                                TinhTrang = ctpm.NgayTra != null ? "Đã trả" :
                                            (pm.NgayTraDuKien < DateTime.Now ? "Quá hạn" : "Đang mượn")
                            };

                // Lọc theo lựa chọn
                if (luaChon == "Quá hạn")
                    query = query.Where(s => s.TinhTrang == "Quá hạn");
                else if (luaChon == "Đang mượn")
                    query = query.Where(s => s.TinhTrang == "Đang mượn");
                else if (luaChon == "Đã trả")
                    query = query.Where(s => s.TinhTrang == "Đã trả");

                // Tính tổng số trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Lấy dữ liệu theo phân trang
                var sachDaMuon = query
                    .OrderByDescending(s => s.NgayMuon)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                dgvSachDaMuon.DataSource = sachDaMuon;
                ResizeRowsToFill();

                // Cập nhật tiêu đề
                if (dgvSachDaMuon.Columns.Count > 0)
                {
                    dgvSachDaMuon.Columns["MaSach"].HeaderText = "Mã Sách";
                    dgvSachDaMuon.Columns["TenSach"].HeaderText = "Tên Sách";
                    dgvSachDaMuon.Columns["MaTacGia"].HeaderText = "Tác Giả";
                    dgvSachDaMuon.Columns["SoLuongMuon"].HeaderText = "Số Lượng Mượn";
                    dgvSachDaMuon.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
                    dgvSachDaMuon.Columns["NgayTraDuKien"].HeaderText = "Ngày Trả Dự Kiến";
                    dgvSachDaMuon.Columns["NgayTra"].HeaderText = "Ngày Trả";
                    dgvSachDaMuon.Columns["TinhTrang"].HeaderText = "Tình Trạng";
                }

                dgvSachDaMuon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSachDaMuon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label phân trang
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load sách đã mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvSachDaMuon.ColumnHeadersHeight;
            int availableHeight = dgvSachDaMuon.ClientSize.Height - headerHeight;
            int rowCount = dgvSachDaMuon.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvSachDaMuon.Rows)
            {
                row.Height = rowHeight;
            }
        }


        private void dgvSachDaMuon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Nếu cần xử lý khi click từng dòng
        }

        private void btnDong_Click(object sender, EventArgs e)
        {


            // Đóng form hiện tại  
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try {
                string keyword = txtTimKiem.Text.Trim().ToLower();

                var sachDaMuon = (from pm in db.PhieuMuonDatabase
                                  join ctpm in db.ChiTietPhieuMuonDatabase on pm.MaPhieuMuon equals ctpm.MaPhieuMuon
                                  join s in db.SachDatabase on ctpm.MaSach equals s.MaSach
                                  where pm.MaDocGia == maDocGia
                                  select new
                                  {
                                      s.MaSach,
                                      s.TenSach,
                                      s.MaTacGia,
                                      ctpm.SoLuongMuon, // <-- Thêm số lượng mượn
                                      pm.NgayMuon,
                                      ctpm.NgayTra,
                                      pm.NgayTraDuKien,
                                      TinhTrang = ctpm.NgayTra != null ? "Đã trả" :
                                                  (pm.NgayTraDuKien < DateTime.Now ? "Quá hạn" : "Đang mượn")
                                  }).ToList();

                var ketQua = sachDaMuon.Where(s =>
                                 s.TenSach.ToLower().Contains(keyword) ||
                                 s.MaTacGia.ToLower().Contains(keyword))
                                 .ToList();

                dgvSachDaMuon.DataSource = ketQua;
                ResizeRowsToFill();
            }catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}



        private void cboLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSachDaMuon();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboLoc_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadSachDaMuon();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadSachDaMuon();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadSachDaMuon();
            }
        }

        private void FormSachDaMuon_Load(object sender, EventArgs e)
        {

        }
    }
}
