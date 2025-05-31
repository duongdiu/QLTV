using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class PhieuMuonDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        public List<QLTV_Database.PhieuMuon> GetAll()
        {
            return db.PhieuMuonDatabase.ToList();
        }

        public QLTV_Database.PhieuMuon GetById(string maPhieuMuon)
        {
            return db.PhieuMuonDatabase.FirstOrDefault(p => p.MaPhieuMuon == maPhieuMuon);
        }

        public void Add(QLTV_Database.PhieuMuon phieuMuon)
        {
            db.PhieuMuonDatabase.Add(phieuMuon);
            db.SaveChanges();
        }

        public void Update(QLTV_Database.PhieuMuon phieuMuon)
        {
            var existing = db.PhieuMuonDatabase.Find(phieuMuon.MaPhieuMuon);
            if (existing != null)
            {
               
                existing.TinhTrang = phieuMuon.TinhTrang;
                db.SaveChanges();
            }
        }

        public void Delete(string maPhieuMuon)
        {
            var phieuMuon = db.PhieuMuonDatabase.Find(maPhieuMuon);
            if (phieuMuon != null)
            {
                db.PhieuMuonDatabase.Remove(phieuMuon);
                db.SaveChanges();
            }
        }
    }
}
