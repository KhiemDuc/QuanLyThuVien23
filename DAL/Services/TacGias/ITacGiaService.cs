using DAL.Common;
using DAL.Model;
using DAL.Services.TacGias.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.TacGias
{
    public interface ITacGiaService
    {
        IQueryable<TacGia> QueryFilter(TacGiaFilterInput input = null);
        IQueryable<TacGia_DTO> QueryFilterDto(TacGiaFilterInput input = null);
        Task<TacGia> GetById(int id);
        Task<TacGia_DTO> GetByIdDto(int id);
        Task<PageResultDTO<TacGia_DTO>> Paging(PagingInput<TacGiaFilterInput> input = null);
        Task<int> CreateTacGia(TacGiaCreateInput input);
        Task<bool> UpdateTacGia(int TacGiaId, TacGiaCreateInput input);
        Task<bool> DeleteTacGiaById(int TacGiaId);
        Task<List<string>> getAllNameTacGia(TacGiaFilterInput input = null);
        int SoLuongTacGia();
    }
}
