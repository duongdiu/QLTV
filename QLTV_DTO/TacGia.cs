using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLTV_DTO
{
    public class TacGia
    {

        public string MaTacGia { get; set; }  // Mã tác giả (Unique)
        public string TenTacGia { get; set; }  // Tên tác giả
        public int NamSinh { get; set; }  // Năm sinh của tác giả
        public string QueQuan { get; set; }  // Quê quán của tác giả
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
