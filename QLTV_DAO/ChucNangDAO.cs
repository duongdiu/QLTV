using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTV_Database;

namespace QLTV_DAO
{
    public class ChucNangDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        public List<QLTV_Database.ChucNang> LayTatCaChucNang()
        {
            return db.ChucNangDatabase.Select(c => new QLTV_Database.ChucNang
            {
                MaChucNang = c.MaChucNang,
                TenChucNang = c.TenChucNang,
                TenForm = c.TenForm
            }).ToList();
        }

        public QLTV_Database.ChucNang LayChucNangTheoMa(string maChucNang)
        {
            var chucNang = db.ChucNangDatabase.FirstOrDefault(c => c.MaChucNang == maChucNang);
            return chucNang != null ? new QLTV_Database.ChucNang
            {
                MaChucNang = chucNang.MaChucNang,
                TenChucNang = chucNang.TenChucNang,
                TenForm = chucNang.TenForm
            } : null;
        }
    }
}
