using QLTV_Database;
using QLTV_DTO;
using System.Collections.Generic;
using System.Linq;

namespace QLTV_DAO
{
    public class ViTriDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        public List<QLTV_Database.ViTri> LayDanhSachViTri()
        {
            return db.ViTriDatabase.ToList();
        }

        public QLTV_Database.ViTri LayViTriTheoMa(string ma)
        {
            return db.ViTriDatabase.FirstOrDefault(v => v.MaViTri == ma);
        }

        public bool ThemViTri(QLTV_Database.ViTri vt)
        {
            if (db.ViTriDatabase.Any(v => v.MaViTri == vt.MaViTri))
                return false;

            db.ViTriDatabase.Add(vt);
            db.SaveChanges();
            return true;
        }

        public bool CapNhatViTri(QLTV_Database.ViTri vt)
        {
            var viTri = db.ViTriDatabase.Find(vt.MaViTri);
            if (viTri == null) return false;

            viTri.Tu = vt.Tu;
            viTri.Ke = vt.Ke;
            viTri.Tang = vt.Tang;
            db.SaveChanges();
            return true;
        }

        public bool XoaViTri(string ma)
        {
            var viTri = db.ViTriDatabase.Find(ma);
            if (viTri == null) return false;

            db.ViTriDatabase.Remove(viTri);
            db.SaveChanges();
            return true;
        }
    }
}
