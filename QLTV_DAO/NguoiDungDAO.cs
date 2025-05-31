using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_Database;
using QLTV_DTO;

namespace QLTV_DAO
{
    public class NguoiDungDAO
    {
        private readonly LibraryDBContext _context;

        public NguoiDungDAO()
        {
            _context = new LibraryDBContext();
        }

        public List<QLTV_Database.NguoiDung> GetAll()
        {
            return _context.NguoiDungDatabase.ToList();
        }

        public QLTV_Database.NguoiDung GetById(string tenDangNhap)
        {
            return _context.NguoiDungDatabase.Find(tenDangNhap);
        }

        public void Add(QLTV_Database.NguoiDung nguoiDung)
        {
            _context.NguoiDungDatabase.Add(nguoiDung);
            _context.SaveChanges();
        }

        public void Update(QLTV_Database.NguoiDung nguoiDung)
        {
            var existing = _context.NguoiDungDatabase.Find(nguoiDung.TenDangNhap);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(nguoiDung);
                _context.SaveChanges();
            }
        }

        public void Delete(string tenDangNhap)
        {
            var nguoiDung = _context.NguoiDungDatabase.Find(tenDangNhap);
            if (nguoiDung != null)
            {
                _context.NguoiDungDatabase.Remove(nguoiDung);
                _context.SaveChanges();
            }
        }
    }
}
