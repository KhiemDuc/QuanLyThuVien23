using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.DocGias.DTO
{
    public class DocGiaCreateInput
    {
        public string TenDocGia { get; set; }
        public string DiaChi { get; set; }

        public string SoDienThoai { set; get; }
        public string CCCD { get; set; }
        public byte[] AnhDocGia { set; get; }
    }
}
