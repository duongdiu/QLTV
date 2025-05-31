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

namespace GUI
{
    public partial class ThuThuForm : Form
    {
        private string tenDangNhap;
        private LibraryDBContext db = new LibraryDBContext();
        private int maxHeight = 150; // Chiều cao tối đa panel
        private int animationSpeed = 15;

        private Timer timerShow;
        private Timer timerHide;
        private FlowLayoutPanel lstThongBao;
        public ThuThuForm(string tenDangNhap)
        {
            InitializeComponent();
            // Đặt kích thước form  
            this.Size = new Size(1920, 1080);

            // Chống thu nhỏ, và định dạng nắp  
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Lưu tên đăng nhập
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
            this.MouseClick += AdminForm_MouseClick;

            LoadAdminNotifications();
        }
        private void AdminForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!pnThongBao.Bounds.Contains(e.Location) && !picThongBao.Bounds.Contains(e.Location))
            {
                if (pnThongBao.Visible)
                {
                    timerHide.Start();
                }
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
        private void LoadAdminNotifications()
        {
            using (var db = new LibraryDBContext())
            {
                var today = DateTime.Today;
                var threeDaysLater = today.AddDays(3);

                int phieuMuonSapTra = db.PhieuMuonDatabase
                    .Count(pm => pm.TinhTrang == "Đang mượn" &&
                                 pm.NgayTraDuKien >= today &&
                                 pm.NgayTraDuKien <= threeDaysLater);

                int sachSapHet = db.SachDatabase.Count(s => s.SoLuong < 10);

                lblThongBao.Text = (phieuMuonSapTra + sachSapHet).ToString();
            }
        }
        private void ShowAdminNotifications()
        {
            if (lstThongBao == null)
            {
                lstThongBao = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false
                };
                pnThongBao.Controls.Add(lstThongBao);
            }

            lstThongBao.Controls.Clear();

            using (var db = new LibraryDBContext())
            {
                var today = DateTime.Today;
                var threeDaysLater = today.AddDays(3);

                var danhSachPhieuMuon = db.PhieuMuonDatabase
                    .Where(pm => pm.TinhTrang == "Đang mượn" &&
                                 pm.NgayTraDuKien >= today &&
                                 pm.NgayTraDuKien <= threeDaysLater)
                    .ToList();

                foreach (var pm in danhSachPhieuMuon)
                {
                    AddNotificationItem($"Độc giả {pm.MaDocGia} có phiếu mượn {pm.MaPhieuMuon} sắp đến hạn trả vào {pm.NgayTraDuKien:dd/MM/yyyy}");
                }

                var danhSachSachSapHet = db.SachDatabase
                    .Where(s => s.SoLuong < 10)
                    .ToList();

                foreach (var sach in danhSachSachSapHet)
                {
                    AddNotificationItem($"Sách \"{sach.TenSach}\" còn lại {sach.SoLuong} quyển, cần bổ sung.");
                }
            }
        }

        private void AddNotificationItem(string message)
        {
            Panel item = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                Margin = new Padding(5),
                Width = pnThongBao.Width - 30,
                AutoSize = true,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = $"• {message}",
                AutoSize = true,
                MaximumSize = new Size(item.Width - 10, 0),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black
            };

            item.Controls.Add(lbl);
            lstThongBao.Controls.Add(item);
        }

        private void OpenChildForm(Form childForm)
        {
            // Xóa form cũ trong panel
            panelContainer.Controls.Clear();

            // Thiết lập form con
            childForm.TopLevel = false; // Không phải cửa sổ độc lập
            childForm.FormBorderStyle = FormBorderStyle.None; // Ẩn viền cửa sổ con
            childForm.Dock = DockStyle.Fill; // Mở rộng full panel

            // Thêm form con vào panel
            panelContainer.Controls.Add(childForm);
            childForm.BringToFront();
            childForm.Show();

        }
        private bool CheckPermission(string tenDangNhap, string maChucNang)
        {
            var userPermissions = db.PhanQuyenDatabase
                .Where(pq => pq.TenDangNhap == tenDangNhap && pq.MaChucNang == maChucNang)
                .FirstOrDefault();

            // Kiểm tra nếu userPermissions != null và DuocTruyCap == true
            return userPermissions?.DuocTruyCap == true;
        }
        private void quảnLýTácGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN15";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyTacGia());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void sáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN14";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLySachForm());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void quảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN16";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyTheLoai());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void quảnLýTácGiảToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN15";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyTacGia());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void quảnLýNhàXuấtBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN13";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyNXB());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void timsach_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN10";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new FormTimKiemSach());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void timdocgia_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN09";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new FormTimKiemDocGia());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void phiếuMượnSách_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN08";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new FormTimKiemPhieuMuon());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void BinhLuanSach_Click(object sender, EventArgs e)
        {
            
        }


        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại

            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void QuanLyBinhLuan(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN03";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new FormQuanLyBinhLuanSach());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void LienHe(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN04";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new FormQuanLyLienHe());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void picThongBao_Click(object sender, EventArgs e)
        {
            if (!pnThongBao.Visible || pnThongBao.Height == 0)
            {
                pnThongBao.Visible = true;
                pnThongBao.BringToFront();
                pnThongBao.Height = 0;
                ShowAdminNotifications();
                timerShow.Start();
            }
            else
            {
                timerHide.Start();
            }
        }

        private void quảnLýĐộcGiảToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN11";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyDocGia());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void xácNhậnMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN12";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyLichHen());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void phiếuMượnSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Lấy tên đăng nhập đã được lưu trong form cha
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN02";

            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                // Khởi tạo form con và truyền tên đăng nhập
                var frm = new FormPhieuMuonSach(tenDangNhap);
                OpenChildForm(frm);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void chiTiếtMượntrảSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN01";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                var frm = new ChiTietMuonTraSach(this.tenDangNhap);
                OpenChildForm(frm);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void ThuThuForm_Load(object sender, EventArgs e)
        {

        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void quảnLýVịTríSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tenDangNhap = this.tenDangNhap;
            string maChucNang = "CN17";
            // Kiểm tra quyền truy cập
            if (CheckPermission(tenDangNhap, maChucNang))
            {
                OpenChildForm(new QuanLyViTri());
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
