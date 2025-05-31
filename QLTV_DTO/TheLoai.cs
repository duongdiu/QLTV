using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLTV_DTO
{
    public class TheLoai
    {
       

        public string MaTheLoai { get; set; }  // Mã thể loại (Unique)
        public string TenTheLoai { get; set; }  // Tên thể loại
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
