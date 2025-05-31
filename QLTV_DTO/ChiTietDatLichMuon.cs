using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class ChiTietDatLichMuon
    {
        public string MaChiTiet { get; set; }     // Mã chi tiết  
        public string MaLichMuon { get; set; }     // Mã lịch mượn  
        public string MaSach { get; set; }         // Mã sách  
        public int SoLuong { get; set; }
    }
}
