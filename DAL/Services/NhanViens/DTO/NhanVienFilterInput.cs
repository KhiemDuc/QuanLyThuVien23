using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.NhanVien.DTO
{
    public class NhanVienFilterInput
    {
        public string TenNhanVien { get; set; }
        public string DiaChi {get; set;}
        public string TenVaiTro { get;  set; }
        public string SoDienThoai { set; get; } 
        public string CCCD { set; get; }
    }
}
