using DAL.Common;
using DAL.Model;
using DAL.Services.DocGias.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.DocGias
{
    public interface IDocGiaService
    {
        IQueryable<DocGia> QueryFilter(DocGiaFilterInput input = null);
        IQueryable<DocGia_DTO> QueryFilterDto(DocGiaFilterInput input = null);
        Task<DocGia> GetById(int id);
        Task<DocGia_DTO> GetByIdDto(int id);
        Task<PageResultDTO<DocGia_DTO>> Paging(PagingInput<DocGiaFilterInput> input = null);
        Task<int> CreateDocGia(DocGiaCreateInput input);
        Task<bool> UpdateDocGia(int DocGiaId, DocGiaCreateInput input);
        Task<bool> DeleteDocGiaById(int DocGiaId);
    }
}
