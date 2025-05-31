using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTV_DTO
{
    public class ChiTietPhieuMuon
    {
        [Key, Column(Order = 1)]
        public string MaPhieuMuon { get; set; }

        [Key, Column(Order = 2)]
        public string MaSach { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng mượn phải lớn hơn 0.")]
        public int SoLuongMuon { get; set; }

        public string TinhTrangSach { get; set; }

        [ForeignKey(nameof(MaPhieuMuon))]
        public DateTime? NgayTra { get; set; }
        public virtual PhieuMuon PhieuMuon { get; set; }

        [ForeignKey(nameof(MaSach))]
        public virtual Sach Sach { get; set; }
    }
}
