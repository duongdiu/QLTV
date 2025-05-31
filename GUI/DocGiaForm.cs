using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;
using QLTV_DTO;

namespace GUI
{
    public partial class DocGiaForm : Form
    {
        private string tenDangNhap;
        private LibraryDBContext db = new LibraryDBContext();
        private int maxHeight = 150; // Chiều cao tối đa panel
        private int animationSpeed = 15;

        private Timer timerShow;
        private Timer timerHide;
        private FlowLayoutPanel lstThongBao;

        public DocGiaForm(string tenDangNhap)
        {
            InitializeComponent();

            this.Size = new Size(1920, 1080);
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.tenDangNhap = tenDangNhap;

            lblThongBao.Text = "0";
            // Cài đặt khung tổng quát cho pnThongBao
            pnThongBao.BorderStyle = BorderStyle.FixedSingle;
            pnThongBao.BackColor = Color.White;
            pnThongBao.Padding = new Padding(10);
            pnThongBao.AutoScroll = true;
            pnThongBao.Visible = false;
            pnThongBao.MaximumSize = new Size(350, 300);
            pnThongBao.BringToFront();

            // Khởi tạo Timer
            timerShow = new Timer();
            timerShow.Interval = 10;
            timerShow.Tick += timerShow_Tick;

            timerHide = new Timer();
            timerHide.Interval = 10;
            timerHide.Tick += timerHide_Tick;

            picThongBao.Click += picThongBao_Click;
            this.MouseClick += DocGiaForm_MouseClick;

            LoadNotifications();
            LoadTopBorrowedBooks();
        }
        private void LoadCoverImage(string path, PictureBox pic)
        {
            pic.Image = null;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    pic.Image = Image.FromStream(stream);
            }
            catch
            {
                pic.Image = null;
            }
        }
        private void LoadTopBorrowedBooks()
        {
            // 1. Lấy dữ liệu nhóm từ database (chỉ những gì EF hiểu được)
            var grouped = db.ChiTietPhieuMuonDatabase
    .GroupBy(ct => ct.MaSach)
    .Select(g => new {
        MaSach = g.Key,
        TotalBorrowed = g.Count() // <- mỗi lần mượn tính là 1 lượt
    })
    .ToList();  // <- chuyển hết về bộ nhớ

            // 2. Dùng LINQ to Objects để sắp xếp & lấy top 2
            var top2 = grouped
                .OrderByDescending(x => x.TotalBorrowed)
                .Take(2)
                .ToList();

            // 3. Lấy dữ liệu sách từ database
            var topBooks = top2.Select(t => db.SachDatabase.FirstOrDefault(s => s.MaSach == t.MaSach)).ToList();

            // 4. Đổ lên giao diện
            if (topBooks.Count > 0)
            {
                var s1 = topBooks[0];
                if (s1 != null)
                {
                    PopulateBookPanel(
                        s1, top2[0].TotalBorrowed,
                        pbCover1, lblAuthor1, lblPublisher1,
                        lblYear1, lblCategory1, lblBorrowCount1
                    );
                    LoadCoverImage(s1?.AnhBia, pbCover1);
                }
            }

            if (topBooks.Count > 1)
            {
                var s2 = topBooks[1];
                if (s2 != null)
                {
                    PopulateBookPanel(
                        s2, top2[1].TotalBorrowed,
                        pbCover2, lblAuthor2, lblPublisher2,
                        lblYear2, lblCategory2, lblBorrowCount2
                    );
                    LoadCoverImage(s2?.AnhBia, pbCover2);
                }
            }
        }



