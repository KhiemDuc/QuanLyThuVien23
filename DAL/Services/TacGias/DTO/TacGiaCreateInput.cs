using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.TacGias.DTO
{
    public class TacGiaCreateInput
    {
        public string TenTacGia { set; get; }
        public string SoDienThoai { set; get; }
        public string DiaChi { set; get; }
        public DateTime ?NamSinh { set; get; }
        public DateTime ?NamMat { set; get; }
        public string MoTa { set; get; }
        public byte[] AnhTacGia { set; get; }
    }
}
