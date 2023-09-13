using DAL.Common;
using DAL.Model;
using DAL.Services.PhieuMuons.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuons
{
    public interface IPhieuMuonService
    {
        IQueryable<PhieuMuon> QueryFilter(PhieuMuonFilterInput input = null);
        IQueryable<PhieuMuon_DTO> QueryFilterDto(PhieuMuonFilterInput input = null);
        Task<PhieuMuon> GetById(string id);
        Task<PhieuMuon_DTO> GetByIdDto(string id);
        Task<PageResultDTO<PhieuMuon_DTO>> Paging(PagingInput<PhieuMuonFilterInput> input = null);
        Task<string> CreatePhieuMuon(PhieuMuonCreateInput input);
        Task<bool> UpdatePhieuMuon(string PhieuMuonId, PhieuMuonCreateInput input);
        Task<bool> DeletePhieuMuonById(string PhieuMuonId);
        List<int> GetNamTrongPhieuMuon();


    }
}
