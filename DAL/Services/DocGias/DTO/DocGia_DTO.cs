using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.DocGias.DTO
{
    public class DocGia_DTO
    {
        public int Id;
        public string TenDocGia { get; set; }
        public string DiaChi { get; set; }
        public byte?[] AnhDocGia { set; get; }
        public string SoDienThoai { set; get; }
        public string CCCD { get; set; }
    }
}
