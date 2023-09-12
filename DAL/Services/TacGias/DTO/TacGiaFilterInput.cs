using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.TacGias.DTO
{
    public class TacGiaFilterInput
    {
        public string TenTacGia { set; get; }
        public string DiaChi { set; get; }
        public int? NamSinh { set; get; }
        public int? NamMat { set; get; }
    }
}
