using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLTV_DTO
{
    public class NhaXuatBan
    {

        public string MaNXB { get; set; }  // Mã nhà xuất bản (Unique)
        public string TenNXB { get; set; }  // Tên nhà xuất bản
        public string DiaChi { get; set; }  // Địa chỉ của nhà xuất bản
        public string Email { get; set; }  // Email của nhà xuất bản
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
