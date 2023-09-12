using DAL.Model;
using DAL.Services.PhieuMuon_Sachs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuons.DTO
{
    public class PhieuMuonCreateInput
    {      
        public string PhieuMuonId { set; get; }
        public string TenPhieuMuon { set; get; }
        public int NhanVienId { get;  set; }
        public int DocGiaId { get;  set; }
        public DateTime NgayMuon { set; get; }
        public DateTime NgayTra { set; get; }
        public string GhiChu { get; set; }
        public int TrangThaiId { get; set; }
        public List<PhieuMuon_SachCreateInput> ListSachMuon { set; get; }
    }
    
}
