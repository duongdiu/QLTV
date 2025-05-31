using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class TacGiaDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy tất cả tác giả
        public List<QLTV_Database.TacGia> GetAll()
        {
            return db.TacGiaDatabase
                     .Select(tg => new QLTV_Database.TacGia
                     {
                         MaTacGia = tg.MaTacGia,
                         TenTacGia = tg.TenTacGia,
                         NamSinh = tg.NamSinh,
                         QueQuan = tg.QueQuan
                     }).ToList();
        }

        // Lấy tác giả theo mã
        public QLTV_Database.TacGia GetById(string maTacGia)
        {
            var tg = db.TacGiaDatabase
                        .FirstOrDefault(t => t.MaTacGia == maTacGia);
            return tg != null ? new QLTV_Database.TacGia
            {
                MaTacGia = tg.MaTacGia,
                TenTacGia = tg.TenTacGia,
                NamSinh = tg.NamSinh,
                QueQuan = tg.QueQuan
            } : null;
        }

        // Thêm tác giả
        public void Add(QLTV_Database.TacGia tg)
        {
            if (tg != null)
            {
                var entity = new QLTV_Database.TacGia
                {
                    MaTacGia = tg.MaTacGia,
                    TenTacGia = tg.TenTacGia,
                    NamSinh = tg.NamSinh,
                    QueQuan = tg.QueQuan
                };

                db.TacGiaDatabase.Add(entity);
                db.SaveChanges();
            }
        }

        // Cập nhật tác giả
        public void Update(QLTV_Database.TacGia tg)
        {
            var existingTG = db.TacGiaDatabase
                               .FirstOrDefault(t => t.MaTacGia == tg.MaTacGia);
            if (existingTG != null)
            {
                existingTG.TenTacGia = tg.TenTacGia;
                existingTG.NamSinh = tg.NamSinh;
                existingTG.QueQuan = tg.QueQuan;

                db.SaveChanges();
            }
        }

        // Xóa tác giả
        public void Delete(string maTacGia)
        {
            var tg = db.TacGiaDatabase
                        .FirstOrDefault(t => t.MaTacGia == maTacGia);
            if (tg != null)
            {
                db.TacGiaDatabase.Remove(tg);
                db.SaveChanges();
            }
        }
    }
}