        private void PopulateBookPanel(
    QLTV_Database.Sach sach, int borrowCount,
    PictureBox pbCover,
    Label lblAuthor, Label lblPublisher, Label lblYear, Label lblCategory, Label lblBorrowCount)
        {
            if (sach == null) return;

            lblAuthor.Text = $"Tác giả:     {db.TacGiaDatabase.Find(sach.MaTacGia)?.TenTacGia}";
            lblPublisher.Text = $"NXB:         {db.NhaXuatBanDatabase.Find(sach.MaNXB)?.TenNXB}";
            lblYear.Text = $"Năm xuất bản:      {sach.NamXuatBan}";
            lblCategory.Text = $"Thể loại:    {db.TheLoaiDatabase.Find(sach.MaTheLoai)?.TenTheLoai}";
            lblBorrowCount.Text = $"Số lượt mượn:     {borrowCount} lần";

            pbCover.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void picThongBao_Click(object sender, EventArgs e)
        {
            if (!pnThongBao.Visible || pnThongBao.Height == 0)
            {
                pnThongBao.Visible = true;
                pnThongBao.BringToFront();
                pnThongBao.Height = 0;
                LoadNotifications();
                ShowNotifications();
                timerShow.Start();
            }
            else
            {
                timerHide.Start();
            }
        }


        private void timerShow_Tick(object sender, EventArgs e)
        {
            if (pnThongBao.Height < maxHeight)
            {
                pnThongBao.Height += animationSpeed;
            }
            else
            {
                timerShow.Stop();
                pnThongBao.Height = maxHeight;
            }
        }

        private void timerHide_Tick(object sender, EventArgs e)
        {
            if (pnThongBao.Height > 0)
            {
                pnThongBao.Height -= animationSpeed;
            }
            else
            {
                timerHide.Stop();
                pnThongBao.Visible = false;
            }
        }

        private void DocGiaForm_MouseClick(object sender, MouseEventArgs e)
        {
            // Ẩn panel nếu click ra ngoài panel hoặc chuông
            if (!pnThongBao.Bounds.Contains(e.Location) && !picThongBao.Bounds.Contains(e.Location))
            {
                if (pnThongBao.Visible)
                {
                    timerHide.Start();
                }
            }
        }
        private string maDocGia;
        private void LoadNotifications()
        {
            var nguoiDung = db.NguoiDungDatabase.FirstOrDefault(nd => nd.TenDangNhap == tenDangNhap);
            if (nguoiDung != null)
            {
                maDocGia = nguoiDung.MaDocGia;
            }

            int notificationCount = GetUpcomingReturnsCount();
            lblThongBao.Text = notificationCount.ToString();
        }

        private void ShowNotifications()
        {
            if (lstThongBao == null)
            {
                lstThongBao = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Padding = new Padding(5)
                };
                pnThongBao.Controls.Add(lstThongBao);
            }

            lstThongBao.Controls.Clear();

            var today = DateTime.Now;
            var threeDaysLater = today.AddDays(3);

            var danhSachPhieuMuon = db.PhieuMuonDatabase
    .Where(pm => pm.TinhTrang == "Đang mượn" &&
                 pm.NgayTraDuKien >= today &&
                 pm.NgayTraDuKien <= threeDaysLater &&
                 pm.MaDocGia == maDocGia)
    .ToList();

            foreach (var phieuMuon in danhSachPhieuMuon)
            {
                // Tạo panel chứa thông báo (có viền)
                Panel itemPanel = new Panel
                {
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(8),
                    Margin = new Padding(5),
                    Width = pnThongBao.Width - 20,
                    AutoSize = true
                };

                Label lbl = new Label
                {
                    AutoSize = true,
                    MaximumSize = new Size(itemPanel.Width - 10, 0),
                    Text = $"• Phiếu mượn {phieuMuon.MaPhieuMuon} sắp đến hạn trả vào ngày {phieuMuon.NgayTraDuKien:dd/MM/yyyy}",
                    Font = new Font("Segoe UI", 10),
                    ForeColor = Color.Black,
                };

                itemPanel.Controls.Add(lbl);
                lstThongBao.Controls.Add(itemPanel);
            }
        }

        private int GetUpcomingReturnsCount()
        {
            var today = DateTime.Today;
            var threeDaysLater = today.AddDays(3);

            return db.PhieuMuonDatabase.Count(pm =>
                pm.TinhTrang == "Đang mượn" &&
                pm.NgayTraDuKien >= today &&
                pm.NgayTraDuKien <= threeDaysLater &&
                pm.MaDocGia == maDocGia
            );
        }


