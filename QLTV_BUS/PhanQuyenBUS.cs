using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTV_DAO;

namespace QLTV_BUS
{
    public class PhanQuyenBUS
    {
        private PhanQuyenDAO phanQuyenDAO = new PhanQuyenDAO();

        // Kiểm tra quyền truy cập của vai trò đối với chức năng
        public bool KiemTraQuyen(string TenDangNhap, string maChucNang)
        {
            var phanQuyens = phanQuyenDAO.LayPhanQuyenTheoVaiTro(TenDangNhap);
            return phanQuyens.Any(p => p.MaChucNang == maChucNang && p.DuocTruyCap.GetValueOrDefault());
        }


        // Cập nhật quyền truy cập cho vai trò và chức năng
        public void CapNhatQuyen(string TenDangNhap, string maChucNang, bool duocTruyCap)
        {
            phanQuyenDAO.CapNhatPhanQuyen(TenDangNhap, maChucNang, duocTruyCap);
        }

        // Lấy danh sách các quyền của một vai trò
        public List<QLTV_Database.PhanQuyen> LayDanhSachQuyenTheoVaiTro(string TenDangNhap)
        {
            return phanQuyenDAO.LayPhanQuyenTheoVaiTro(TenDangNhap);
        }
    }
}
