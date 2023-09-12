using DAL.Common;
using DAL.Model;
using DAL.Services.Sachs.DTO;
using DAL.Services.TacGias.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.TacGias
{
    public class TacGiaService : ITacGiaService
    {
        public readonly QuanLyThuVienEntities _db;
        public TacGiaService()
        {
            _db = new QuanLyThuVienEntities();
        }

        #region Crud
        public async Task<int> CreateTacGia(TacGiaCreateInput input)
        {
            var entity = await MapperCreateInputToEntity(input, new Model.TacGia());
            _db.TacGias.Add(entity);
            return _db.SaveChanges();
        }

        public async Task<bool> UpdateTacGia(int TacGiaId, TacGiaCreateInput input)
        {
            var entity = await GetById(TacGiaId);
            entity = await MapperCreateInputToEntity(input, entity);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteTacGiaById(int TacGiaId)
        {
            var entity = await GetById(TacGiaId);
            if (entity != null)
            {
                var sachLienKet = _db.Saches.Where(s => s.ID_TacGia == TacGiaId).ToList();
                foreach (var s in sachLienKet)
                {
                    s.TacGia = null;
                }
                await _db.SaveChangesAsync();
                _db.TacGias.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Model.TacGia> GetById(int id)
        {
            return await QueryFilter().FirstOrDefaultAsync(p => p.ID == id) ?? throw new Exception($"Không tìm thấy nhân viên có id {id}.");
        }

        public async Task<TacGia_DTO> GetByIdDto(int id)
        {
            return await QueryFilterDto().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception($"Không tìm thấy nhân viên có id {id}.");
        }

        #endregion

       
        public async Task<PageResultDTO<TacGia_DTO>> Paging(PagingInput<TacGiaFilterInput> input = null)
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
            return new PageResultDTO<TacGia_DTO>(totalCount, listData);
        }

        public IQueryable<Model.TacGia> QueryFilter(TacGiaFilterInput input = null)
        {
            var query = _db.TacGias.AsQueryable();
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.TenTacGia))
                {
                    var lower = input.TenTacGia.Trim().ToLower();
                    query = query.Where(p => p.TenTacGia.ToLower().Contains(lower));

                }
                if (!string.IsNullOrEmpty(input.DiaChi))
                {
                    var lower = input.DiaChi.Trim().ToLower();
                    query = query.Where(p => p.DiaChi.ToLower().Contains(lower));
                }
                if (input.NamSinh.HasValue)
                {
                    var namSinhYear = input.NamSinh.Value;
                    query = query.Where(p => p.NamSinh != null && p.NamSinh.Value.Year == namSinhYear);
                }

                if (input.NamMat.HasValue)
                {
                    var namMatYear = input.NamMat.Value;
                    query = query.Where(p => p.NamMat != null && p.NamMat.Value.Year == namMatYear);
                }
            }
            return query;
        }

        public IQueryable<TacGia_DTO> QueryFilterDto(TacGiaFilterInput input = null)
        {
            try
            {
                var query = from q in QueryFilter(input)
                            select new TacGia_DTO
                            {
                                TenTacGia = q.TenTacGia,
                                DiaChi = q.DiaChi,
                                SoDienThoai = q.SoDienThoai,
                                Id = q.ID,
                                NamSinh = q.NamSinh,
                                NamMat = q.NamMat,
                                MoTa = q.MoTaThem,
                                AnhTacGia = q.AnhTacGia                                
                            };
                return query;
            }
            catch
            {
                throw new Exception("Lỗi chỗ QueryFilter DTO");
            }
        }
        private async Task<Model.TacGia> MapperCreateInputToEntity(TacGiaCreateInput input, Model.TacGia entity)
        {
            await Task.Run(() =>
            {
                entity.TenTacGia = input.TenTacGia;
                entity.DiaChi = input.DiaChi;
                entity.SoDienThoai = input.SoDienThoai;
                entity.NamSinh = input.NamSinh;
                entity.NamMat = input.NamMat;
                entity.AnhTacGia = input.AnhTacGia;
                entity.MoTaThem = input.MoTa;
            });
            return entity;
        }

        public async Task<List<string>> getAllNameTacGia(TacGiaFilterInput input)
        {
            var listNameTacGia = await QueryFilter(input).Select(s => s.TenTacGia).ToListAsync();
            return listNameTacGia ?? throw new Exception("Không lấy ra được tên tác giả");
        }
        public int SoLuongTacGia()
        {
            return _db.TacGias.Count();
        }
    }
}
