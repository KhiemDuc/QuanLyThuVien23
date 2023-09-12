using DAL.Common;
using DAL.Model;
using DAL.Services.DocGias.DTO;
using DAL.Services.NhanVien.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.DocGias
{
    public class DocGiaService : IDocGiaService
    {
        public readonly QuanLyThuVienEntities _db;
        
        public DocGiaService()
        {
            _db = new QuanLyThuVienEntities();
        }

        #region Crud
        public async Task<int> CreateDocGia(DocGiaCreateInput input)
        {
            var entity = await MapperCreateInputToEntity(input, new Model.DocGia());
            _db.DocGias.Add(entity);
            return _db.SaveChanges();
        }

        public async Task<bool> UpdateDocGia(int DocGiaId, DocGiaCreateInput input)
        {
            var entity = await GetById(DocGiaId);
            entity = await MapperCreateInputToEntity(input, entity);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteDocGiaById(int DocGiaId)
        {
            var entity = await GetById(DocGiaId);
            if (entity != null)
            {
                _db.DocGias.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Model.DocGia> GetById(int id)
        {
            return await QueryFilter().FirstOrDefaultAsync(p => p.ID == id) ?? throw new Exception($"Không tìm thấy độc giả có id {id}.");
        }

        public async Task<DocGia_DTO> GetByIdDto(int id)
        {
            return await QueryFilterDto().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception($"Không tìm thấy độc giả có id {id}.");
        }

        #endregion

        public async Task<PageResultDTO<DocGia_DTO>> Paging(PagingInput<DocGiaFilterInput> input = null)
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
            return new PageResultDTO<DocGia_DTO>(totalCount, listData);
        }
        #region query
        public IQueryable<Model.DocGia> QueryFilter(DocGiaFilterInput input = null)
        {
            var query = _db.DocGias.AsQueryable();
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.TenDocGia))
                {
                    var lower = input.TenDocGia.Trim().ToLower();
                    query = query.Where(p => p.TenDocGia.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.CCCD))
                {
                    var lower = input.CCCD.Trim().ToLower();
                    query = query.Where(p => p.CCCD.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.DiaChi))
                {
                    var lower = input.DiaChi.Trim().ToLower();
                    query = query.Where(p => p.DiaChi.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.SoDienThoai))
                {
                    var lower = input.SoDienThoai.Trim().ToLower();
                    query = query.Where(p => p.SoDienThoai.ToLower().Contains(lower));
                }
            }
            return query;
        }
        public IQueryable<DocGia_DTO> QueryFilterDto(DocGiaFilterInput input = null)
        {
            try
            {
                var query = from q in QueryFilter(input)
                            select new DocGia_DTO
                            {
                                TenDocGia = q.TenDocGia,
                                DiaChi = q.DiaChi,
                                SoDienThoai = q.SoDienThoai,
                                CCCD = q.CCCD,
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
        private async Task<Model.DocGia> MapperCreateInputToEntity(DocGiaCreateInput input, Model.DocGia entity)
        {
            await Task.Run(() =>
            {
                entity.TenDocGia = input.TenDocGia;
                entity.DiaChi = input.DiaChi;
                entity.CCCD = input.CCCD;
                entity.SoDienThoai = input.SoDienThoai;
                entity.AnhSinhVien = input.AnhDocGia;
            });
            return entity;
        }
    }
}
