using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class PhanQuyen
    {
        public string TenDangNhap { get; set; }
        public string MaChucNang { get; set; }
        public bool DuocTruyCap { get; set; }

        [NotMapped] // Nếu dùng Entity Framework, thêm annotation này để tránh mapping với CSDL
        public string TenChucNang { get; set; }
    }
}
