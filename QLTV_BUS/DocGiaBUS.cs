using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace BUS
{
    public class DocGiaBUS
    {
        private DocGiaDAO docGiaDAO = new DocGiaDAO();

        public List<QLTV_Database.DocGia> GetAllDocGia()
        {
            return docGiaDAO.GetAll();
        }

        public QLTV_Database.DocGia GetDocGiaById(string maDocGia)
        {
            return docGiaDAO.GetById(maDocGia);
        }


        public void AddDocGia(QLTV_Database.DocGia docGia)
        {
            docGiaDAO.Add(docGia);
        }

        public void UpdateDocGia(QLTV_Database.DocGia docGia)
        {
            docGiaDAO.Update(docGia);
        }

        public void DeleteDocGia(string maDocGia)
        {
            docGiaDAO.Delete(maDocGia);
        }
    }
}
