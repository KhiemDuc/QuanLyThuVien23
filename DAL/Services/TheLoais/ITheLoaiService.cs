using DAL.Common;
using DAL.Services.TheLoais.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.TheLoais
{
    public interface ITheLoaiService
    {
        IQueryable<Model.TheLoai> QueryFilter(TheLoaiFilterInput input = null);
        IQueryable<TheLoai_DTO> QueryFilterDto(TheLoaiFilterInput input = null);
        Task<Model.TheLoai> GetById(string id);
        Task<TheLoai_DTO> GetByIdDto(string id);
        Task<PageResultDTO<TheLoai_DTO>> Paging(PagingInput<TheLoaiFilterInput> input = null);
        Task<int> CreateTheLoai(TheLoaiCreateInput input);
        Task<bool> UpdateTheLoai(string TheLoaiid, TheLoaiCreateInput input);
        Task<bool> DeleteTheLoaiById(string TheLoaiId);
        int SoLuongTheLoai();
    }
}
