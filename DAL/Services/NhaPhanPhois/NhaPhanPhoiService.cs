using DAL.Common;
using DAL.Model;
using DAL.Services.NhaPhanPhois.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.NhaPhanPhois
{
    public class NhaPhanPhoiService : INhaPhanPhoiService
    {
        public readonly QuanLyThuVienEntities _db;
        public NhaPhanPhoiService()
        {
            _db = new QuanLyThuVienEntities();
        }

        #region Crud
        public async Task<int> CreateNhaPhanPhoi(NhaPhanPhoiCreateInput input)
        {
            var entity = await MapperCreateInputToEntity(input, new Model.NhaPhanPhoi());
            _db.NhaPhanPhois.Add(entity);
            return _db.SaveChanges();
        }

        public async Task<bool> UpdateNhaPhanPhoi(string NhaPhanPhoiId, NhaPhanPhoiCreateInput input)
        {
            var entity = await GetById(NhaPhanPhoiId);
            entity = await MapperCreateInputToEntity(input, entity);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteNhaPhanPhoiById(string NhaPhanPhoiId)
        {
            var entity = await GetById(NhaPhanPhoiId);
            if (entity != null)
            {
                var nhaPhanPhoiLienKet = _db.Saches.Where(s => s.ID_NhaPhanPhoi == NhaPhanPhoiId).ToList();
                foreach (var s in nhaPhanPhoiLienKet)
                {
                    s.NhaPhanPhoi = null;
                }
                await _db.SaveChangesAsync();
                _db.NhaPhanPhois.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Model.NhaPhanPhoi> GetById(string id)
        {
            return await QueryFilter().FirstOrDefaultAsync(p => p.ID == id) ?? throw new Exception($"Không tìm thấy nhà phân phối có id {id}.");
        }

        public async Task<NhaPhanPhoi_DTO> GetByIdDto(string id)
        {
            return await QueryFilterDto().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception($"Không tìm thấy nhân viên có id {id}.");
        }

        #endregion

        public async Task<PageResultDTO<NhaPhanPhoi_DTO>> Paging(PagingInput<NhaPhanPhoiFilterInput> input = null)
        {
            var filtered = QueryFilterDto(input.Filter);
            var totalCount = await filtered.CountAsync();
            filtered = filtered.OrderByDescending(p => p.Id);
            if (input.SkipCount > 0)
            {
                filtered = filtered.Skip(input.SkipCount);
            }
            if (input.MaxResultCount > 0)
            {
                filtered = filtered.Take(input.MaxResultCount);
            }
            var listData = await filtered.ToListAsync();
            return new PageResultDTO<NhaPhanPhoi_DTO>(totalCount, listData);
        }

        #region Query
        public IQueryable<Model.NhaPhanPhoi> QueryFilter(NhaPhanPhoiFilterInput input = null)
        {
            var query = _db.NhaPhanPhois.AsQueryable();
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.TenNhaPhanPhoi))
                {
                    var lower = input.TenNhaPhanPhoi.Trim().ToLower();
                    query = query.Where(p => p.TenNhaPhanPhoi.ToLower().Contains(lower));

                }
                if (!string.IsNullOrEmpty(input.MaNhaPhanPhoi))
                {
                    var lower = input.MaNhaPhanPhoi.Trim().ToLower();
                    query = query.Where(p => p.ID.ToLower().Contains(lower));

                }
                if (!string.IsNullOrEmpty(input.DiaChi))
                {
                    var lower = input.DiaChi.Trim().ToLower();
                    query = query.Where(p => p.DiaChi.ToLower().Contains(lower));
                }
            }
            return query;
        }

        public IQueryable<NhaPhanPhoi_DTO> QueryFilterDto(NhaPhanPhoiFilterInput input = null)
        {
            try
            {
                var query = from q in QueryFilter(input)
                            select new NhaPhanPhoi_DTO
                            {
                                TenNhaPhanPhoi = q.TenNhaPhanPhoi,
                                DiaChi = q.DiaChi,
                                SoDienThoai = q.SoDienThoai,
                                Id = q.ID
                            };
                return query;
            }
            catch
            {
                throw new Exception("Lỗi chỗ QueryFilter DTO");
            }
        }
        #endregion
        private async Task<Model.NhaPhanPhoi> MapperCreateInputToEntity(NhaPhanPhoiCreateInput input, Model.NhaPhanPhoi entity)
        {
            await Task.Run(() =>
            {
                entity.ID = input.NhaPhanPhoiId;
                entity.TenNhaPhanPhoi = input.TenNhaPhanPhoi;
                entity.DiaChi = input.DiaChi;
                entity.SoDienThoai = input.SoDienThoai;
            });
            return entity;
        }
    }
}
