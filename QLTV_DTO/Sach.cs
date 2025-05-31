using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTV_DTO
{
    public class Sach
    {
        public string MaSach { get; set; }  // Mã sách, sẽ được sinh tự động qua Trigger
        public string TenSach { get; set; }  // Tên sách
        public string MaTacGia { get; set; }  // Mã tác giả (liên kết với bảng TacGia)
        public string MaTheLoai { get; set; }
        public string MaViTri { get; set; }// Mã thể loại (liên kết với bảng TheLoai)
        public string MaNXB { get; set; }  // Mã nhà xuất bản (liên kết với bảng NhaXuatBan)

        [Range(1900, int.MaxValue, ErrorMessage = "Năm xuất bản phải lớn hơn hoặc bằng 1900.")]
        public int NamXuatBan { get; set; }  // Năm xuất bản của sách

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng sách phải lớn hơn hoặc bằng 0.")]

        public int SoLuong { get; set; }  // Số lượng sách còn lại trong kho
        [Range(0, double.MaxValue, ErrorMessage = "Giá tiền phải lớn hơn hoặc bằng 0.")]
        public decimal GiaTien { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số lần tái bản phải lớn hơn hoặc bằng 0.")]
        public int SoLanTaiBan { get; set; }

        public string AnhBia { get; set; }  // Đường dẫn đến ảnh bìa của sách
        public string GhiChu { get; set; }  // Ghi chú bổ sung (nếu có)
        // 👇 Thêm các navigation property để EF hiểu quan hệ
        [ForeignKey("MaTacGia")]
        public virtual TacGia TacGia { get; set; }

        [ForeignKey("MaTheLoai")]
        public virtual TheLoai TheLoai { get; set; }

        [ForeignKey("MaNXB")]
        public virtual NhaXuatBan NhaXuatBan { get; set; }
        [ForeignKey("MaViTri")]
        public virtual ViTri ViTri { get; set; }
    }
}
