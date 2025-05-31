using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class PhanHoiDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        public List<QLTV_Database.PhanHoi> GetAll()
        {
            return db.PhanHoiDatabase.ToList();
        }

        public List<QLTV_Database.PhanHoi> GetBySachId(string maSach)
        {
            return db.PhanHoiDatabase.Where(p => p.MaSach == maSach).ToList();
        }

        public void Add(QLTV_Database.PhanHoi phanHoi)
        {
            db.PhanHoiDatabase.Add(phanHoi);
            db.SaveChanges();
        }

        public void Delete(int maPhanHoi)
        {
            var phanHoi = db.PhanHoiDatabase.Find(maPhanHoi);
            if (phanHoi != null)
            {
                db.PhanHoiDatabase.Remove(phanHoi);
                db.SaveChanges();
            }
        }

        // ✅ Cập nhật trạng thái
        public void CapNhatTrangThai(int maPhanHoi, string trangThai)
        {
            var ph = db.PhanHoiDatabase.Find(maPhanHoi);
            if (ph != null)
            {
                ph.TrangThai = trangThai;
                db.SaveChanges();
            }
        }

        // ✅ Phản hồi admin
        public void GuiPhanHoiAdmin(int maPhanHoi, string noiDungPhanHoi)
        {
            var ph = db.PhanHoiDatabase.Find(maPhanHoi);
            if (ph != null)
            {
                ph.PhanHoiAdmin = noiDungPhanHoi;
                db.SaveChanges();
            }
        }

        // ✅ Tìm kiếm, lọc
        public List<QLTV_Database.PhanHoi> TimKiem(string keyword, string trangThai)
        {
            var query = db.PhanHoiDatabase.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.MaSach.Contains(keyword) || p.MaDocGia.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(trangThai) && trangThai != "Tất cả")
            {
                query = query.Where(p => p.TrangThai == trangThai);
            }

            return query.ToList();
        }
    }
}
