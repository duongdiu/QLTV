using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class PhanHoi
    {
       
        public int MaPhanHoi { get; set; }
        public string MaDocGia { get; set; }
        public string MaSach { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayGui { get; set; }
        public string TrangThai { get; set; }      // VD: "Chờ duyệt", "Đã duyệt", "Bị từ chối", "Ẩn"
        public string PhanHoiAdmin { get; set; }
    }
}
