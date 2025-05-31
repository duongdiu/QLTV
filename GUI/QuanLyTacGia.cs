using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using QLTV_Database;

namespace GUI
{
    public partial class QuanLyTacGia : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5; // Số bản ghi trên mỗi trang
        private int currentPage = 1; // Trang hiện tại
        private int totalPages = 1; // Tổng số trang
        public QuanLyTacGia()
        {
            InitializeComponent();
            LoadDanhSachTacGia();
            dgvTacGia.CellClick += dgvTacGia_CellClick;
            dgvTacGia.DataBindingComplete += (s, e) => ResizeRowsToFill();

        }
        private void LoadDanhSachTacGia(string tuKhoa = "")
        {
            try
            {
                // 1. Lấy query gốc, chưa ToList()
                var query = db.TacGiaDatabase
                    .Select(tg => new
                    {
                        tg.MaTacGia,
                        tg.TenTacGia,
                        tg.NamSinh,
                        tg.QueQuan
                    })
                    .AsQueryable();

                // 2. Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(tg =>
                        tg.TenTacGia.ToLower().Contains(tuKhoa) ||
                        tg.QueQuan.ToLower().Contains(tuKhoa));
                }

                // 3. Tính tổng bản ghi và tổng trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 4. Đảm bảo currentPage nằm trong [1…totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // 5. Lấy dữ liệu trang hiện tại
                var pageData = query
                    .OrderBy(tg => tg.MaTacGia) // Bắt buộc OrderBy trước Skip
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 6. Bind vào DataGridView
                dgvTacGia.DataSource = pageData;
                ResizeRowsToFill();

                // 7. Đặt tên cột trong DataGridView
                dgvTacGia.Columns["MaTacGia"].HeaderText = "Mã Tác Giả";
                dgvTacGia.Columns["TenTacGia"].HeaderText = "Tên Tác Giả";
                dgvTacGia.Columns["NamSinh"].HeaderText = "Năm Sinh";
                dgvTacGia.Columns["QueQuan"].HeaderText = "Quê Quán";

                dgvTacGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTacGia.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 8. Cập nhật label hiển thị trang (giả sử bạn có lblPage)
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvTacGia.ColumnHeadersHeight;
            int availableHeight = dgvTacGia.ClientSize.Height - headerHeight;
            int rowCount = dgvTacGia.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvTacGia.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void dgvTacGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaTacGia.Text = dgvTacGia.Rows[e.RowIndex].Cells["MaTacGia"].Value.ToString();
                txtTenTacGia.Text = dgvTacGia.Rows[e.RowIndex].Cells["TenTacGia"].Value.ToString();
                txtNamSinh.Text = dgvTacGia.Rows[e.RowIndex].Cells["NamSinh"].Value.ToString();
                txtQueQuan.Text = dgvTacGia.Rows[e.RowIndex].Cells["QueQuan"].Value.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenTacGia.Text))
                {
                    MessageBox.Show("Vui lòng nhập Tên Tác Giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra Tên tác giả: Không chứa số hoặc ký tự đặc biệt
                if (!Regex.IsMatch(txtTenTacGia.Text, @"^[\p{L}\s]+$"))
                {
                    MessageBox.Show("Tên tác giả chỉ được chứa chữ cái và khoảng trắng, không chứa số hoặc ký tự đặc biệt!",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                // Kiểm tra Năm sinh: Chỉ được nhập số, trong khoảng hợp lệ
                if (!int.TryParse(txtNamSinh.Text, out int namSinh) || namSinh < 1800 || namSinh > DateTime.Now.Year)
                {
                    MessageBox.Show("Năm sinh phải là số hợp lệ từ 1800 đến năm hiện tại!",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra Quê quán: Không chứa ký tự đặc biệt (trừ '/')
                if (!Regex.IsMatch(txtQueQuan.Text, @"^[\p{L}\s/]+$"))
                {
                    MessageBox.Show("Quê quán chỉ được chứa chữ cái, khoảng trắng và dấu '/'!",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string maTacGia = txtMaTacGia.Text.Trim();
                if (string.IsNullOrEmpty(maTacGia))
                {
                    // Tự động tạo mã tác giả nếu không nhập
                    maTacGia = "TG" + (db.TacGiaDatabase.Count() + 1).ToString("D3"); // VD: TG001, TG002, ...
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(maTacGia, @"^TG\d{3}$"))
                {
                    MessageBox.Show("Mã Tác Giả không đúng định dạng! Vui lòng nhập theo dạng TGxxx (VD: TG001)",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaTacGia.Focus();
                    return;
                }

                if (db.TacGiaDatabase.Any(tg => tg.MaTacGia == maTacGia))
                {
                    MessageBox.Show("Mã Tác Giả đã tồn tại! Vui lòng nhập mã khác.",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaTacGia.Focus();
                    return;
                }

                var tacGia = new TacGia
                {
                    MaTacGia = maTacGia,
                    TenTacGia = txtTenTacGia.Text,
                    NamSinh = namSinh,
                    QueQuan = txtQueQuan.Text
                };

                db.TacGiaDatabase.Add(tacGia);
                db.SaveChanges();

                MessageBox.Show($"Thêm tác giả thành công! Mã: {maTacGia}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachTacGia(); // Cập nhật danh sách
                ResizeRowsToFill();
                btnLamMoi_Click(sender, e); // Xóa dữ liệu trên form
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
                var tacGia = db.TacGiaDatabase.Find(txtMaTacGia.Text);
                if (tacGia != null)
                {
                    // Kiểm tra Tên tác giả: Không chứa số hoặc ký tự đặc biệt
                    if (!Regex.IsMatch(txtTenTacGia.Text, @"^[\p{L}\s]+$"))
                    {
                        MessageBox.Show("Tên tác giả chỉ được chứa chữ cái và khoảng trắng, không chứa số hoặc ký tự đặc biệt!",
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra Năm sinh: Chỉ được nhập số, trong khoảng hợp lệ
                    if (!int.TryParse(txtNamSinh.Text, out int namSinh) || namSinh < 1800 || namSinh > DateTime.Now.Year)
                    {
                        MessageBox.Show("Năm sinh phải là số hợp lệ từ 1800 đến năm hiện tại!",
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra Quê quán: Không chứa ký tự đặc biệt (trừ '/')
                    if (!Regex.IsMatch(txtQueQuan.Text, @"^[\p{L}\s/]+$"))
                    {
                        MessageBox.Show("Quê quán chỉ được chứa chữ cái, khoảng trắng và dấu '/'!",
                                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Kiểm tra xem có thay đổi dữ liệu không
                    if (tacGia.TenTacGia == txtTenTacGia.Text.Trim() &&
                        tacGia.QueQuan == txtQueQuan.Text.Trim() &&
                        tacGia.NamSinh == namSinh)
                    {
                        MessageBox.Show("Không có dữ liệu nào thay đổi!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Cập nhật dữ liệu mới
                    tacGia.TenTacGia = txtTenTacGia.Text.Trim();
                    tacGia.QueQuan = txtQueQuan.Text.Trim();
                    tacGia.NamSinh = namSinh;

                    db.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachTacGia();
                    ResizeRowsToFill();
                    btnLamMoi_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tác giả!",
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var tacGia = db.TacGiaDatabase.Find(txtMaTacGia.Text);
                if (tacGia != null)
                {
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.TacGiaDatabase.Remove(tacGia);
                        db.SaveChanges();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachTacGia();
                        ResizeRowsToFill();
                        btnLamMoi_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tác giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaTacGia.Clear();
            txtTenTacGia.Clear();
            txtNamSinh.Clear();
            txtQueQuan.Clear();
            txtMaTacGia.Focus();
        }

        private void QuanLyTacGia_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachTacGia();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachTacGia();
            }
        }
    }

    }

