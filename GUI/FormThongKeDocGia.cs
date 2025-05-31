using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using Excel = Microsoft.Office.Interop.Excel;

namespace GUI
{
    public partial class FormThongKeDocGia : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        public FormThongKeDocGia()
        {
            InitializeComponent();
            cboThongKeDocGia.Items.Add("Tất cả độc giả");
            cboThongKeDocGia.Items.Add("Độc giả đang mượn sách");
            cboThongKeDocGia.Items.Add("Độc giả trễ hạn");
            cboThongKeDocGia.Items.Add("Độc giả đã trả hết");
            cboThongKeDocGia.SelectedIndex = 0;
            LoadDanhSachDocGia();
            dgvThongKeDocGia.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadDanhSachDocGia(string tuKhoa = "", string trangThai = "Tất cả độc giả")
        {
            try
            {
                var query = from dg in db.DocGiaDatabase
                            join pm in db.PhieuMuonDatabase on dg.MaDocGia equals pm.MaDocGia into gj
                            from subPm in gj.DefaultIfEmpty()
                            group new { dg, subPm } by dg into grouped
                            select new
                            {
                                MaDocGia = grouped.Key.MaDocGia,
                                TenDocGia = grouped.Key.HoTen,
                                Email = grouped.Key.Email,
                                SoDienThoai = grouped.Key.SoDienThoai,
                                SoLuongMuon = grouped.Count(p => p.subPm != null),
                                TinhTrang = grouped.Any(p => p.subPm != null)
                                            ? (grouped.Any(p => p.subPm.NgayTraDuKien < DateTime.Now) ? "Trễ hạn" : "Đang mượn")
                                            : "Đã trả hết"
                            };

                // Lọc trạng thái
                if (trangThai == "Độc giả đang mượn sách")
                    query = query.Where(d => d.TinhTrang == "Đang mượn");
                else if (trangThai == "Độc giả trễ hạn")
                    query = query.Where(d => d.TinhTrang == "Trễ hạn");
                else if (trangThai == "Độc giả đã trả hết")
                    query = query.Where(d => d.TinhTrang == "Đã trả hết");

                // Tìm kiếm theo từ khóa
                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(d => d.TenDocGia.ToLower().Contains(tuKhoa)
                                          || d.MaDocGia.ToLower().Contains(tuKhoa));
                }

                // Lấy tổng số bản ghi
                var totalRecords = query.Count();

                // Tính số trang
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Thêm phương thức OrderBy để sắp xếp theo một trường (ví dụ: theo MaDocGia)
                query = query.OrderBy(d => d.MaDocGia); // Bạn có thể thay 'MaDocGia' bằng trường khác nếu cần

                // Lấy dữ liệu cho trang hiện tại
                var pagedData = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Hiển thị dữ liệu lên DataGridView
                dgvThongKeDocGia.DataSource = pagedData;
                ResizeRowsToFill();
                // Cập nhật header và các cột
                dgvThongKeDocGia.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                dgvThongKeDocGia.Columns["TenDocGia"].HeaderText = "Tên Độc Giả";
                dgvThongKeDocGia.Columns["Email"].HeaderText = "Email";
                dgvThongKeDocGia.Columns["SoDienThoai"].HeaderText = "Số Điện Thoại";
                dgvThongKeDocGia.Columns["SoLuongMuon"].HeaderText = "Số Lượng Mượn";
                dgvThongKeDocGia.Columns["TinhTrang"].HeaderText = "Tình Trạng";
                dgvThongKeDocGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvThongKeDocGia.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Cập nhật thông tin phân trang
                lblPage.Text = $"Trang {currentPage} / {totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResizeRowsToFill()
        {
            int headerHeight = dgvThongKeDocGia.ColumnHeadersHeight;
            int availableHeight = dgvThongKeDocGia.ClientSize.Height - headerHeight;
            int rowCount = dgvThongKeDocGia.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvThongKeDocGia.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void dgvThongKe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboThongKeDocGia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

        

       

        private void btnDongDG_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        private void btnTimKiemDG_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormThongKeDocGia_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnThongKeDG_Click_1(object sender, EventArgs e)
        {
            LoadDanhSachDocGia("", cboThongKeDocGia.SelectedItem.ToString());
            ResizeRowsToFill();
        }

        private void btnTimKiemDG_Click_1(object sender, EventArgs e)
        {
            LoadDanhSachDocGia(txtTimKiemDG.Text, cboThongKeDocGia.SelectedItem.ToString());
            ResizeRowsToFill();
        }

        private void btnXuatExcelDG_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Tạo một ứng dụng Excel mới  
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                // ======= TIÊU ĐỀ TRANG =======
                excelWorksheet.Cells[1, 1].Value = "BÁO CÁO THỐNG KÊ ĐỘC GIẢ";
                Excel.Range titleRange = excelWorksheet.Range["A1", "F1"];
                titleRange.Merge();
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ======= TIÊU ĐỀ CỘT (ở dòng 2) =======
                excelWorksheet.Cells[2, 1].Value = "Mã Độc Giả";
                excelWorksheet.Cells[2, 2].Value = "Tên Độc Giả";
                excelWorksheet.Cells[2, 3].Value = "Email";
                excelWorksheet.Cells[2, 4].Value = "Số Điện Thoại";
                excelWorksheet.Cells[2, 5].Value = "Số Lượng Mượn";
                excelWorksheet.Cells[2, 6].Value = "Tình Trạng";

                // ======= DỮ LIỆU TỪ DÒNG 3 =======
                int rowStart = 3;
                foreach (DataGridViewRow row in dgvThongKeDocGia.Rows)
                {
                    excelWorksheet.Cells[rowStart, 1].Value = row.Cells["MaDocGia"].Value;
                    excelWorksheet.Cells[rowStart, 2].Value = row.Cells["TenDocGia"].Value;
                    excelWorksheet.Cells[rowStart, 3].Value = row.Cells["Email"].Value;
                    excelWorksheet.Cells[rowStart, 4].Value = row.Cells["SoDienThoai"].Value;
                    excelWorksheet.Cells[rowStart, 5].Value = row.Cells["SoLuongMuon"].Value;
                    excelWorksheet.Cells[rowStart, 6].Value = row.Cells["TinhTrang"].Value;
                    rowStart++;
                }

                // ======= ĐỊNH DẠNG HEADER (hàng 2) =======
                Excel.Range headerRange = excelWorksheet.Range[excelWorksheet.Cells[2, 1], excelWorksheet.Cells[2, 6]];
                headerRange.Font.Bold = true;

                // ======= FOOTER IN ẤN =======
                excelWorksheet.PageSetup.CenterFooter = "Trang &P / &N";
                excelWorksheet.PageSetup.LeftFooter = "Xuất bởi: Quản trị viên";
                excelWorksheet.PageSetup.RightFooter = DateTime.Now.ToString("dd/MM/yyyy");

                // ======= TỰ ĐỘNG CĂN CỘT =======
                excelWorksheet.Columns.AutoFit();

                // ======= LƯU FILE =======
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = "ThongKeDocGia.xlsx";
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        excelWorkbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // ======= ĐÓNG & GIẢI PHÓNG =======
                excelWorkbook.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoiDG_Click_1(object sender, EventArgs e)
        {
            LoadDanhSachDocGia();
            ResizeRowsToFill();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachDocGia();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachDocGia();
            }
        }
    }
}
