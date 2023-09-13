using DAL.Common;
using DAL.Model;
using DAL.Services.PhieuMuon_Sachs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuon_PhieuMuon_Sachs
{
    public interface IPhieuMuon_SachsService
    {
        IQueryable<Model.PhieuMuon_Sachs> QueryFilter(PhieuMuon_SachFilterInput input = null);
        IQueryable<PhieuMuon_Sach_DTO> QueryFilterDto(PhieuMuon_SachFilterInput input = null);
        Task<PageResultDTO<PhieuMuon_Sach_DTO>> Paging(PagingInput<PhieuMuon_SachFilterInput> input = null);
        Task<int> CreatePhieuMuon_Sach(PhieuMuon_SachCreateInput input);
        Task<int> UpdatePhieuMuon_Sach(PhieuMuon_SachFilterInput filter, PhieuMuon_SachCreateInput input);
        Task<int> DeletePhieuMuon_SachById(PhieuMuon_SachFilterInput filter);
        int getTongSachChoMuon(PhieuMuon_SachFilterInput input = null);
        int GetSoLuongTheLoaiMuonTrongThang(int month, int year);
        Dictionary<int, int> GetNgayMuonVaSoLuong(int? month, int? year);
        Dictionary<string, int> GetSoLuongSachTrongThang(int? month, int? year );
    }
}
