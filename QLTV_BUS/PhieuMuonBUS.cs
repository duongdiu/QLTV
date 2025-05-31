using System.Collections.Generic;
using QLTV_DAO;
using QLTV_DTO;

namespace QLTV_BUS
{
    public class PhieuMuonBUS
    {
        private PhieuMuonDAO phieuMuonDAO = new PhieuMuonDAO();

        public List<QLTV_Database.PhieuMuon> GetAllPhieuMuon()
        {
            return phieuMuonDAO.GetAll();
        }

        public QLTV_Database.PhieuMuon GetPhieuMuonById(string maPhieuMuon)
        {
            return phieuMuonDAO.GetById(maPhieuMuon);
        }

        public void AddPhieuMuon(QLTV_Database.PhieuMuon phieuMuon)
        {
            phieuMuonDAO.Add(phieuMuon);
        }

        public void UpdatePhieuMuon(QLTV_Database.PhieuMuon phieuMuon)
        {
            phieuMuonDAO.Update(phieuMuon);
        }

        public void DeletePhieuMuon(string maPhieuMuon)
        {
            phieuMuonDAO.Delete(maPhieuMuon);
        }
    }
}
