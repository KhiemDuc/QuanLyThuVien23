using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuons.DTO
{
    public class PhieuMuon_DTO
    {
        public string TenPhieuMuon { get; set; }
        public string PhieuMuonId { get; set; }
        public string TenNhanVien { get; set; }
        public int NhanVienId { set; get; }
        public string TenDocGia { set; get; }
        public int DocGiaId { set; get; }
        public DateTime NgayMuon { set; get; }
        public DateTime NgayTra { set; get; }
        public List<Model.PhieuMuon_Sachs> listSachMuon { get; set; }
        public int TrangThaiId { get; set; }
        public string TenTrangThai { set; get; }
        public string GhiChu { set; get; }
    }
}
