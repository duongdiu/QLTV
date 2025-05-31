using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace QLTV_BUS
{
    public class ChiTietPhieuMuonBUS
    {
        private ChiTietPhieuMuonDAO chiTietPhieuMuonDAO = new ChiTietPhieuMuonDAO();

        // Lấy tất cả các chi tiết phiếu mượn
        public List<QLTV_Database.ChiTietPhieuMuon> GetAllChiTietPhieuMuon()
        {
            return chiTietPhieuMuonDAO.GetAll();
        }

        // Thêm chi tiết phiếu mượn
        public void AddChiTietPhieuMuon(QLTV_Database.ChiTietPhieuMuon chiTiet)
        {
            chiTietPhieuMuonDAO.Add(chiTiet);
        }

        // Xóa chi tiết phiếu mượn theo MaPhieuMuon và MaSach
        public void DeleteChiTietPhieuMuon(string maPhieuMuon, string maSach)
        {
            chiTietPhieuMuonDAO.Delete(maPhieuMuon, maSach);
        }
    }
}
