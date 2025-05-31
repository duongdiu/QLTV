using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLTV_Database;

namespace GUI
{
    public partial class FormDocGiaChinh : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public FormDocGiaChinh()
        {
            InitializeComponent();
            LoadDuLieu();
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
                    TotalBorrowed = g.Sum(ct => ct.SoLuongMuon ?? 0)
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
        private void flowLayoutPanelBinhLuan_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormDocGiaChinh_Load(object sender, EventArgs e)
        {

        }
    }
}
