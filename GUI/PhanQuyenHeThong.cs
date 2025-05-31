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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GUI
{
    public partial class PhanQuyenHeThong : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        public PhanQuyenHeThong()
        {
            InitializeComponent();

        }

        private void PhanQuyenHeThong_Load(object sender, EventArgs e)
        {
            try
            {
                LoadNguoiDung();
                SetupDataGridView();
                dgvPhanQuyen.DataBindingComplete += dgvPhanQuyen_DataBindingComplete;

                // Load quyền cho tài khoản đầu tiên (nếu có)  
                if (comboTaiKhoan.Items.Count > 0)
                {
                    comboTaiKhoan.SelectedIndex = 0; // Chọn mục đầu tiên  
                    LoadPhanQuyen(comboTaiKhoan.SelectedItem.ToString()); // Tải quyền cho tài khoản đầu tiên  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load tài khoản người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvPhanQuyen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ResizeRowsToFill();
        }
        private void LoadNguoiDung()
        {
            try
            {
                // Lấy danh sách người dùng từ cơ sở dữ liệu thông qua LibraryDBContext
                var nguoiDungs = db.NguoiDungDatabase.Where(nd => nd.VaiTro == "Librarian").ToList();

                if (nguoiDungs.Count == 0)
                {
                    MessageBox.Show("Không có người dùng nào trong hệ thống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Xóa tất cả các item trong ComboBox trước khi thêm mới
                comboTaiKhoan.Items.Clear();

                // Thêm mục mặc định "Chọn tài khoản"
                comboTaiKhoan.Items.Add("--- Chọn tài khoản ---");

                // Thêm các tên đăng nhập vào ComboBox
                foreach (var nguoiDung in nguoiDungs)
                {
                    comboTaiKhoan.Items.Add(nguoiDung.TenDangNhap);
                }

                // Nếu ComboBox có item, chọn item đầu tiên là "Chọn tài khoản"
                if (comboTaiKhoan.Items.Count > 0)
                {
                    comboTaiKhoan.SelectedIndex = 0;  // Chọn mục mặc định
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvPhanQuyen.ColumnHeadersHeight;
            int availableHeight = dgvPhanQuyen.ClientSize.Height - headerHeight;
            int rowCount = dgvPhanQuyen.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvPhanQuyen.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private void SetupDataGridView()
        {
            dgvPhanQuyen.AutoGenerateColumns = false;
            dgvPhanQuyen.Columns.Clear();

            dgvPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên Chức Năng",
                DataPropertyName = "TenChucNang",
                Name = "TenChucNang",
                ReadOnly = true
            });

            dgvPhanQuyen.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Cấp Quyền",
                DataPropertyName = "DuocTruyCap",
                Name = "CapQuyen"
            });

            // ✅ Thêm cột MaChucNang ẩn đi để xử lý khi lưu
            dgvPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaChucNang",
                Name = "MaChucNang",
                Visible = false
            });

            dgvPhanQuyen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhanQuyen.EditMode = DataGridViewEditMode.EditOnEnter;
        }
        private void LoadPhanQuyen(string username)
        {
            try
            {
                // Tạo danh sách quyền từ CSDL
                var danhSachPhanQuyen = db.ChucNangDatabase
                    .Select(cn => new QLTV_DTO.PhanQuyen
                    {
                        MaChucNang = cn.MaChucNang,
                        TenChucNang = cn.TenChucNang,
                        TenDangNhap = username,
                        DuocTruyCap = db.PhanQuyenDatabase
                            .Any(pq => pq.TenDangNhap == username && pq.MaChucNang == cn.MaChucNang)
                    })
                    .ToList();

                // Lưu lại dữ liệu để phân trang
                allData = danhSachPhanQuyen.Cast<object>().ToList();
                totalPages = (int)Math.Ceiling((double)allData.Count / pageSize);
                currentPage = 1;
                LoadPageData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadPageData()
        {
            var pageData = allData.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            dgvPhanQuyen.DataSource = null;
            dgvPhanQuyen.DataSource = pageData;
            ResizeRowsToFill();

            lblPage.Text = $"Trang {currentPage} / {totalPages}";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try { 
            if (comboTaiKhoan.SelectedIndex <= 0) return; // Kiểm tra nếu không có tài khoản nào được chọn  
            string username = comboTaiKhoan.SelectedItem.ToString();

            foreach (DataGridViewRow row in dgvPhanQuyen.Rows)
            {
                // ✅ Phải có cột "MaChucNang" trong DataSource nhưng ẩn đi
                string maChucNang = row.Cells["MaChucNang"].Value?.ToString();
                if (string.IsNullOrEmpty(maChucNang)) continue;

                bool capQuyen = Convert.ToBoolean(row.Cells["CapQuyen"].Value);

                // Kiểm tra quyền hiện tại trong DB
                var quyenHienTai = db.PhanQuyenDatabase
                    .FirstOrDefault(p => p.TenDangNhap == username && p.MaChucNang == maChucNang);

                if (capQuyen && quyenHienTai == null)
                {
                    // Thêm mới nếu chưa có
                    db.PhanQuyenDatabase.Add(new PhanQuyen
                    {
                        TenDangNhap = username,
                        MaChucNang = maChucNang,
                        DuocTruyCap = true
                    });
                }
                else if (!capQuyen && quyenHienTai != null)
                {
                    // Gỡ quyền nếu có sẵn mà bị bỏ chọn
                    db.PhanQuyenDatabase.Remove(quyenHienTai);
                }
            }

           
            
                db.SaveChanges();
                MessageBox.Show("Cập nhật quyền truy cập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPhanQuyen(username); // Load lại dữ liệu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu quyền: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  
                

        private void comboTaiKhoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTaiKhoan.SelectedIndex > 0)
            {
                LoadPhanQuyen(comboTaiKhoan.SelectedItem.ToString());
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


