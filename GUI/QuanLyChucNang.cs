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
    public partial class QuanLyChucNang : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        public QuanLyChucNang()
        {
            InitializeComponent();
            LoadChucNang();
            ResetForm();
            dgvChucNang.CellClick += dgvChucNang_CellClick;
            dgvChucNang.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }

        private void QuanLyChucNang_Load(object sender, EventArgs e)
        {
            try
            {
                List<string> allFormNames = new List<string>() {
        "ChiTietMuonTraSach",
        "FormPhieuMuonSach",
        "FormQuanLyBinhLuanSach",
        "FormQuanLyLienHe",
        "FormQuanLyNguoiDung",
        "FormThongKeDocGia",
        "FormThongKeSach",
        "FormTimKiemPhieuMuon",
        "FormTimKiemDocGia",
        "FormTimKiemSach",
        "PhanQuyenHeThong",
        "QuanLyChucNang",
        "QuanLyDocGia",
        "QuanLyLichHen",
        "QuanLyNXB",
        "QuanLySachForm",
        "QuanLyTacGia",
        "QuanLyTheLoai",
        "QuanLyViTri",
        // thêm các tên form khác ở đây  
    };

                // Lấy các mã chức năng đã có trong cơ sở dữ liệu  
                var existingFunctions = db.ChucNangDatabase
                    .Select(c => c.TenForm)
                    .ToList();

                // Lọc ra những form chưa có chức năng  
                var availableForms = allFormNames
                    .Where(formName => !existingFunctions.Contains(formName))
                    .ToList();

                // Thêm phần "Chọn tên form" vào đầu danh sách  
                availableForms.Insert(0, "--Chọn tên form--");

                comboBoxTenForm.DataSource = availableForms;
                comboBoxTenForm.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadChucNang()
        {
            var danhSach = db.ChucNangDatabase
                .Select(c => new
                {
                    c.MaChucNang,
                    c.TenChucNang,
                    c.TenForm
                })
                .ToList();
            // Lưu lại dữ liệu để phân trang
            allData = danhSach.Cast<object>().ToList();
            totalPages = (int)Math.Ceiling((double)allData.Count / pageSize);
            currentPage = 1;
            LoadPageData();
            
        }
        private void LoadPageData()
        {
            var pageData = allData
        .Skip((currentPage - 1) * pageSize)
        .Take(pageSize)
        .ToList();

            dgvChucNang.DataSource = pageData;
            ResizeRowsToFill();

            dgvChucNang.Columns["MaChucNang"].HeaderText = "Mã Chức Năng";
            dgvChucNang.Columns["TenChucNang"].HeaderText = "Tên Chức Năng";
            dgvChucNang.Columns["TenForm"].HeaderText = "Tên Form";
            dgvChucNang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChucNang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            lblPage.Text = $"Trang {currentPage}/{totalPages}";
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvChucNang.ColumnHeadersHeight;
            int availableHeight = dgvChucNang.ClientSize.Height - headerHeight;
            int rowCount = dgvChucNang.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvChucNang.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private string GenerateMaChucNang()
        {
            int count = db.ChucNangDatabase.Count() + 1; // Đếm số lượng chức năng hiện tại  
            return $"CN{count:00}"; // Tạo mã dạng CN01, CN02...  
        }

        private void dgvChucNang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra có chọn hàng  
            {
                var row = dgvChucNang.Rows[e.RowIndex];
                txtMaChucNang.Text = row.Cells["MaChucNang"].Value.ToString();
                txtTenChucNang.Text = row.Cells["TenChucNang"].Value.ToString();

                string selectedForm = row.Cells["TenForm"].Value.ToString();

                // Sử dụng IndexOf để tìm chỉ số thay vì Contains  
                int index = comboBoxTenForm.Items.IndexOf(selectedForm);
                if (index >= 0) // Kiểm tra xem có tìm thấy không  
                {
                    comboBoxTenForm.SelectedIndex = index; // Gán chỉ số  
                }
                else
                {
                    comboBoxTenForm.SelectedIndex = 0; // Đặt về giá trị mặc định  
                }
            }
        }
        private void UpdateComboBoxFormNames()
        {
            List<string> allFormNames = new List<string>() {
        "ChiTietMuonTraSach",
        "FormPhieuMuonSach",
        "FormQuanLyBinhLuanSach",
        "FormQuanLyLienHe",
        "FormQuanLyNguoiDung",
        "FormThongKeDocGia",
        "FormThongKeSach",
        "FormTimKiemPhieuMuon",
        "FormTimKiemDocGia",
        "FormTimKiemSach",
        "PhanQuyenHeThong",
        "QuanLyChucNang",
        "QuanLyDocGia",
        "QuanLyLichHen",
        "QuanLyNXB",
        "QuanLySachForm",
        "QuanLyTacGia",
        "QuanLyTheLoai",
    };

            // Lấy các mã chức năng đã có trong cơ sở dữ liệu  
            var existingFunctions = db.ChucNangDatabase
                .Select(c => c.TenForm)
                .ToList();

            // Lọc ra những form chưa có chức năng  
            var availableForms = allFormNames
                .Where(formName => !existingFunctions.Contains(formName))
                .ToList();

            // Thêm phần "Chọn tên form" vào đầu danh sách  
            availableForms.Insert(0, "--Chọn tên form--");

            // Cập nhật DataSource cho ComboBox  
            comboBoxTenForm.DataSource = availableForms;

            // Kiểm tra Item Count trước khi thiết lập SelectedIndex  
            if (availableForms.Count > 0)
            {
                comboBoxTenForm.SelectedIndex = 0; // Đặt chỉ số mặc định là 0  
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào  
                if (string.IsNullOrWhiteSpace(txtTenChucNang.Text) || comboBoxTenForm.SelectedIndex == 0)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var chucNang = new ChucNang
                {
                    MaChucNang = GenerateMaChucNang(),
                    TenChucNang = txtTenChucNang.Text,
                    TenForm = comboBoxTenForm.SelectedItem.ToString()
                };

                db.ChucNangDatabase.Add(chucNang);
                db.SaveChanges();
                LoadChucNang();
                ResizeRowsToFill();
                ResetForm();

                // Cập nhật danh sách form trong ComboBox  
                UpdateComboBoxFormNames();

                MessageBox.Show("Thêm chức năng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chức năng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var maChucNang = txtMaChucNang.Text;
                var chucNang = db.ChucNangDatabase.FirstOrDefault(c => c.MaChucNang == maChucNang);

                if (chucNang != null)
                {
                    if (string.IsNullOrWhiteSpace(txtTenChucNang.Text) || comboBoxTenForm.SelectedIndex == 0)
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    chucNang.TenChucNang = txtTenChucNang.Text;
                    chucNang.TenForm = comboBoxTenForm.SelectedItem.ToString();
                    db.SaveChanges();
                    LoadChucNang();
                    ResizeRowsToFill();
                    ResetForm();
                    MessageBox.Show("Sửa chức năng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có thông tin nào để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa chức năng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var maChucNang = txtMaChucNang.Text;
                var chucNang = db.ChucNangDatabase.FirstOrDefault(c => c.MaChucNang == maChucNang);

                if (chucNang != null)
                {
                    var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa chức năng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        db.ChucNangDatabase.Remove(chucNang);
                        db.SaveChanges();
                        LoadChucNang();
                        ResetForm();
                        UpdateComboBoxFormNames(); // Cập nhật lại ComboBox  
                        MessageBox.Show("Xóa chức năng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Không có thông tin nào để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chức năng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtMaChucNang.Clear();
            txtTenChucNang.Clear();

            // Kiểm tra Item Count trước khi thiết lập SelectedIndex  
            if (comboBoxTenForm.Items.Count > 0)
            {
                comboBoxTenForm.SelectedIndex = 0; // Chỉ thiết lập nếu có item  
            } // Hoặc thiết lập lại giá trị mặc định  
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void comboBoxTenForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTenForm.SelectedIndex == 0)
            {
                // Nếu chọn "Chọn tên form", không làm gì  
                return;
            }
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
    }
}
