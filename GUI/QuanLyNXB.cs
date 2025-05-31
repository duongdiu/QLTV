using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using QLTV_Database;
using QLTV_DTO;

namespace GUI
{
    public partial class QuanLyNXB : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5;    // Số dòng mỗi trang
        private int totalPages = 1;
        public QuanLyNXB()
        {
            InitializeComponent();
            LoadDanhSachNXB();
            dgvNXB.CellClick += dgvNXB_CellClick;
            dgvNXB.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void LoadDanhSachNXB()
        {
            try
            {
                // 1. Lấy query gốc, chưa ToList()
                var query = db.NhaXuatBanDatabase.Select(nxb => new
                {
                    nxb.MaNXB,
                    nxb.TenNXB,
                    nxb.DiaChi,
                    nxb.Email
                }).AsQueryable();

                // 2. Tính tổng bản ghi và tổng trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 3. Đảm bảo currentPage nằm trong [1…totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // 4. Lấy dữ liệu trang hiện tại
                var pageData = query
                    .OrderBy(n => n.MaNXB)                       // Bắt buộc OrderBy trước Skip
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 5. Bind vào DataGridView
                dgvNXB.DataSource = pageData;
                ResizeRowsToFill();

                // 6. Format lại các cột
                dgvNXB.Columns["MaNXB"].HeaderText = "Mã NXB";
                dgvNXB.Columns["TenNXB"].HeaderText = "Tên NXB";
                dgvNXB.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvNXB.Columns["Email"].HeaderText = "Email";
                dgvNXB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvNXB.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            int headerHeight = dgvNXB.ColumnHeadersHeight;
            int availableHeight = dgvNXB.ClientSize.Height - headerHeight;
            int rowCount = dgvNXB.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvNXB.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void dgvNXB_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaNXB.Text = dgvNXB.Rows[e.RowIndex].Cells["MaNXB"].Value.ToString();
                txtTenNXB.Text = dgvNXB.Rows[e.RowIndex].Cells["TenNXB"].Value.ToString();
                txtDiaChi.Text = dgvNXB.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
                txtEmail.Text = dgvNXB.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenNXB.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tên Nhà Xuất Bản!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!Regex.IsMatch(txtTenNXB.Text, @"^[\p{L}\s]+$"))
                {
                    MessageBox.Show("Tên NXB chỉ chứa chữ cái và khoảng trắng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Regex.IsMatch(txtDiaChi.Text, @"^[\p{L}\d\s,/-]+$"))
                {
                    MessageBox.Show("Địa chỉ không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
                {
                    MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string maNXB = txtMaNXB.Text.Trim();
                if (string.IsNullOrEmpty(maNXB))
                {
                    maNXB = "NXB" + (db.NhaXuatBanDatabase.Count() + 1).ToString("D3");
                }
                else if (!Regex.IsMatch(maNXB, @"^NXB\d{3}$"))
                {
                    MessageBox.Show("Mã NXB không đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaNXB.Focus();
                    return;
                }

                if (db.NhaXuatBanDatabase.Any(NhaXuatBan => NhaXuatBan.MaNXB == maNXB))
                {
                    MessageBox.Show("Mã NXB đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var nxb = new QLTV_Database.NhaXuatBan { MaNXB = maNXB, TenNXB = txtTenNXB.Text, DiaChi = txtDiaChi.Text, Email = txtEmail.Text };
                db.NhaXuatBanDatabase.Add(nxb);
                db.SaveChanges();
                MessageBox.Show("Thêm NXB thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachNXB();
                ResizeRowsToFill();
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var nxb = db.NhaXuatBanDatabase.Find(txtMaNXB.Text);
                if (nxb != null)
                {
                    if (nxb.TenNXB == txtTenNXB.Text.Trim() && nxb.DiaChi == txtDiaChi.Text.Trim() && nxb.Email == txtEmail.Text.Trim())
                    {
                        MessageBox.Show("Không có dữ liệu nào thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    nxb.TenNXB = txtTenNXB.Text.Trim();
                    nxb.DiaChi = txtDiaChi.Text.Trim();
                    nxb.Email = txtEmail.Text.Trim();
                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachNXB();
                    ResizeRowsToFill();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy NXB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var nxb = db.NhaXuatBanDatabase.Find(txtMaNXB.Text);
                if (nxb != null)
                {
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.NhaXuatBanDatabase.Remove(nxb);
                        db.SaveChanges();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachNXB();
                        ResizeRowsToFill();
                        ResetForm();


                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy NXB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetForm()
        {
            txtMaNXB.Clear();
            txtTenNXB.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtMaNXB.Focus();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNXB.Clear();
            txtTenNXB.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtMaNXB.Focus();
            ResizeRowsToFill();
        }

        private void QuanLyNXB_DragLeave(object sender, EventArgs e)
        {

        }

        private void QuanLyNXB_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachNXB();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachNXB();
            }
        }
    }
}
