using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class NguoiDung
    {
        
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string VaiTro { get; set; }
        public string MaDocGia { get; set; }
        // Thêm thuộc tính IsActive
        public bool IsActive { get; set; } = true;
        public string TrangThaiTaiKhoan => IsActive ? "Đang hoạt động" : "Không hoạt động";
    }
}