        private void mượnTrảToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void LoadDuLieu()
        {
            // Thiết lập các thuộc tính của FlowLayoutPanel
            flowLayoutPanelBinhLuan.WrapContents = false;  // Không cho các panel xuống dòng
            flowLayoutPanelBinhLuan.AutoScroll = true;  // Bật chế độ cuộn tự động nếu nội dung quá lớn
            flowLayoutPanelBinhLuan.FlowDirection = FlowDirection.TopDown;  // Các panel xếp theo chiều dọc

            // Lấy dữ liệu bình luận từ cơ sở dữ liệu
            var danhSach = db.PhanHoiDatabase.AsQueryable();
            danhSach = danhSach.Where(p => p.TrangThai == "Đã duyệt");

            var query = danhSach
                .Join(db.SachDatabase, ph => ph.MaSach, sach => sach.MaSach, (ph, sach) => new
                {
                    ph.MaPhanHoi,
                    ph.MaDocGia,
                    TenSach = sach.TenSach,
                    ph.NoiDung,
                    ph.NgayGui,
                    ph.TrangThai,
                    ph.PhanHoiAdmin
                })
                .ToList()
                .Select(p => new
                {
                    p.MaPhanHoi,
                    p.MaDocGia,
                    p.TenSach,
                    p.NoiDung,
                    NgayGui = p.NgayGui.HasValue ? p.NgayGui.Value.ToString("dd/MM/yyyy") : "",
                    p.TrangThai,
                    p.PhanHoiAdmin
                })
                .ToList();

            // Xóa các điều khiển cũ trong FlowLayoutPanel
            flowLayoutPanelBinhLuan.Controls.Clear();

            // Duyệt qua tất cả bình luận đã duyệt và thêm vào FlowLayoutPanel
            foreach (var phanHoi in query)
            {
                // Tạo một panel chứa thông tin bình luận
                Panel panelBinhLuan = new Panel();
                panelBinhLuan.BorderStyle = BorderStyle.FixedSingle;
                panelBinhLuan.Margin = new Padding(10);
                panelBinhLuan.Width = flowLayoutPanelBinhLuan.Width - 20;  // Đặt chiều rộng của panel bằng chiều rộng FlowLayoutPanel trừ đi margin
                // Tạo Label cho phản hồi admin
                Label lblPhanHoiAdmin = new Label();
                lblPhanHoiAdmin.Text = "Phản Hồi Admin: " + phanHoi.PhanHoiAdmin;
                lblPhanHoiAdmin.Font = new Font("Times New Roman", 14, FontStyle.Regular);
                lblPhanHoiAdmin.Dock = DockStyle.Top;
                panelBinhLuan.Controls.Add(lblPhanHoiAdmin);
                // Tạo Label cho nội dung bình luận
                Label lblNoiDung = new Label();
                lblNoiDung.Text = "Nội Dung: " + phanHoi.NoiDung;
                lblNoiDung.Font = new Font("Times New Roman", 14, FontStyle.Regular);
                lblNoiDung.Dock = DockStyle.Top;
                panelBinhLuan.Controls.Add(lblNoiDung);

                // Tạo Label cho tên sách
                Label lblTenSach = new Label();
                lblTenSach.Text = "Tên Sách: " + phanHoi.TenSach;
                lblTenSach.Font = new Font("Times New Roman", 14, FontStyle.Regular);
                lblTenSach.Dock = DockStyle.Top;
                panelBinhLuan.Controls.Add(lblTenSach);
                // Tạo Label cho tên độc giả
                Label lblDocGia = new Label();
                lblDocGia.Text = "Mã Độc Giả: " + phanHoi.MaDocGia;
                lblDocGia.Font = new Font("Times New Roman", 14, FontStyle.Regular);
                lblDocGia.Dock = DockStyle.Top;
                panelBinhLuan.Controls.Add(lblDocGia);

              

               

                // Thêm panel bình luận vào FlowLayoutPanel
                flowLayoutPanelBinhLuan.Controls.Add(panelBinhLuan);
            }
        
        }

        private void LoadFormVaoPanel(Form form)
        {
            panelContent.Controls.Clear(); // Xóa nội dung cũ
            form.TopLevel = false; // Đặt Form con không phải cửa sổ chính
            form.FormBorderStyle = FormBorderStyle.None; // Ẩn viền
            form.Dock = DockStyle.Fill; // Fill toàn bộ panel
            panelContent.Controls.Add(form); // Thêm Form vào Panel
            form.Show(); // Hiển thị Form

        }



