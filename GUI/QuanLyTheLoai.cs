using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class QuanLyTheLoai : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1; // Trang hiện tại
        private int totalPages = 1;  // Tổng số trang
        private int pageSize = 5;
        public QuanLyTheLoai()
        {
            InitializeComponent();
            LoadDanhSachTheLoai();
            dgvTheLoai.CellClick += dgvTheLoai_CellClick;
            dgvTheLoai.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void LoadDanhSachTheLoai(string tuKhoa = "")
        {
            try
            {
                var query = db.TheLoaiDatabase.AsQueryable();

                // Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(tl => tl.TenTheLoai.ToLower().Contains(tuKhoa));
                }

                // Tính tổng số bản ghi và tổng số trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Đảm bảo currentPage nằm trong [1...totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // Lấy dữ liệu theo trang hiện tại
                var pageData = query
                    .OrderBy(tl => tl.MaTheLoai)  // Sắp xếp trước khi phân trang
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(tl => new
                    {
                        tl.MaTheLoai,
                        tl.TenTheLoai
                    })
                    .ToList();

                // Bind vào DataGridView
                dgvTheLoai.DataSource = pageData;
                ResizeRowsToFill();

                // Đặt tên cột trong DataGridView
                dgvTheLoai.Columns["MaTheLoai"].HeaderText = "Mã thể loại";
                dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên thể loại";
                dgvTheLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTheLoai.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật label hiển thị trang (giả sử bạn có lblPage)
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvTheLoai.ColumnHeadersHeight;
            int availableHeight = dgvTheLoai.ClientSize.Height - headerHeight;
            int rowCount = dgvTheLoai.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvTheLoai.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void dgvTheLoai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaTheLoai.Text = dgvTheLoai.Rows[e.RowIndex].Cells["MaTheLoai"].Value.ToString();
                txtTenTheLoai.Text = dgvTheLoai.Rows[e.RowIndex].Cells["TenTheLoai"].Value.ToString();
               
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tên Thể Loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(txtTenTheLoai.Text, @"^[\p{L}\s]+$"))
                {
                    MessageBox.Show("Tên thể loại chỉ chứa chữ cái và khoảng trắng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string maTheLoai = txtMaTheLoai.Text.Trim();
                if (string.IsNullOrEmpty(maTheLoai))
                {
                    maTheLoai = "TL" + (db.TheLoaiDatabase.Count() + 1).ToString("D3");
                }
                else if (!Regex.IsMatch(maTheLoai, @"^TL\d{3}$"))
                {
                    MessageBox.Show("Mã thể loại không đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaTheLoai.Focus();
                    return;
                }

                if (db.TheLoaiDatabase.Any(tl => tl.MaTheLoai == maTheLoai))
                {
                    MessageBox.Show("Mã thể loại đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var theLoai = new TheLoai { MaTheLoai = maTheLoai, TenTheLoai = txtTenTheLoai.Text };
                db.TheLoaiDatabase.Add(theLoai);
                db.SaveChanges();
                MessageBox.Show("Thêm thể loại thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachTheLoai();
                ResizeRowsToFill();
                btnLamMoi_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm thể loại:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var theLoai = db.TheLoaiDatabase.Find(txtMaTheLoai.Text);
            if (theLoai != null)
            {
                if (theLoai.TenTheLoai == txtTenTheLoai.Text.Trim())
                {
                    MessageBox.Show("Không có dữ liệu nào thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                theLoai.TenTheLoai = txtTenTheLoai.Text.Trim();

                db.SaveChanges();
                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachTheLoai();
                ResizeRowsToFill();
                btnLamMoi_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thể loại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi sửa thể loại:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var theLoai = db.TheLoaiDatabase.Find(txtMaTheLoai.Text);
            if (theLoai != null)
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    db.TheLoaiDatabase.Remove(theLoai);
                    db.SaveChanges();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTheLoai();
                    ResizeRowsToFill();
                    btnLamMoi_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thể loại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi xóa thể loại:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTheLoai.Clear();
            txtTenTheLoai.Clear();
            txtMaTheLoai.Focus();
        }

        private void QuanLyTheLoai_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachTheLoai();  // Nếu có tìm kiếm
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachTheLoai();  // Nếu có tìm kiếm
            }
        }
    }
}