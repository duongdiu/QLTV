using System.Collections.Generic;
using System.Linq;
using QLTV_Database; // Sử dụng DbContext  
using QLTV_DTO;     // Sử dụng DTO  

namespace QLTV_DAO
{
    public class NhaXuatBanDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy tất cả các nhà xuất bản
        public List<QLTV_Database.NhaXuatBan> GetAll()
        {
            return db.NhaXuatBanDatabase
                     .Select(nxb => new QLTV_Database.NhaXuatBan
                     {
                         MaNXB = nxb.MaNXB,
                         TenNXB = nxb.TenNXB,
                         DiaChi = nxb.DiaChi,
                         Email = nxb.Email
                     }).ToList();
        }

        // Lấy nhà xuất bản theo mã
        public QLTV_Database.NhaXuatBan GetById(string maNXB)
        {
            var nxb = db.NhaXuatBanDatabase
                        .FirstOrDefault(n => n.MaNXB == maNXB);
            return nxb != null ? new QLTV_Database.NhaXuatBan
            {
                MaNXB = nxb.MaNXB,
                TenNXB = nxb.TenNXB,
                DiaChi = nxb.DiaChi,
                Email = nxb.Email
            } : null;
        }

        // Thêm nhà xuất bản
        public void Add(QLTV_Database.NhaXuatBan nxb)
        {
            if (nxb != null)
            {
                var entity = new QLTV_Database.NhaXuatBan
                {
                    MaNXB = nxb.MaNXB,
                    TenNXB = nxb.TenNXB,
                    DiaChi = nxb.DiaChi,
                    Email = nxb.Email
                };

                db.NhaXuatBanDatabase.Add(entity);
                db.SaveChanges();
            }
        }

        // Cập nhật nhà xuất bản
        public void Update(QLTV_Database.NhaXuatBan nxb)
        {
            var existingNXB = db.NhaXuatBanDatabase
                                 .FirstOrDefault(n => n.MaNXB == nxb.MaNXB);
            if (existingNXB != null)
            {
                existingNXB.TenNXB = nxb.TenNXB;
                existingNXB.DiaChi = nxb.DiaChi;
                existingNXB.Email = nxb.Email;

                db.SaveChanges();
            }
        }

        // Xóa nhà xuất bản
        public void Delete(string maNXB)
        {
            var nxb = db.NhaXuatBanDatabase
                        .FirstOrDefault(n => n.MaNXB == maNXB);
            if (nxb != null)
            {
                db.NhaXuatBanDatabase.Remove(nxb);
                db.SaveChanges();
            }
        }
    }
}
