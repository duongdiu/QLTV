using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_DTO;
using QLTV_Database;

namespace QLTV_DAO
{
    public class DocGiaDAO
    {
        private LibraryDBContext db;

        public DocGiaDAO()
        {
            db = new LibraryDBContext();
        }

        // Lấy danh sách tất cả độc giả
        public List<QLTV_Database.DocGia> GetAll()
        {
            return db.DocGiaDatabase.ToList();
        }

        // Thêm độc giả mới
        public bool Add(QLTV_Database.DocGia docGia)
        {
            try
            {
                db.DocGiaDatabase.Add(docGia);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Sửa thông tin độc giả
        public bool Update(QLTV_Database.DocGia docGia)
        {
            try
            {
                var existingDocGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == docGia.MaDocGia);
                if (existingDocGia == null) return false;

                existingDocGia.HoTen = docGia.HoTen;
                existingDocGia.NgaySinh = docGia.NgaySinh;
                existingDocGia.GioiTinh = docGia.GioiTinh;
                existingDocGia.DiaChi = docGia.DiaChi;
                existingDocGia.Email = docGia.Email;
                existingDocGia.SoDienThoai = docGia.SoDienThoai;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Xóa độc giả
        public bool Delete(string maDocGia)
        {
            try
            {
                var docGia = db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == maDocGia);
                if (docGia == null) return false;

                db.DocGiaDatabase.Remove(docGia);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Tìm độc giả theo mã hoặc tên
        public List<QLTV_Database.DocGia> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return new List<QLTV_Database.DocGia>();

            return db.DocGiaDatabase.Where(d => d.MaDocGia.Contains(keyword) || d.HoTen.Contains(keyword)).ToList();
        }
        public QLTV_Database.DocGia GetById(string maDocGia)
        {
            return db.DocGiaDatabase.FirstOrDefault(d => d.MaDocGia == maDocGia);
        }
    }
}
