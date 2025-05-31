using System.Collections.Generic;
using System.Linq;
using QLTV_Database; // Sử dụng DbContext  
using QLTV_DTO;     // Sử dụng DTO  

namespace QLTV_DAO
{
    public class ChiTietPhieuMuonDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy tất cả các chi tiết phiếu mượn  
        public List<QLTV_Database.ChiTietPhieuMuon> GetAll()
        {
            // Chuyển đổi từ ChiTietPhieuMuon trong QLTV_Database sang QLTV_DTO  
            return db.ChiTietPhieuMuonDatabase
                     .Select(ct => new QLTV_Database.ChiTietPhieuMuon
                     {
                         MaPhieuMuon = ct.MaPhieuMuon,
                         MaSach = ct.MaSach,
                         SoLuongMuon = ct.SoLuongMuon,
                         TinhTrangSach = ct.TinhTrangSach
                     }).ToList();
        }

        // Lấy chi tiết phiếu mượn theo mã phiếu mượn  
        public List<QLTV_Database.ChiTietPhieuMuon> GetByPhieuMuonId(string maPhieuMuon)
        {
            return db.ChiTietPhieuMuonDatabase
                     .Where(ct => ct.MaPhieuMuon == maPhieuMuon)
                     .Select(ct => new QLTV_Database.ChiTietPhieuMuon
                     {
                         MaPhieuMuon = ct.MaPhieuMuon,
                         MaSach = ct.MaSach,
                         SoLuongMuon = ct.SoLuongMuon,
                         TinhTrangSach = ct.TinhTrangSach
                     }).ToList();
        }

        // Thêm chi tiết phiếu mượn  
        public void Add(QLTV_Database.ChiTietPhieuMuon chiTiet)
        {
            if (chiTiet != null)
            {
                var entity = new QLTV_Database.ChiTietPhieuMuon
                {
                    MaPhieuMuon = chiTiet.MaPhieuMuon,
                    MaSach = chiTiet.MaSach,
                    SoLuongMuon = chiTiet.SoLuongMuon,
                    TinhTrangSach = chiTiet.TinhTrangSach
                };

                db.ChiTietPhieuMuonDatabase.Add(entity);
                db.SaveChanges();
            }
        }

        // Xóa chi tiết phiếu mượn  
        public void Delete(string maPhieuMuon, string maSach)
        {
            var chiTiet = db.ChiTietPhieuMuonDatabase
                            .FirstOrDefault(ct => ct.MaPhieuMuon == maPhieuMuon && ct.MaSach == maSach);
            if (chiTiet != null)
            {
                db.ChiTietPhieuMuonDatabase.Remove(chiTiet);
                db.SaveChanges();
            }
        }
    }
}
