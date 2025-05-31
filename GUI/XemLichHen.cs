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
    public partial class XemLichHen : Form
    {
        private string maDocGia;
        private LibraryDBContext db = new LibraryDBContext();
        public XemLichHen(string maDocGia)
        {
            InitializeComponent();
            this.maDocGia = maDocGia;
            dgvLichHen.CellClick += dgvLichHen_CellClick;
            LoadLichHen();
        }
        private void LoadLichHen()
        {
            try
            {
                var query = from lich in db.DatLichMuonDatabase
                            where lich.MaDocGia == maDocGia
                            orderby lich.NgayDenMuon descending
                            select new
                            {
                                lich.MaLichMuon,
                                lich.NgayDenMuon,
                                lich.GioDenMuon,
                                lich.TrangThai,
                                lich.GhiChu,
                                Sach = (from ct in db.ChiTietDatLichMuonDatabase
                                        join s in db.SachDatabase on ct.MaSach equals s.MaSach
                                        where ct.MaLichMuon == lich.MaLichMuon
                                        select s.TenSach).FirstOrDefault(),
                                SoLuong = (from ct in db.ChiTietDatLichMuonDatabase
                                           where ct.MaLichMuon == lich.MaLichMuon
                                           select ct.SoLuong).FirstOrDefault()
                            };

                var lichHenList = query.ToList().Select(x => new
                {
                    x.MaLichMuon,
                    x.NgayDenMuon,
                    GioDen = x.GioDenMuon.ToString(@"hh\:mm"), // Định dạng giờ
                    x.TrangThai,
                    x.GhiChu,
                    x.Sach,
                    x.SoLuong
                }).ToList();

                dgvLichHen.DataSource = lichHenList;

                // Đặt lại tên cột
                dgvLichHen.Columns["MaLichMuon"].HeaderText = "Mã Lịch Hẹn";
                dgvLichHen.Columns["NgayDenMuon"].HeaderText = "Ngày Đến";
                dgvLichHen.Columns["GioDen"].HeaderText = "Giờ Đến";
                dgvLichHen.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dgvLichHen.Columns["GhiChu"].HeaderText = "Ghi Chú";
                dgvLichHen.Columns["Sach"].HeaderText = "Tên Sách";
                dgvLichHen.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvLichHen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvLichHen.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                btnHuyLich.Enabled = false; // disable khi chưa chọn
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu lịch hẹn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void dgvLichHen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvLichHen.CurrentRow != null)
            {
                btnHuyLich.Enabled = true;
            }
            else
            {
                btnHuyLich.Enabled = false;
            }

        }

        private void btnHuyLich_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLichHen.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn lịch hẹn muốn huỷ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maLichMuon = dgvLichHen.CurrentRow.Cells["MaLichMuon"].Value.ToString();
                var lich = db.DatLichMuonDatabase.FirstOrDefault(l => l.MaLichMuon == maLichMuon);

                if (lich == null)
                {
                    MessageBox.Show("Không tìm thấy lịch hẹn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (lich.TrangThai != "Chờ xử lý")
                {
                    MessageBox.Show("Chỉ có thể huỷ lịch ở trạng thái 'Chờ xử lý'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var confirm = MessageBox.Show("Bạn có chắc chắn muốn huỷ lịch hẹn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        var chiTietList = db.ChiTietDatLichMuonDatabase.Where(ct => ct.MaLichMuon == maLichMuon).ToList();
                        db.ChiTietDatLichMuonDatabase.RemoveRange(chiTietList);

                        db.DatLichMuonDatabase.Remove(lich);
                        db.SaveChanges();

                        MessageBox.Show("Huỷ lịch hẹn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadLichHen();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi huỷ lịch: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy lịch: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XemLichHen_Load(object sender, EventArgs e)
        {

        }
    }
}
