using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class ViTri
    {
        [Key]
        [StringLength(10)]
        public string MaViTri { get; set; }

        [StringLength(50)]
        public string Tu { get; set; }

        [StringLength(50)]
        public string Ke { get; set; }

        [StringLength(50)]
        public string Tang { get; set; }
        [StringLength(50)]
        public string Kho { get; set; } // 

        // (Tuỳ chọn) Hiển thị tên vị trí dạng gộp
        [NotMapped]
        public string TenViTri => $"{Kho} - {Tu} - {Ke} - {Tang}";
    }
}
