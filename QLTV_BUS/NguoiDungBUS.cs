using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace QLTV_BUS
{
    public class NguoiDungBUS
    {
        private NguoiDungDAO nguoiDungDAO = new NguoiDungDAO();

        // Lấy danh sách tất cả người dùng
        public List<QLTV_Database.NguoiDung> GetAllNguoiDung()
        {
            return nguoiDungDAO.GetAll();
        }

        // Tìm người dùng theo tên đăng nhập
        public QLTV_Database.NguoiDung GetNguoiDungByUsername(string tenDangNhap)
        {
            return nguoiDungDAO.GetById(tenDangNhap);
        }

        // Thêm người dùng mới
        public void AddNguoiDung(QLTV_Database.NguoiDung nguoiDung)
        {
            nguoiDungDAO.Add(nguoiDung);
        }

        // Cập nhật thông tin người dùng
        public void UpdateNguoiDung(QLTV_Database.NguoiDung nguoiDung)
        {
            nguoiDungDAO.Update(nguoiDung);
        }

        // Xóa người dùng
        public void DeleteNguoiDung(string tenDangNhap)
        {
            nguoiDungDAO.Delete(tenDangNhap);
        }

        // Kiểm tra đăng nhập
        public bool Login(string tenDangNhap, string matKhau)
        {
            QLTV_Database.NguoiDung nguoiDung = nguoiDungDAO.GetById(tenDangNhap);
            if (nguoiDung != null && nguoiDung.MatKhau == matKhau) // TODO: Hash password
            {
                return true;
            }
            return false;
        }
    }
}
