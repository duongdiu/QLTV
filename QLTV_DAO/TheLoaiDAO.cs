using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class TheLoaiDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy tất cả thể loại
        public List<QLTV_Database.TheLoai> GetAll()
        {
            return db.TheLoaiDatabase
                     .Select(tl => new QLTV_Database.TheLoai
                     {
                         MaTheLoai = tl.MaTheLoai,
                         TenTheLoai = tl.TenTheLoai
                     }).ToList();
        }

        // Lấy thể loại theo mã
        public QLTV_Database.TheLoai GetById(string maTheLoai)
        {
            var tl = db.TheLoaiDatabase
                        .FirstOrDefault(t => t.MaTheLoai == maTheLoai);
            return tl != null ? new QLTV_Database.TheLoai 
            {
                MaTheLoai = tl.MaTheLoai,
                TenTheLoai = tl.TenTheLoai
            } : null;
        }

        // Thêm thể loại
        public void Add(QLTV_Database.TheLoai tl)
        {
            if (tl != null)
            {
                var entity = new QLTV_Database.TheLoai
                {
                    MaTheLoai = tl.MaTheLoai,
                    TenTheLoai = tl.TenTheLoai
                };

                db.TheLoaiDatabase.Add(entity);
                db.SaveChanges();
            }
        }

        // Cập nhật thể loại
        public void Update(QLTV_Database.TheLoai tl)
        {
            var existingTL = db.TheLoaiDatabase
                               .FirstOrDefault(t => t.MaTheLoai == tl.MaTheLoai);
            if (existingTL != null)
            {
                existingTL.TenTheLoai = tl.TenTheLoai;

                db.SaveChanges();
            }
        }

        // Xóa thể loại
        public void Delete(string maTheLoai)
        {
            var tl = db.TheLoaiDatabase
                        .FirstOrDefault(t => t.MaTheLoai == maTheLoai);
            if (tl != null)
            {
                db.TheLoaiDatabase.Remove(tl);
                db.SaveChanges();
            }
        }
    }
}
