using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace BUS
{
    public class NhaXuatBanBUS
    {
        private NhaXuatBanDAO nhaXuatBanDAO = new NhaXuatBanDAO();

        // Lấy tất cả nhà xuất bản
        public List<QLTV_Database.NhaXuatBan> GetAllNhaXuatBan()
        {
            return nhaXuatBanDAO.GetAll();
        }

        // Lấy nhà xuất bản theo mã
        public QLTV_Database.NhaXuatBan GetNhaXuatBanById(string maNXB)
        {
            return nhaXuatBanDAO.GetById(maNXB);
        }

        // Thêm nhà xuất bản
        public void AddNhaXuatBan(QLTV_Database.NhaXuatBan nhaXuatBan)
        {
            nhaXuatBanDAO.Add(nhaXuatBan);
        }

        // Cập nhật nhà xuất bản
        public void UpdateNhaXuatBan(QLTV_Database.NhaXuatBan nhaXuatBan)
        {
            nhaXuatBanDAO.Update(nhaXuatBan);
        }

        // Xóa nhà xuất bản
        public void DeleteNhaXuatBan(string maNXB)
        {
            nhaXuatBanDAO.Delete(maNXB);
        }
    }
}
