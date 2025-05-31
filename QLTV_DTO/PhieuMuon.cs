using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class PhieuMuon
    {
        
        public string MaPhieuMuon { get; set; }
        public string MaDocGia { get; set; }
        public string TenDocGia { get; set; }
        public DateTime NgayMuon { get; set; }
        
        public DateTime NgayTraDuKien { get; set; }
        public decimal TienPhat { get; set; }
        public string TinhTrang { get; set; }

    }
}
