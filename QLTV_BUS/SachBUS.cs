using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace BUS
{
    public class SachBUS
    {
        private SachDAO sachDAO = new SachDAO();

        public List<QLTV_Database.Sach> GetAllSach()
        {
            return sachDAO.GetAll();
        }

        public QLTV_Database.Sach GetSachById(string maSach)
        {
            return sachDAO.GetById(maSach);
        }

        public void AddSach(QLTV_Database.Sach sach)
        {
            sachDAO.Add(sach);
        }

        public void UpdateSach(QLTV_Database.Sach sach)
        {
            sachDAO.Update(sach);
        }

        public void DeleteSach(string maSach)
        {
            sachDAO.Delete(maSach);
        }
    }
}
