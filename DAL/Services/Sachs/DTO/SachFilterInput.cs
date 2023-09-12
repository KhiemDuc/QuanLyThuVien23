using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.Sachs.DTO
{
    public class SachFilterInput
    {
        public string TenSach { set; get; }
        public int namBatDau { set; get; }
        public int namKetThuc { set; get; }
        public string TenTacGia { set; get; }
        public  string TenNhaPhanPhoi { set; get; }
        public List<TheLoai> listTheLoai { set; get; }
    }
}
