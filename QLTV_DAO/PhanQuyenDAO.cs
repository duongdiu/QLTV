using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLTV_Database;

namespace QLTV_DAO
{
    public class PhanQuyenDAO
    {
        private LibraryDBContext db = new LibraryDBContext();

        // Lấy quyền theo vai trò
        public List<QLTV_Database.PhanQuyen> LayPhanQuyenTheoVaiTro(string TenDangNhap)
        {
            return db.PhanQuyenDatabase
                .Where(p => p.TenDangNhap == TenDangNhap)
                .Select(p => new QLTV_Database.PhanQuyen
                {
                    TenDangNhap = p.TenDangNhap,
                    MaChucNang = p.MaChucNang,
                    DuocTruyCap = p.DuocTruyCap
                }).ToList();
        }

        // Cập nhật quyền cho vai trò và chức năng
        public void CapNhatPhanQuyen(string TenDangNhap, string maChucNang, bool duocTruyCap)
        {
            var phanQuyen = db.PhanQuyenDatabase
                .FirstOrDefault(p => p.TenDangNhap == TenDangNhap && p.MaChucNang == maChucNang);

            if (phanQuyen != null)
            {
                phanQuyen.DuocTruyCap = duocTruyCap;
            }
            else
            {
                db.PhanQuyenDatabase.Add(new QLTV_Database.PhanQuyen
                {
                    TenDangNhap = TenDangNhap,
                    MaChucNang = maChucNang,
                    DuocTruyCap = duocTruyCap
                });
            }

            db.SaveChanges();
        }
    }
    }
