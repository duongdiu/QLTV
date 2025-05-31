using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class LienHeDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy tất cả liên hệ
        public List<QLTV_Database.LienHe> GetAll()
        {
            return db.LienHeDatabase.ToList();
        }

        // Lấy liên hệ theo mã liên hệ
        public QLTV_Database.LienHe GetByMaLienHe(string maLienHe)
        {
            return db.LienHeDatabase.Find(maLienHe);
        }

        // Thêm mới liên hệ
        public void Add(QLTV_Database.LienHe lienHe)
        {
            db.LienHeDatabase.Add(lienHe);
            db.SaveChanges();
        }

        // Xóa liên hệ
        public void Delete(string maLienHe)
        {
            var lienHe = db.LienHeDatabase.Find(maLienHe);
            if (lienHe != null)
            {
                db.LienHeDatabase.Remove(lienHe);
                db.SaveChanges();
            }
        }

        

        // Tìm kiếm, lọc theo chủ đề hoặc email
        public List<QLTV_Database.LienHe> TimKiem(string keyword, string trangThai)
        {
            var query = db.LienHeDatabase.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(l => l.HoTen.Contains(keyword) || l.Email.Contains(keyword) || l.Chude.Contains(keyword));
            }

            

            return query.ToList();
        }
    }
}
