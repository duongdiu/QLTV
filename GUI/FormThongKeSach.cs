using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using QLTV_Database;
using Excel = Microsoft.Office.Interop.Excel;

namespace GUI
{
    public partial class FormThongKeSach : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int pageSize = 5;          // Số dòng mỗi trang
        private int currentPage = 1;       // Trang hiện tại
        private int totalPages = 1;        // Tổng số trang
        private List<object> allData;
        public FormThongKeSach()
        {
            InitializeComponent();
            cboThongKe.Items.Add("Tất cả sách");
            cboThongKe.Items.Add("Sách đang mượn");
            cboThongKe.Items.Add("Sách đã trả");
            cboThongKe.Items.Add("Sách trễ hạn");

            // Đặt giá trị mặc định nếu cần  
            cboThongKe.SelectedIndex = 0;
            dgvThongKe.DataBindingComplete += (s, e) => ResizeRowsToFill();
            LoadDanhSachSach();
            

        }

        private void dgvSachDaMuon_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void LoadDanhSachSach(string tuKhoa = "", string trangThai = "Tất cả sách")
        {
            try
            {
                var query = from s in db.SachDatabase
                            join ct in db.ChiTietPhieuMuonDatabase on s.MaSach equals ct.MaSach into gj
                            from subCt in gj.DefaultIfEmpty()
                            join pm in db.PhieuMuonDatabase on subCt.MaPhieuMuon equals pm.MaPhieuMuon into gj2
                            from subPm in gj2.DefaultIfEmpty()
                            group new { s, subPm } by s into grouped // Nhóm theo sách  
                            select new
                            {
                                MaSach = grouped.Key.MaSach,
                                TenSach = grouped.Key.TenSach,
                                TacGia = grouped.Key.TacGia.TenTacGia,
                                TheLoai = grouped.Key.TheLoai.TenTheLoai,
                                NhaXB = grouped.Key.NhaXuatBan.TenNXB,
                                NamXuatBan = grouped.Key.NamXuatBan,
                                SoLuong = grouped.Key.SoLuong,
                                SoLanTaiBan = grouped.Key.SoLanTaiBan,
                                SoLuongMuon = grouped.Count(g => g.subPm != null), // Đếm số lượng mượn  
                                TinhTrang = grouped.Any(g => g.subPm != null) ? "Đang mượn" :
                                             grouped.Any(g => g.subPm != null && g.subPm.NgayTraDuKien < DateTime.Now) ? "Trễ hạn" :
                                             "Đã trả"
                            };

                // Điều kiện lọc theo trạng thái sách  
                if (trangThai == "Sách đang mượn")
                {
                    query = query.Where(s => s.TinhTrang == "Đang mượn");
                }
                else if (trangThai == "Sách trễ hạn")
                {
                    query = query.Where(s => s.TinhTrang == "Trễ hạn");
                }
                else if (trangThai == "Sách đã trả")
                {
                    query = query.Where(s => s.TinhTrang == "Đã trả");
                }

                // Điều kiện tìm kiếm  
                if (!string.IsNullOrWhiteSpace(tuKhoa))
                {
                    tuKhoa = tuKhoa.ToLower();
                    query = query.Where(s =>
                        s.TenSach.ToLower().Contains(tuKhoa) ||
                        s.TacGia.ToLower().Contains(tuKhoa) ||
                        s.TheLoai.ToLower().Contains(tuKhoa) ||
                        s.NhaXB.ToLower().Contains(tuKhoa));
                }

                // Lấy tổng số bản ghi
                var totalRecords = query.Count();

                // Tính số trang
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Sắp xếp dữ liệu trước khi phân trang
                query = query.OrderBy(s => s.MaSach); // Bạn có thể thay đổi trường này nếu cần

                // Lấy dữ liệu cho trang hiện tại
                var pagedData = query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();


                // Hiển thị dữ liệu lên DataGridView
                dgvThongKe.DataSource = pagedData;
                ResizeRowsToFill();

                // Cập nhật header cho các cột
                dgvThongKe.Columns["MaSach"].HeaderText = "Mã Sách";
                dgvThongKe.Columns["TenSach"].HeaderText = "Tên Sách";
                dgvThongKe.Columns["TacGia"].HeaderText = "Tác Giả";
                dgvThongKe.Columns["TheLoai"].HeaderText = "Thể Loại";
                dgvThongKe.Columns["NhaXB"].HeaderText = "Nhà XB";
                dgvThongKe.Columns["NamXuatBan"].HeaderText = "Năm Xuất Bản";
                dgvThongKe.Columns["SoLanTaiBan"].HeaderText = "Số lần tái bản";
                dgvThongKe.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvThongKe.Columns["SoLuongMuon"].HeaderText = "Số Lượng Mượn";
                dgvThongKe.Columns["TinhTrang"].Visible = false;
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


        private void TimKiemSach()
        {
            string tuKhoa = txtTimKiem.Text.ToLower();
            dgvThongKe.DataSource = ((List<dynamic>)dgvThongKe.DataSource)
                .Where(s => s.TenSach.ToLower().Contains(tuKhoa) ||
                            s.MaDocGia.ToString().Contains(tuKhoa) ||
                            s.MaPhieuMuon.ToString().Contains(tuKhoa))
                .ToList();
        }

        

        

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        
        
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnThongKe_Click_1(object sender, EventArgs e)
        {
            // Lấy giá trị từ ComboBox  
            string trangThai = cboThongKe.SelectedItem.ToString();

            // Gọi hàm để tải danh sách sách theo trạng thái đã chọn  
            LoadDanhSachSach("", trangThai);
            ResizeRowsToFill();
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            TimKiemSach();
            ResizeRowsToFill();
        }

        private void btnXBC_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Tạo một ứng dụng Excel mới  
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

                // ======= BỔ SUNG TIÊU ĐỀ (TITLE) =======
                excelWorksheet.Cells[1, 1] = "BÁO CÁO THỐNG KÊ SÁCH";
                Excel.Range titleRange = excelWorksheet.Range["A1", "H1"];
                titleRange.Merge(); // Gộp các ô
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ======= Tiêu đề cột bắt đầu từ dòng 3 =======
                excelWorksheet.Cells[3, 1].Value = "Mã Sách";
                excelWorksheet.Cells[3, 2].Value = "Tên Sách";
                excelWorksheet.Cells[3, 3].Value = "Tác Giả";
                excelWorksheet.Cells[3, 4].Value = "Thể Loại";
                excelWorksheet.Cells[3, 5].Value = "Nhà XB";
                excelWorksheet.Cells[3, 6].Value = "Năm Xuất Bản";
                excelWorksheet.Cells[3, 7].Value = "Số lần tái bản";
                excelWorksheet.Cells[3, 8].Value = "Số Lượng";
                excelWorksheet.Cells[3, 9].Value = "Số Lượng Mượn";

                // Lấy dữ liệu từ DataGridView bắt đầu từ dòng 4  
                int rowStart = 4;
                foreach (DataGridViewRow row in dgvThongKe.Rows)
                {
                    excelWorksheet.Cells[rowStart, 1].Value = row.Cells["MaSach"].Value;
                    excelWorksheet.Cells[rowStart, 2].Value = row.Cells["TenSach"].Value;
                    excelWorksheet.Cells[rowStart, 3].Value = row.Cells["TacGia"].Value;
                    excelWorksheet.Cells[rowStart, 4].Value = row.Cells["TheLoai"].Value;
                    excelWorksheet.Cells[rowStart, 5].Value = row.Cells["NhaXB"].Value;
                    excelWorksheet.Cells[rowStart, 6].Value = row.Cells["NamXuatBan"].Value;
                    excelWorksheet.Cells[rowStart, 7].Value = row.Cells["SoLanTaiBan"].Value;
                    excelWorksheet.Cells[rowStart, 8].Value = row.Cells["SoLuong"].Value;
                    excelWorksheet.Cells[rowStart, 9].Value = row.Cells["SoLuongMuon"].Value;
                    rowStart++;
                }

                // Định dạng tiêu đề cột  
                Excel.Range headerRange = excelWorksheet.Range[excelWorksheet.Cells[3, 1], excelWorksheet.Cells[3, 9]];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                // Tự động điều chỉnh cột  
                excelWorksheet.Columns.AutoFit();

                // ======= BỔ SUNG CHÂN TRANG (FOOTER) =======
                excelWorksheet.PageSetup.CenterFooter = "Trang &P / &N";
                excelWorksheet.PageSetup.LeftFooter = "Xuất bởi: Quản trị viên";
                excelWorksheet.PageSetup.RightFooter = DateTime.Now.ToString("dd/MM/yyyy");

                // Hiển thị hộp thoại lưu file  
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = "ThongKeSach.xlsx";
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Lưu file  
                        excelWorkbook.SaveAs(saveFileDialog.FileName);
                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Đóng workbook và ứng dụng Excel  
                excelWorkbook.Close(false);
                excelApp.Quit();

                // Giải phóng bộ nhớ  
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            LoadDanhSachSach("", cboThongKe.SelectedItem?.ToString() ?? "Tất cả sách");
            ResizeRowsToFill();
        }

        private void FormThongKeSach_Load(object sender, EventArgs e)
        {

        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachSach();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadDanhSachSach();
            }
        }
    }
}
