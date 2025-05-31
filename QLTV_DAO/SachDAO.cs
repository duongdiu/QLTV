using System;
using System.Collections.Generic;
using System.Linq;
using QLTV_DTO;
using QLTV_Database;
using System.Data.Entity;

namespace QLTV_DAO
{
    public class SachDAO
    {
        private LibraryDBContext db;

        public SachDAO()
        {
            db = new LibraryDBContext();
        }

        // Lấy danh sách tất cả sách, kèm thông tin Tác Giả, Thể Loại, Nhà Xuất Bản
        public List<QLTV_Database.Sach> GetAll()
        {
            return db.SachDatabase
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Include(s => s.NhaXuatBan)
                .ToList();
        }

        // Thêm sách mới
        public bool Add(QLTV_Database.Sach sach)
        {
            try
            {
                db.SachDatabase.Add(sach);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi thêm sách: " + ex.Message);
                return false;
            }
        }

        // Cập nhật thông tin sách
        public bool Update(QLTV_Database.Sach sach)
        {
            try
            {
                var existingBook = db.SachDatabase.FirstOrDefault(s => s.MaSach == sach.MaSach);
                if (existingBook == null) return false;

                existingBook.TenSach = sach.TenSach;
                existingBook.MaTacGia = sach.MaTacGia;
                existingBook.MaTheLoai = sach.MaTheLoai;
                existingBook.MaNXB = sach.MaNXB;
                existingBook.NamXuatBan = sach.NamXuatBan;
                existingBook.GiaTien = sach.GiaTien;
                existingBook.SoLuong = sach.SoLuong;
                existingBook.GhiChu = sach.GhiChu;

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật sách: " + ex.Message);
                return false;
            }
        }

        // Xóa sách theo mã sách
        public bool Delete(string maSach)
        {
            try
            {
                var sach = db.SachDatabase.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null) return false;

                db.SachDatabase.Remove(sach);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa sách: " + ex.Message);
                return false;
            }
        }

        // Lấy thông tin sách theo mã sách
        public QLTV_Database.Sach GetById(string maSach)
        {
            return db.SachDatabase
                .Include(s => s.TacGia)
                .Include(s => s.TheLoai)
                .Include(s => s.NhaXuatBan)
                .FirstOrDefault(s => s.MaSach == maSach);
        }
    }
}
