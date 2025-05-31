using QLTV_DAO;
using QLTV_DTO;
using System.Collections.Generic;

namespace QLTV_BUS
{
    public class ViTriBUS
    {
        private ViTriDAO dao = new ViTriDAO();

        public List<QLTV_Database.ViTri> LayTatCaViTri()
        {
            return dao.LayDanhSachViTri();
        }

        public QLTV_Database.ViTri TimViTriTheoMa(string ma)
        {
            return dao.LayViTriTheoMa(ma);
        }

        public bool ThemViTri(QLTV_Database.ViTri vt)
        {
            return dao.ThemViTri(vt);
        }

        public bool CapNhatViTri(QLTV_Database.ViTri vt)
        {
            return dao.CapNhatViTri(vt);
        }

        public bool XoaViTri(string ma)
        {
            return dao.XoaViTri(ma);
        }
    }
}
