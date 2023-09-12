using DAL.Common;
using DAL.Model;
using DAL.Services.NhanVien.DTO;
using DAL.Services.Sachs.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.NhanVien
{
    public interface INhanVienService
    {
        
        IQueryable<Model.NhanVien> QueryFilter(NhanVienFilterInput input = null);
        IQueryable<NhanVien_DTO> QueryFilterDto(NhanVienFilterInput input = null);
        Task<Model.NhanVien> GetById(int id);
        Task<NhanVien_DTO> GetByIdDto(int id);
        Task<PageResultDTO<NhanVien_DTO>> Paging(PagingInput<NhanVienFilterInput> input = null);
        Task<int> CreateNhanVien(NhanVienCreateInput input);
        Task<bool> UpdateNhanVien(int NhanVienid, NhanVienCreateInput input);
        Task<bool> DeleteNhanVienById(int NhanVienId);
    }
}
