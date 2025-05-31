using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class DatLichMuon
    {
        public string MaLichMuon { get; set; }  // Mã lịch mượn  
        public string MaDocGia { get; set; }    // Mã độc giả  
        public DateTime NgayDenMuon { get; set; } // Ngày đến mượn  
        public TimeSpan GioDenMuon { get; set; } // Giờ đến mượn  
        public string TrangThai { get; set; }    // Trạng thái  
        public string GhiChu { get; set; }
    }
}
