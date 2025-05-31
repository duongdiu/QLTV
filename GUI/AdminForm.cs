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
    public partial class AdminForm : Form
    {
        private LibraryDBContext db = new LibraryDBContext();
        private int maxHeight = 150; // Chiều cao tối đa panel
        private int animationSpeed = 15;

        private Timer timerShow;
        private Timer timerHide;
        private FlowLayoutPanel lstThongBao;
        private string tenDangNhap;
        public AdminForm(string tenDangNhap)
        {
            InitializeComponent();
            // Đặt kích thước form  
            this.Size = new Size(1920, 1080);

            // Chống thu nhỏ, và định dạng nắp  
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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


        private void AdminForm_Load(object sender, EventArgs e)
        {

        }

        private void quảnLýĐộcGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void độcGiảToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyDocGia());
        }

        private void quảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyTheLoai());
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

        private void sáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLySachForm());
        }

        private void quảnLýTácGiảToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyTacGia());
        }

        private void quảnLýNhàXuấtBảnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyNXB());
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sáchToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormThongKeSach());
        }

        private void độcGiảToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormThongKeDocGia());
        }

        private void sáchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormTimKiemSach());
        }

        private void độcGiảToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormTimKiemDocGia());
        }

        private void phiếuMượnSáchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormTimKiemPhieuMuon());
        }

        

        

        private void quanLyNguoiDung_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQuanLyNguoiDung());
        }

        

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại

            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

       

        private void LichHen_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyLichHen());
        }

        private void PhieuMuonTraSach_Click(object sender, EventArgs e)
        {
            // Tạo instance của FormPhieuMuonSach và truyền tenDangNhap
            var frm = new FormPhieuMuonSach(this.tenDangNhap);
            OpenChildForm(frm);
        }

        private void ChiTietMuonTraSach_Click(object sender, EventArgs e)
        {
            var frm = new ChiTietMuonTraSach(this.tenDangNhap);
            OpenChildForm(frm);
        }

        private void pnThongBao_Paint(object sender, PaintEventArgs e)
        {

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

        private void lịchHẹnMượnSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyLichHen());
        }

        

       

        private void QuanLyPhanHoi_Click(object sender, EventArgs e)
        {

        }

        private void QuanLyBinhLuan_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQuanLyBinhLuanSach());
        }

        private void LienHe_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQuanLyLienHe());
        }

        private void ChucNang_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyChucNang());
        }

        private void PhanQuyen_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PhanQuyenHeThong());
        }

        private void quảnLýVịTríSáchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new QuanLyViTri());
        }

        private void phiếuMượnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormThongKePhieuMuon());
        }
    }
}
