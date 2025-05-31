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
using Microsoft.Win32;
using QLTV_Database;

namespace GUI
{
    public partial class MainForm : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        public MainForm()
        {
            InitializeComponent();
            // Đặt kích thước form  
            this.Size = new Size(1920, 1080);

            // Chống thu nhỏ, và định dạng nắp  
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
            var grouped = db.ChiTietPhieuMuonDatabase
    .GroupBy(ct => ct.MaSach)
    .Select(g => new {
        MaSach = g.Key,
        TotalBorrowed = g.Count() // <- mỗi lần mượn tính là 1 lượt
    })
    .ToList();

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

       



        private void LoadFormVaoPanel(Form form)
        {
            panelContent.Controls.Clear(); // Xóa nội dung cũ
            form.TopLevel = false; // Đặt Form con không phải cửa sổ chính
            form.FormBorderStyle = FormBorderStyle.None; // Ẩn viền
            form.Dock = DockStyle.Fill; // Fill toàn bộ panel
            panelContent.Controls.Add(form); // Thêm Form vào Panel
            form.Show(); // Hiển thị Form

        }
        public void LoadMainTrangChu()
        {
            MainTrangChu mainTrangChu = new MainTrangChu();

            // Gọi phương thức để load form vào panel  
            LoadFormVaoPanel(mainTrangChu);
        }

        
        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            

            Register registerForm = new Register(); // Tạo instance của form Register  
            registerForm.ShowDialog();

            this.Show();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            

            Login loginForm = new Login(); // Tạo instance của form Login  
            loginForm.ShowDialog();

            this.Show();
        }
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            LoadMainTrangChu();
        }
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
        public void LoadFormLienHe()
        {
            FormLienHe formLienHe = new FormLienHe();
            formLienHe.Size = new System.Drawing.Size(1298, 760);

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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
