using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace BUS
{
    public class TheLoaiBUS
    {
        private TheLoaiDAO theLoaiDAO = new TheLoaiDAO();

        // Lấy tất cả thể loại
        public List<QLTV_Database.TheLoai> GetAllTheLoai()
        {
            return theLoaiDAO.GetAll();
        }

        // Lấy thể loại theo mã
        public QLTV_Database.TheLoai GetTheLoaiById(string maTheLoai)
        {
            return theLoaiDAO.GetById(maTheLoai);
        }

        // Thêm thể loại
        public void AddTheLoai(QLTV_Database.TheLoai theLoai)
        {
            theLoaiDAO.Add(theLoai);
        }

        // Cập nhật thể loại
        public void UpdateTheLoai(QLTV_Database.TheLoai theLoai)
        {
            theLoaiDAO.Update(theLoai);
        }

        // Xóa thể loại
        public void DeleteTheLoai(string maTheLoai)
        {
            theLoaiDAO.Delete(maTheLoai);
        }
    }
}
