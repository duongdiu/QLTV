using System;
using System.Collections.Generic;
using QLTV_Database;
using QLTV_DAO;
using QLTV_DTO;

namespace QLTV_BUS
{
    public class LienHeBUS
    {
        private LienHeDAO lienHeDAO = new LienHeDAO();

        // Thêm mới liên hệ
        public bool AddLienHe(QLTV_Database.LienHe lienHeDTO)
        {
            // Kiểm tra dữ liệu hợp lệ
            if (string.IsNullOrEmpty(lienHeDTO.HoTen))
            {
                throw new ArgumentException("Họ tên không được để trống.");
            }
            if (string.IsNullOrEmpty(lienHeDTO.Email))
            {
                throw new ArgumentException("Email không được để trống.");
            }
            if (string.IsNullOrEmpty(lienHeDTO.NoiDung))
            {
                throw new ArgumentException("Nội dung liên hệ không được để trống.");
            }

            // Chuyển DTO thành đối tượng Entity (Database Entity)
            var lienHe = new QLTV_Database.LienHe
            {
                MaLienHe = lienHeDTO.MaLienHe,
                HoTen = lienHeDTO.HoTen,
                Email = lienHeDTO.Email,
                Sdt = lienHeDTO.Sdt,
                Chude = lienHeDTO.Chude,
                NoiDung = lienHeDTO.NoiDung,
                FileDinhKem = lienHeDTO.FileDinhKem,
                ThoiGianGui = lienHeDTO.ThoiGianGui,
                
            };

            // Gọi DAO để thêm vào cơ sở dữ liệu
            lienHeDAO.Add(lienHe);
            return true;
        }

        // Lấy tất cả liên hệ
        public List<QLTV_Database.LienHe> GetAllLienHe()
        {
            var lienHeList = lienHeDAO.GetAll();
            List<QLTV_Database.LienHe> lienHeDTOList = new List<QLTV_Database.LienHe>();

            foreach (var lienHe in lienHeList)
            {
                lienHeDTOList.Add(new QLTV_Database.LienHe
                {
                    MaLienHe = lienHe.MaLienHe,
                    HoTen = lienHe.HoTen,
                    Email = lienHe.Email,
                    Sdt = lienHe.Sdt,
                    Chude = lienHe.Chude,
                    NoiDung = lienHe.NoiDung,
                    FileDinhKem = lienHe.FileDinhKem,
                    ThoiGianGui = lienHe.ThoiGianGui,
                    
                });
            }

            return lienHeDTOList;
        }

        // Lấy liên hệ theo mã
        public QLTV_Database.LienHe GetLienHeById(string maLienHe)
        {
            var lienHe = lienHeDAO.GetByMaLienHe(maLienHe);
            if (lienHe != null)
            {
                return new QLTV_Database.LienHe
                {
                    MaLienHe = lienHe.MaLienHe,
                    HoTen = lienHe.HoTen,
                    Email = lienHe.Email,
                    Sdt = lienHe.Sdt,
                    Chude = lienHe.Chude,
                    NoiDung = lienHe.NoiDung,
                    FileDinhKem = lienHe.FileDinhKem,
                    ThoiGianGui = lienHe.ThoiGianGui,
                   
                };
            }
            return null;
        }

        // Cập nhật trạng thái liên hệ
       
        // Tìm kiếm, lọc liên hệ theo từ khóa và trạng thái
        public List<QLTV_Database.LienHe> SearchLienHe(string keyword, string trangThai)
        {
            var lienHeList = lienHeDAO.TimKiem(keyword, trangThai);
            List<QLTV_Database.LienHe> lienHeDTOList = new List<QLTV_Database.LienHe>();

            foreach (var lienHe in lienHeList)
            {
                lienHeDTOList.Add(new QLTV_Database.LienHe
                {
                    MaLienHe = lienHe.MaLienHe,
                    HoTen = lienHe.HoTen,
                    Email = lienHe.Email,
                    Sdt = lienHe.Sdt,
                    Chude = lienHe.Chude,
                    NoiDung = lienHe.NoiDung,
                    FileDinhKem = lienHe.FileDinhKem,
                    ThoiGianGui = lienHe.ThoiGianGui,
                    
                });
            }

            return lienHeDTOList;
        }

        // Xóa liên hệ
        public bool DeleteLienHe(string maLienHe)
        {
            lienHeDAO.Delete(maLienHe);
            return true;
        }
    }
}
