using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using Excel = Microsoft.Office.Interop.Excel;

namespace GUI
{
    public partial class FormThongKePhieuMuon : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        public FormThongKePhieuMuon()
        {
            InitializeComponent();
            cboTinhTrang.Items.Add("Tất cả");
            cboTinhTrang.Items.Add("Đang mượn");
            cboTinhTrang.Items.Add("Đã trả");
            cboTinhTrang.Items.Add("Quá hạn");
            cboTinhTrang.SelectedIndex = 0;

            dtpTuNgay.Value = DateTime.Today.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Today;
            dgvThongKe.DataBindingComplete += (s, e) => ResizeRowsToFill();
        }
        private void LoadPhieuMuon()
        {
            try
            {
                string tinhTrang = cboTinhTrang.SelectedItem.ToString();
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date;

                var query = from pm in db.PhieuMuonDatabase
                            join dg in db.DocGiaDatabase on pm.MaDocGia equals dg.MaDocGia
                            join ct in db.ChiTietPhieuMuonDatabase
                    on pm.MaPhieuMuon equals ct.MaPhieuMuon into ctGroup
                            from ct in ctGroup.OrderByDescending(c => c.NgayTra).Take(1).DefaultIfEmpty()
                            where pm.NgayMuon >= tuNgay && pm.NgayMuon <= denNgay
                            select new
                            {
                                pm.MaPhieuMuon,
                                pm.MaDocGia,
                                TenDocGia = dg.HoTen,
                                NgayMuon = pm.NgayMuon,
                                NgayTraDuKien = pm.NgayTraDuKien,
                                NgayTra = ct.NgayTra,
                                pm.TienPhat,
                                pm.TinhTrang

                            };

                // Lọc theo tình trạng
                if (tinhTrang != "Tất cả")
                {
                    query = query.Where(p => p.TinhTrang == tinhTrang);
                }

                // Lấy tổng số bản ghi
                var totalRecords = query.Count();

                // Tính số trang
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Thêm phương thức OrderBy để sắp xếp theo MaPhieuMuon (hoặc trường khác nếu cần)
                query = query.OrderBy(p => p.MaPhieuMuon); // Hoặc thay đổi trường theo yêu cầu của bạn

                // Lấy dữ liệu cho trang hiện tại
                var pagedData = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                // Hiển thị dữ liệu lên DataGridView
                dgvThongKe.DataSource = pagedData;
                ResizeRowsToFill();
                // Cập nhật header cho các cột
                dgvThongKe.Columns["MaPhieuMuon"].HeaderText = "Mã Phiếu Mượn";
                dgvThongKe.Columns["MaDocGia"].HeaderText = "Mã Độc Giả";
                dgvThongKe.Columns["TenDocGia"].HeaderText = "Tên Độc Giả";
                dgvThongKe.Columns["NgayMuon"].HeaderText = "Ngày Mượn";
                dgvThongKe.Columns["NgayTra"].HeaderText = "Ngày Trả";
                dgvThongKe.Columns["NgayTraDuKien"].HeaderText = "Ngày Trả Dự Kiến";
                dgvThongKe.Columns["TienPhat"].HeaderText = "Tiền Phạt";
                dgvThongKe.Columns["TinhTrang"].HeaderText = "Tình Trạng";

                dgvThongKe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvThongKe.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            int headerHeight = dgvThongKe.ColumnHeadersHeight;
            int availableHeight = dgvThongKe.ClientSize.Height - headerHeight;
            int rowCount = dgvThongKe.Rows.Count;

            if (rowCount == 0) return;

            int rowHeight = availableHeight / rowCount;

            // Đảm bảo chiều cao hàng tối thiểu là 1
            rowHeight = Math.Max(rowHeight, 1);

            // Gán chiều cao cho tất cả các hàng
            foreach (DataGridViewRow row in dgvThongKe.Rows)
            {
                row.Height = rowHeight;
            }
        }

        private void FormThongKePhieuMuon_Load(object sender, EventArgs e)
        {
            LoadPhieuMuon();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            LoadPhieuMuon();
            ResizeRowsToFill();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboTinhTrang.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Today.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Today;
            LoadPhieuMuon();
            ResizeRowsToFill();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo ứng dụng Excel
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                // ======= TIÊU ĐỀ TRANG =======
                excelWorksheet.Cells[1, 1].Value = "BÁO CÁO THỐNG KÊ PHIẾU MƯỢN";
                Excel.Range titleRange = excelWorksheet.Range["A1", "G1"];
                titleRange.Merge();
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ======= TIÊU ĐỀ CỘT (từ dòng 2) =======
                excelWorksheet.Cells[2, 1].Value = "Mã Phiếu Mượn";
                excelWorksheet.Cells[2, 2].Value = "Mã Độc Giả";
                excelWorksheet.Cells[2, 3].Value = "Ngày Mượn";
                excelWorksheet.Cells[2, 4].Value = "Ngày Trả";
                excelWorksheet.Cells[2, 5].Value = "Ngày Trả Dự Kiến";
                excelWorksheet.Cells[2, 6].Value = "Tiền Phạt";
                excelWorksheet.Cells[2, 7].Value = "Tình Trạng";

                // ======= DỮ LIỆU TỪ DÒNG 3 =======
                int rowStart = 3;
                foreach (DataGridViewRow row in dgvThongKe.Rows)
                {
                    excelWorksheet.Cells[rowStart, 1].Value = row.Cells["MaPhieuMuon"].Value;
                    excelWorksheet.Cells[rowStart, 2].Value = row.Cells["MaDocGia"].Value;
                    excelWorksheet.Cells[rowStart, 3].Value = row.Cells["NgayMuon"].Value;
                    excelWorksheet.Cells[rowStart, 4].Value = row.Cells["NgayTra"].Value;
                    excelWorksheet.Cells[rowStart, 5].Value = row.Cells["NgayTraDuKien"].Value;
                    excelWorksheet.Cells[rowStart, 6].Value = row.Cells["TienPhat"].Value;
                    excelWorksheet.Cells[rowStart, 7].Value = row.Cells["TinhTrang"].Value;
                    rowStart++;
                }

                // ======= ĐỊNH DẠNG HEADER =======
                Excel.Range headerRange = excelWorksheet.Range[excelWorksheet.Cells[2, 1], excelWorksheet.Cells[2, 7]];
                headerRange.Font.Bold = true;

                // ======= FOOTER IN ẤN =======
                excelWorksheet.PageSetup.CenterFooter = "Trang &P / &N";
                excelWorksheet.PageSetup.LeftFooter = "Xuất bởi: Quản trị viên";
                excelWorksheet.PageSetup.RightFooter = DateTime.Now.ToString("dd/MM/yyyy");

                // ======= TỰ ĐỘNG CĂN CỘT =======
                excelWorksheet.Columns.AutoFit();


                // Hộp thoại lưu
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.FileName = "ThongKePhieuMuon.xlsx";
                    saveDialog.Filter = "Excel Files|*.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        excelWorkbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Đóng Excel
                excelWorkbook.Close();
                excelApp.Quit();

                // Giải phóng COM
                Marshal.ReleaseComObject(excelWorksheet);
                Marshal.ReleaseComObject(excelWorkbook);
                Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadPhieuMuon();
            ResizeRowsToFill();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPhieuMuon();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPhieuMuon();
            }
        }
    }
}
