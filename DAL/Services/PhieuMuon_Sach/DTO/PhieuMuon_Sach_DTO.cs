using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuon_Sachs
{
    public class PhieuMuon_Sach_DTO
    {
        public string PhieuMuonId { set; get; }
        public string TenPhieuMuon { set; get; }
        public int SachId { set; get; }
        public string TenSach { set; get; }
        public string TacGia { set; get; }
        public List<TheLoai> listTheLoai { set; get; }
        public string TenTheLoai => listTheLoai?.Any() != true ? null : listTheLoai.Select(p => p.TenTheLoai).Aggregate((a, b) => a + ", " + b);
        public int SoLuong { set; get; }
    }
}
