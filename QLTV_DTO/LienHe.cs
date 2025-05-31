using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV_DTO
{
    public class LienHe
    {
        public string MaLienHe { get; set; }      // Mã liên hệ
        public string HoTen { get; set; }         // Họ tên người liên hệ
        public string Email { get; set; }         // Địa chỉ email
        public string Sdt { get; set; }           // Số điện thoại (tùy chọn)
        public string Chude { get; set; }         // Chủ đề phản hồi
        public string NoiDung { get; set; }       // Nội dung phản hồi
        public string FileDinhKem { get; set; }   // Đường dẫn tệp đính kèm (tùy chọn)
        public DateTime ThoiGianGui { get; set; }  // Thời gian gửi
    }
}
