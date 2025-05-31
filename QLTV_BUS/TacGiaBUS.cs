using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace BUS
{
    public class TacGiaBUS
    {
        private TacGiaDAO tacGiaDAO = new TacGiaDAO();

        // Lấy tất cả tác giả
        public List<QLTV_Database.TacGia> GetAllTacGia()
        {
            return tacGiaDAO.GetAll();
        }

        // Lấy tác giả theo mã
        public QLTV_Database.TacGia GetTacGiaById(string maTacGia)
        {
            return tacGiaDAO.GetById(maTacGia);
        }

        // Thêm tác giả
        public void AddTacGia(QLTV_Database.TacGia tacGia)
        {
            tacGiaDAO.Add(tacGia);
        }

        // Cập nhật tác giả
        public void UpdateTacGia(QLTV_Database.TacGia tacGia)
        {
            tacGiaDAO.Update(tacGia);
        }

        // Xóa tác giả
        public void DeleteTacGia(string maTacGia)
        {
            tacGiaDAO.Delete(maTacGia);
        }
    }
}
