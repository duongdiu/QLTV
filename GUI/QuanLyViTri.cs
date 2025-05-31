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
    public partial class QuanLyViTri : Form
    {
        LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1; // Trang hiện tại
        private int totalPages = 1;  // Tổng số trang
        private int pageSize = 5;   // Số lượng bản ghi mỗi trang
        public QuanLyViTri()
        {
            InitializeComponent();
            LoadDanhSachViTri();
            dgvViTri.CellClick += dgvViTri_CellClick;
            dgvViTri.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void LoadDanhSachViTri(string tuKhoa = "")
        {
            try
            {
                var query = db.ViTriDatabase.AsQueryable();

                // Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(vt => vt.MoTa.ToLower().Contains(tuKhoa));
                }

                // Tính tổng số bản ghi và tổng số trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Đảm bảo currentPage nằm trong [1...totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // Lấy dữ liệu theo trang hiện tại
                var pageData = query
                    .OrderBy(vt => vt.MaViTri)  // Sắp xếp trước khi phân trang
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .Select(vt => new
                    {
                        vt.MaViTri,
                        vt.Kho,
                        vt.Tu,
                        vt.Ke,
                        vt.Tang,
                        vt.MoTa
                    })
                    .ToList();

                // Bind vào DataGridView
                dgvViTri.DataSource = pageData;
                ResizeRowsToFill();

                // Đặt tên cột trong DataGridView
                dgvViTri.Columns["MaViTri"].HeaderText = "Mã vị trí";
                dgvViTri.Columns["Kho"].HeaderText = "Kho";
                dgvViTri.Columns["Tu"].HeaderText = "Tủ";
                dgvViTri.Columns["Ke"].HeaderText = "Kệ";
                dgvViTri.Columns["Tang"].HeaderText = "Tầng";
                dgvViTri.Columns["MoTa"].HeaderText = "Mô tả";
                dgvViTri.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvViTri.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            int headerHeight = dgvViTri.ColumnHeadersHeight;
            int availableHeight = dgvViTri.ClientSize.Height - headerHeight;
            int rowCount = dgvViTri.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvViTri.Rows)
            {
                row.Height = rowHeight;
            }
        }


        private void ResetForm()
        {
            txtMaViTri.Clear();
            txtKho.Clear();
            txtTu.Clear();
            txtKe.Clear();
            txtTang.Clear();
            txtMoTa.Clear();
            txtMaViTri.Focus();
        }


        private void QuanLyViTri_Load(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTu.Text) || string.IsNullOrWhiteSpace(txtKe.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maViTri = txtMaViTri.Text.Trim();
                if (string.IsNullOrEmpty(maViTri))
                {
                    maViTri = "VT" + (db.ViTriDatabase.Count() + 1).ToString("D2");
                }

                var viTri = new ViTri
                {
                    MaViTri = maViTri,
                    Kho = txtKho.Text,
                    Tu = txtTu.Text,
                    Ke = txtKe.Text,
                    Tang = txtTang.Text,
                    MoTa = txtMoTa.Text
                };

                db.ViTriDatabase.Add(viTri);
                db.SaveChanges();
                MessageBox.Show("Thêm vị trí thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachViTri();
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
                // Validate dữ liệu trước khi sửa
                var viTri = db.ViTriDatabase.Find(txtMaViTri.Text);
                if (viTri != null)
                {
                    // Kiểm tra xem có thay đổi gì không
                    bool isChanged = false;

                    if (viTri.Tu != txtTu.Text.Trim())
                    {
                        viTri.Tu = txtTu.Text.Trim();
                        isChanged = true;
                    }
                    if (viTri.Kho != txtKho.Text.Trim())
                    {
                        viTri.Kho = txtKho.Text.Trim();
                        isChanged = true;
                    }

                    if (viTri.Ke != txtKe.Text.Trim())
                    {
                        viTri.Ke = txtKe.Text.Trim();
                        isChanged = true;
                    }

                    if (viTri.Tang != txtTang.Text.Trim())
                    {
                        viTri.Tang = txtTang.Text.Trim();
                        isChanged = true;
                    }

                    if (viTri.MoTa != txtMoTa.Text.Trim())
                    {
                        viTri.MoTa = txtMoTa.Text.Trim();
                        isChanged = true;
                    }

                    // Nếu không có thay đổi nào, thông báo cho người dùng
                    if (!isChanged)
                    {
                        MessageBox.Show("Không có dữ liệu nào thay đổi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Nếu có thay đổi, lưu lại
                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachViTri();
                    ResizeRowsToFill();
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vị trí!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var viTri = db.ViTriDatabase.Find(txtMaViTri.Text);
                if (viTri != null)
                {
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.ViTriDatabase.Remove(viTri);
                        db.SaveChanges();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachViTri();
                        ResizeRowsToFill();
                        ResetForm();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy vị trí!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void txtTang_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvViTri_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaViTri.Text = dgvViTri.Rows[e.RowIndex].Cells["MaViTri"].Value.ToString();
                txtKho.Text = dgvViTri.Rows[e.RowIndex].Cells["Kho"].Value?.ToString() ?? "";
                txtTu.Text = dgvViTri.Rows[e.RowIndex].Cells["Tu"].Value.ToString();
                txtKe.Text = dgvViTri.Rows[e.RowIndex].Cells["Ke"].Value.ToString();
                txtTang.Text = dgvViTri.Rows[e.RowIndex].Cells["Tang"].Value.ToString();
                txtMoTa.Text = dgvViTri.Rows[e.RowIndex].Cells["MoTa"].Value.ToString();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachViTri();  // Nếu có tìm kiếm
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachViTri();  // Nếu có tìm kiếm
            }
        }
    }
}