        private void btnSachDaMuon_Click(object sender, EventArgs e)
        {
            try
            {
                string maDocGia = db.NguoiDungDatabase
                                    .Where(nd => nd.TenDangNhap == tenDangNhap)
                                    .Select(nd => nd.MaDocGia)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maDocGia))
                {
                    FormSachDaMuon form = new FormSachDaMuon(maDocGia);
                    LoadFormVaoPanel(form);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sách đã mượn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMuonSach_Click(object sender, EventArgs e)
        {
            try
            {
                string maDocGia = db.NguoiDungDatabase
                                    .Where(nd => nd.TenDangNhap == tenDangNhap)
                                    .Select(nd => nd.MaDocGia)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maDocGia))
                {
                    FormMuonSach form = new FormMuonSach(maDocGia);
                    LoadFormVaoPanel(form);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form mượn sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnBinhLuan_Click(object sender, EventArgs e)
        {
            try
            {
                string maDocGia = db.NguoiDungDatabase
                                    .Where(nd => nd.TenDangNhap == tenDangNhap)
                                    .Select(nd => nd.MaDocGia)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maDocGia))
                {
                    FormBinhLuan form = new FormBinhLuan(maDocGia);
                    LoadFormVaoPanel(form);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form bình luận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                string maDocGia = db.NguoiDungDatabase
                                    .Where(nd => nd.TenDangNhap == tenDangNhap)
                                    .Select(nd => nd.MaDocGia)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maDocGia))
                {
                    FormCapNhatThongTin form = new FormCapNhatThongTin(maDocGia);
                    LoadFormVaoPanel(form);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form cập nhật thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnThongBao_Click_1(object sender, EventArgs e)
        {

        }

        
        
        
        

        private void lblSoThongBao_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panelContent_Paint(object sender, PaintEventArgs e)
        {
            LoadDuLieu();
        }
        
        private void DocGiaForm_Load(object sender, EventArgs e)
        {

        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại

            // Giả sử bạn muốn trở về MainForm  
            MainForm mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            mainForm?.Show(); // Hiện MainForm  
        }

        private void flowLayoutPanelBinhLuan_Paint(object sender, PaintEventArgs e)
        {
        }
        public void LoadFormDocGiaChinh()
        {
            FormDocGiaChinh docGiaChinhForm = new FormDocGiaChinh();

            // Gọi phương thức để load form vào panel  
            LoadFormVaoPanel(docGiaChinhForm);
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            LoadFormDocGiaChinh();

        }

        private void lblBinhLuan_Click(object sender, EventArgs e)
        {

        }

        private void lblSachDuocMuon_Click(object sender, EventArgs e)
        {

        }
        public void LoadFormLienHe()
        {
            FormLienHe formLienHe = new FormLienHe();

            // Gọi phương thức để load form vào panel  
            LoadFormVaoPanel(formLienHe);
        }
        private void btnLienHe_Click(object sender, EventArgs e)
        {
            LoadFormLienHe();
        }
        public void LoadFormQuyDinh()
        {
            FormQuyDinh formQuyDinh = new FormQuyDinh();

            // Gọi phương thức để load form vào panel  
            LoadFormVaoPanel(formQuyDinh);
        }
        private void btnQuyDinh_Click(object sender, EventArgs e)
        {
            LoadFormQuyDinh();
        }
        public void LoadFormTinTuc()
        {
            FormTinTuc formTinTuc = new FormTinTuc();

            // Gọi phương thức để load form vào panel  
            LoadFormVaoPanel(formTinTuc);
        }
        private void btnTinTuc_Click(object sender, EventArgs e)
        {
            LoadFormTinTuc();
        }

        
        private void btnLichHen_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy mã độc giả từ tên đăng nhập
                string maDocGia = db.NguoiDungDatabase
                                    .Where(nd => nd.TenDangNhap == tenDangNhap)
                                    .Select(nd => nd.MaDocGia)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(maDocGia))
                {
                    // Truyền mã độc giả vào form XemLichHen
                    XemLichHen form = new XemLichHen(maDocGia);
                    LoadFormVaoPanel(form); // hoặc form.ShowDialog() nếu không dùng MDI/panel
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin độc giả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form xem lịch hẹn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pnlThongBao_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblSoThongBao_Click_1(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panelBook1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

