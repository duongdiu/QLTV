using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace QLTV_BUS
{
    public class PhanHoiBUS
    {
        private PhanHoiDAO phanHoiDAO = new PhanHoiDAO();

        public List<QLTV_Database.PhanHoi> GetAllPhanHoi()
        {
            return phanHoiDAO.GetAll();
        }

        public void AddPhanHoi(QLTV_Database.PhanHoi phanHoi)
        {
            phanHoiDAO.Add(phanHoi);
        }

        public void DeletePhanHoi(int maPhanHoi)
        {
            phanHoiDAO.Delete(maPhanHoi);
        }

        // ✅ Cập nhật trạng thái
        public void CapNhatTrangThai(int maPhanHoi, string trangThai)
        {
            phanHoiDAO.CapNhatTrangThai(maPhanHoi, trangThai);
        }

        // ✅ Gửi phản hồi từ admin
        public void GuiPhanHoiAdmin(int maPhanHoi, string noiDungPhanHoi)
        {
            phanHoiDAO.GuiPhanHoiAdmin(maPhanHoi, noiDungPhanHoi);
        }

        // ✅ Tìm kiếm / Lọc bình luận
        public List<QLTV_Database.PhanHoi> TimKiemPhanHoi(string keyword, string trangThai)
        {
            return phanHoiDAO.TimKiem(keyword, trangThai);
        }
    }
}
