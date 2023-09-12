using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuons.DTO
{
    public class PhieuMuonFilterInput
    {
        public string PhieuMuonId { set; get; }
        public string TenPhieuMuon { set; get; }
        public string TenNhanVien { set; get; }
        public string TenDocGia { set; get; }
        public int? TrangThaiId { get; set; }
        public string TenTrangThai { set; get; }
        public DateTime? NgayMuon1 { get; set; }
        public DateTime? NgayMuon2 { get; set; }
    }
}
