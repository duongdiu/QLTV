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
    public partial class FormQuanLyBinhLuanSach : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private string maPhanHoiDangChon = null; // Đổi maPhanHoiDangChon thành string
        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 1;
        public FormQuanLyBinhLuanSach()
        {
            InitializeComponent();
            LoadTrangThaiComboBox();
            LoadDuLieu();
            dgvBinhLuan.CellClick += dgvBinhLuan_CellClick;
            dgvBinhLuan.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadTrangThaiComboBox()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("- Chọn trạng thái -"); // Thêm dòng này đầu tiên
            cboTrangThai.Items.Add("Chờ duyệt");
            cboTrangThai.Items.Add("Đã duyệt");
            cboTrangThai.Items.Add("Ẩn");
            cboTrangThai.Items.Add("Từ chối");
            cboTrangThai.SelectedIndex = 0;
        }
        private void LoadDuLieu()
        {
            try {
                string tuKhoa = txtTimKiem.Text.Trim();
                string trangThai = cboTrangThai.SelectedItem?.ToString();

                var danhSach = db.PhanHoiDatabase.AsQueryable();

                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    danhSach = danhSach.Where(p =>
                        p.MaSach.Contains(tuKhoa) ||
                        p.MaDocGia.Contains(tuKhoa) ||
                        p.NoiDung.Contains(tuKhoa));
                }

                if (!string.IsNullOrEmpty(trangThai) && trangThai != "- Chọn trạng thái -")
                {
                    danhSach = danhSach.Where(p => p.TrangThai == trangThai);
                }

                // Join với bảng sách và lấy dữ liệu
                var query = danhSach
                    .Join(db.SachDatabase, ph => ph.MaSach, sach => sach.MaSach, (ph, sach) => new
                    {
                        ph.MaPhanHoi,
                        ph.MaDocGia,
                        TenSach = sach.TenSach,
                        ph.NoiDung,
                        ph.NgayGui,
                        ph.TrangThai,
                        ph.PhanHoiAdmin
                    });

                // Tổng số bản ghi
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Phân trang: Skip/Take
                var pageData = query
                    .OrderBy(p => p.MaPhanHoi)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
                    .Select(p => new
                    {
                        p.MaPhanHoi,
                        p.MaDocGia,
                        p.TenSach,
                        p.NoiDung,
                        NgayGui = p.NgayGui.HasValue ? p.NgayGui.Value.ToString("dd/MM/yyyy") : "",
                        p.TrangThai,
                        p.PhanHoiAdmin
                    })
                    .ToList();

                dgvBinhLuan.DataSource = pageData;
                ResizeRowsToFill();

                // Cài header
                if (dgvBinhLuan.Columns.Contains("MaPhanHoi"))
                    dgvBinhLuan.Columns["MaPhanHoi"].HeaderText = "Mã Phản Hồi";
                if (dgvBinhLuan.Columns.Contains("MaDocGia"))
                    dgvBinhLuan.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                if (dgvBinhLuan.Columns.Contains("TenSach"))
                    dgvBinhLuan.Columns["TenSach"].HeaderText = "Tên Sách";
                if (dgvBinhLuan.Columns.Contains("NoiDung"))
                    dgvBinhLuan.Columns["NoiDung"].HeaderText = "Nội Dung";
                if (dgvBinhLuan.Columns.Contains("NgayGui"))
                    dgvBinhLuan.Columns["NgayGui"].HeaderText = "Ngày Gửi";
                if (dgvBinhLuan.Columns.Contains("TrangThai"))
                    dgvBinhLuan.Columns["TrangThai"].HeaderText = "Trạng Thái";
                if (dgvBinhLuan.Columns.Contains("PhanHoiAdmin"))
                    dgvBinhLuan.Columns["PhanHoiAdmin"].HeaderText = "Phản Hồi Admin";

                dgvBinhLuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvBinhLuan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvBinhLuan.ClearSelection();
                maPhanHoiDangChon = null;

                // Label hiển thị trang (nếu có label lblTrang)
                lblPage.Text = $"Trang {currentPage} / {totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvBinhLuan.ColumnHeadersHeight;
            int availableHeight = dgvBinhLuan.ClientSize.Height - headerHeight;
            int rowCount = dgvBinhLuan.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvBinhLuan.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        private void btnGuiPhanHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (maPhanHoiDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn một bình luận để phản hồi.");
                    return;
                }

                var ph = db.PhanHoiDatabase.Find(maPhanHoiDangChon);
                if (ph != null)
                {
                    ph.PhanHoiAdmin = txtPhanHoi.Text.Trim();
                    db.SaveChanges();
                    MessageBox.Show("Đã phản hồi bình luận.");
                    LoadDuLieu();
                    ResizeRowsToFill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            try
            {
                LoadDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                CapNhatTrangThai("Đã duyệt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi duyệt bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAn_Click(object sender, EventArgs e)
        {
            try
            {
                CapNhatTrangThai("Ẩn");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ẩn bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (maPhanHoiDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bình luận cần xóa.");
                    return;
                }

                var ph = db.PhanHoiDatabase.Find(maPhanHoiDangChon);
                if (ph != null)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa bình luận này?", "Xác nhận",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        db.PhanHoiDatabase.Remove(ph);
                        db.SaveChanges();
                        MessageBox.Show("Đã xóa bình luận.");
                        LoadDuLieu();
                        ResizeRowsToFill();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy bình luận để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatTrangThai(string trangThai)
        {
            try
            {
                if (maPhanHoiDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bình luận cần cập nhật.");
                    return;
                }

                var ph = db.PhanHoiDatabase.Find(maPhanHoiDangChon);
                if (ph != null)
                {
                    ph.TrangThai = trangThai;
                    db.SaveChanges();
                    LoadDuLieu();
                    ResizeRowsToFill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvBinhLuan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Kiểm tra xem giá trị ô MaPhanHoi có hợp lệ không
                    string maPhanHoiStr = dgvBinhLuan.Rows[e.RowIndex].Cells["MaPhanHoi"].Value?.ToString();

                    if (!string.IsNullOrEmpty(maPhanHoiStr))
                    {
                        // Nếu giá trị hợp lệ, lưu mã phản hồi đã chọn
                        maPhanHoiDangChon = maPhanHoiStr;

                        // Tìm phản hồi từ cơ sở dữ liệu theo MaPhanHoi
                        var phanHoi = db.PhanHoiDatabase.Find(maPhanHoiDangChon);

                        if (phanHoi != null)
                        {
                            // Nếu có phản hồi, hiển thị nội dung phản hồi admin (nếu có)
                            txtPhanHoi.Text = phanHoi.PhanHoiAdmin;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã phản hồi không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormQuanLyBinhLuanSach_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDuLieu();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDuLieu();
            }
        }
    }
}
