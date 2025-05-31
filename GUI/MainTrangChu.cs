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
    public partial class MainTrangChu : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public MainTrangChu()
        {
            InitializeComponent();
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

        private void MainTrangChu_Load(object sender, EventArgs e)
        {

        }
    }
}
