using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using QLTV_DTO;

namespace GUI
{
    public partial class QuanLyDocGia : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int currentPage = 1;
        private int pageSize = 5;    // Số dòng mỗi trang
        private int totalPages = 1;
        public QuanLyDocGia()
        {
            InitializeComponent();
            LoadDocGia();
            ResetForm();

            dgvDocGia.CellClick += dgvDocGia_CellClick;
            dgvDocGia.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadDocGia()
        {
            try
            {  // 1. Lấy query gốc, chưa ToList()
                var query = db.DocGiaDatabase
                    .Select(d => new
                    {
                        d.MaDocGia,
                        d.HoTen,
                        d.GioiTinh,
                        d.NgaySinh,
                        d.SoDienThoai,
                        d.DiaChi,
                        d.Email,
                        d.NgayDangKy
                    })
                    .AsQueryable();

                // 2. Tính tổng bản ghi và tổng trang
                int totalRecords = query.Count();
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // 3. Đảm bảo currentPage nằm trong [1…totalPages]
                if (currentPage < 1) currentPage = 1;
                if (currentPage > totalPages) currentPage = totalPages;

                // 4. Lấy dữ liệu trang hiện tại
                var pageData = query
                    .OrderBy(d => d.MaDocGia)                        // Bắt buộc OrderBy trước Skip
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // 5. Bind vào DataGridView
                dgvDocGia.DataSource = pageData;
                ResizeRowsToFill();

                // 6. Format lại các cột
                dgvDocGia.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                dgvDocGia.Columns["HoTen"].HeaderText = "Họ và Tên";
                dgvDocGia.Columns["GioiTinh"].HeaderText = "Giới Tính";
                dgvDocGia.Columns["NgaySinh"].HeaderText = "Ngày Sinh";
                dgvDocGia.Columns["SoDienThoai"].HeaderText = "Số điện thoại";
                dgvDocGia.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                dgvDocGia.Columns["Email"].HeaderText = "Email";
                dgvDocGia.Columns["NgayDangKy"].HeaderText = "Ngày Đăng Ký";
                dgvDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDocGia.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 7. Cập nhật label hiển thị trang (giả sử bạn có lblPage)
                lblPage.Text = $"Trang {currentPage}/{totalPages}";
            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvDocGia.ColumnHeadersHeight;
            int availableHeight = dgvDocGia.ClientSize.Height - headerHeight;
            int rowCount = dgvDocGia.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvDocGia.Rows)
            {
                row.Height = rowHeight;
            }
        }
        private bool KiemTraDuLieuNhap()
        {
            // Kiểm tra Họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên độc giả.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Regex: chỉ chấp nhận chữ cái và khoảng trắng (có hỗ trợ Unicode để chấp nhận dấu tiếng Việt)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtHoTen.Text.Trim(), @"^[\p{L}\s]+$"))
            {
                MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trắng, không được có số hoặc ký tự đặc biệt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Kiểm tra SĐT
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtSoDienThoai.Text, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Phải có 10 chữ số và bắt đầu bằng số 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return false;
            }

            // Kiểm tra địa chỉ
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Email.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Regex kiểm tra định dạng Email cơ bản
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Định dạng Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Kiểm tra ngày sinh
            if (dtpNgaySinh.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("Ngày sinh phải nhỏ hơn ngày hiện tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpNgaySinh.Focus();
                return false;
            }

            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraDuLieuNhap())
                    return;

                using (var db = new LibraryDBContext())
                {
                    // Tạo mã tự động
                    var danhSachMa = db.DocGiaDatabase
                        .Select(d => d.MaDocGia)
                        .ToList();

                    List<int> cacSo = new List<int>();
                    foreach (var ma in danhSachMa)
                    {
                        if (!string.IsNullOrEmpty(ma) && ma.StartsWith("DG") && ma.Length > 2)
                        {
                            string soChuoi = ma.Substring(2);
                            if (int.TryParse(soChuoi, out int so))
                            {
                                cacSo.Add(so);
                            }
                        }
                    }

                    int maxSo = (cacSo.Count > 0) ? cacSo.Max() : 0;
                    int soMoi = maxSo + 1;
                    string maMoi = $"DG{soMoi:D2}";

                    // Kiểm tra email đã tồn tại chưa
                    string email = txtEmail.Text.Trim();
                    if (db.DocGiaDatabase.Any(d => d.Email == email))
                    {
                        MessageBox.Show("Email đã tồn tại. Vui lòng nhập email khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var docGia = new QLTV_Database.DocGia
                    {
                        MaDocGia = maMoi,
                        HoTen = txtHoTen.Text.Trim(),
                        NgaySinh = dtpNgaySinh.Value,
                        GioiTinh = radNam.Checked ? "Nam" : "Nữ",
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        DiaChi = txtDiaChi.Text.Trim(),
                        Email = email,
                        NgayDangKy = DateTime.Now
                    };

                    db.DocGiaDatabase.Add(docGia);
                    db.SaveChanges();

                    LoadDocGia();
                    ResizeRowsToFill();
                    MessageBox.Show($"Thêm độc giả thành công với mã: {maMoi}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string ma = txtMaDocGia.Text.Trim();
                if (!KiemTraDuLieuNhap()) return;
                var docGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == ma);
                if (docGia != null)
                {
                    string hoTenMoi = txtHoTen.Text.Trim();
                    string gioiTinhMoi = radNam.Checked ? "Nam" : "Nữ";
                    string soDTMoi = txtSoDienThoai.Text.Trim();
                    string diaChiMoi = txtDiaChi.Text.Trim();
                    string emailMoi = txtEmail.Text.Trim();
                    DateTime ngaySinhMoi = dtpNgaySinh.Value;

                    // Kiểm tra nếu không có thay đổi
                    if (docGia.HoTen == hoTenMoi &&
                        docGia.GioiTinh == gioiTinhMoi &&
                        docGia.SoDienThoai == soDTMoi &&
                        docGia.DiaChi == diaChiMoi &&
                        docGia.Email == emailMoi &&
                        docGia.NgaySinh.HasValue && docGia.NgaySinh.Value.Date == ngaySinhMoi.Date)
                    {
                        MessageBox.Show("Bạn chưa thay đổi thông tin nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Cập nhật nếu có thay đổi
                    docGia.HoTen = hoTenMoi;
                    docGia.GioiTinh = gioiTinhMoi;
                    docGia.SoDienThoai = soDTMoi;
                    docGia.DiaChi = diaChiMoi;
                    docGia.Email = emailMoi;
                    docGia.NgaySinh = ngaySinhMoi;

                    db.SaveChanges();
                    LoadDocGia();
                    ResizeRowsToFill();

                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string ma = txtMaDocGia.Text.Trim();
                var docGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == ma);
                if (docGia != null)
                {
                    // Kiểm tra độc giả có phiếu mượn không
                    bool coPhieuMuon = db.PhieuMuonDatabase.Any(p => p.MaDocGia == ma);
                    if (coPhieuMuon)
                    {
                        MessageBox.Show("Không thể xóa độc giả vì đang có phiếu mượn trong hệ thống.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa độc giả này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.DocGiaDatabase.Remove(docGia);
                        db.SaveChanges();
                        LoadDocGia();
                        ResizeRowsToFill();

                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy độc giả để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
                ResizeRowsToFill();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetForm()
        {
            txtMaDocGia.Clear();
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear(); // Thêm dòng này
            radNam.Checked = true;
            dtpNgaySinh.Value = DateTime.Today;
        }
        private void dgvDocGia_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtMaDocGia.Text = dgvDocGia.Rows[e.RowIndex].Cells["MaDocGia"].Value.ToString();
                    txtHoTen.Text = dgvDocGia.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();
                    txtSoDienThoai.Text = dgvDocGia.Rows[e.RowIndex].Cells["SoDienThoai"].Value.ToString();
                    txtDiaChi.Text = dgvDocGia.Rows[e.RowIndex].Cells["DiaChi"].Value.ToString();
                    txtEmail.Text = dgvDocGia.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                    dtpNgaySinh.Value = Convert.ToDateTime(dgvDocGia.Rows[e.RowIndex].Cells["NgaySinh"].Value);
                    string gioiTinh = dgvDocGia.Rows[e.RowIndex].Cells["GioiTinh"].Value.ToString();
                    radNam.Checked = gioiTinh == "Nam";
                    radNu.Checked = gioiTinh == "Nữ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn độc giả: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QuanLyDocGia_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDocGia();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDocGia();
            }
        }
    }
}
