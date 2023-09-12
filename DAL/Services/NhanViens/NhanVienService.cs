using DAL.Common;
using DAL.Model;
using DAL.Services.NhanVien.DTO;
using DAL.Services.Sachs.DTO;
using DAL.Services.TacGias.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.NhanVien
{
    public class NhanVienService : INhanVienService
    {
        public readonly QuanLyThuVienEntities  _db ;
        public NhanVienService()
        {
            _db = new QuanLyThuVienEntities();
        }

        #region Crud
        public async Task<int> CreateNhanVien(NhanVienCreateInput input)
        {
            var entity = await MapperCreateInputToEntity(input, new Model.NhanVien());
            _db.NhanViens.Add(entity);
            return _db.SaveChanges();
        }

        public async Task<bool> UpdateNhanVien(int NhanVienId, NhanVienCreateInput input)
        {
            var entity = await GetById(NhanVienId);
            entity = await MapperCreateInputToEntity(input, entity);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteNhanVienById(int NhanvienId)
        {
            var entity = await GetById(NhanvienId);
            if(entity != null)
            {
                _db.NhanViens.Remove(entity);
                 await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Model.NhanVien> GetById(int id)
        {
            return await QueryFilter().FirstOrDefaultAsync(p => p.ID == id) ?? throw new Exception($"Không tìm thấy nhân viên có id {id}.");
        }
        public async Task<NhanVien_DTO> GetByIdDto(int id)
        {
            return await QueryFilterDto().FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception($"Không tìm thấy nhân viên có id {id}.");
        }
        #endregion
        public async Task<PageResultDTO<NhanVien_DTO>> Paging(PagingInput<NhanVienFilterInput> input = null)
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
            return new PageResultDTO<NhanVien_DTO>(totalCount, listData);
        }
        public IQueryable<Model.NhanVien> QueryFilter(NhanVienFilterInput input = null)
        {
            var query = _db.NhanViens.AsQueryable();
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.TenNhanVien))
                {
                    var lower = input.TenNhanVien.Trim().ToLower();
                    query = query.Where(p => p.TenNhanVien.ToLower().Contains(lower));

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
                if (!string.IsNullOrEmpty(input.CCCD))
                {
                    var lower = input.CCCD.Trim().ToLower();
                    query = query.Where(p => p.CCCD.ToLower().Contains(lower));
                }
                if (input.TenVaiTro != null && input != null)
                {
                    query = query.Where(p => p.VaiTro.TenRole == input.TenVaiTro);
                }
            }
            return query;
        }
        public IQueryable<NhanVien_DTO> QueryFilterDto(NhanVienFilterInput input = null)
        {
            try
            {
                var query = from q in QueryFilter(input)
                            select new NhanVien_DTO
                            {
                                TenNhanVien = q.TenNhanVien,
                                DiaChi = q.DiaChi,
                                SoDienThoai = q.SoDienThoai,
                                CCCD = q.CCCD,
                                TaiKhoan = q.TaiKhoan,
                                MatKhau = q.MatKhau,
                                VaiTro = q.VaiTro.ID,
                                Id = q.ID,
                                NgaySinh = q.NgaySinh,
                                GioiTinh = q.GioiTinh,
                                NgayVaoLam = q.NgayVaoLam,
                                AnhNhanVien = q.AnhDaiDien
                            };
                return query;
            }
            catch
            {
                throw new Exception("Lỗi chỗ QueryFilter DTO");
            }
        }
        private async Task<Model.NhanVien> MapperCreateInputToEntity(NhanVienCreateInput input, Model.NhanVien entity)
        {
            await Task.Run(() =>
            {
                entity.TenNhanVien = input.TenNhanVien;
                entity.DiaChi = input.DiaChi;
                entity.CCCD = input.CCCD;
                entity.SoDienThoai = input.SoDienThoai;
                entity.TaiKhoan = input.TaiKhoan;
                entity.MatKhau = input.MatKhau;
                entity.ID_Role = input.VaiTroId;
                entity.GioiTinh = input.GioiTinh;
                entity.NgaySinh = input.NgaySinh;
                entity.NgayVaoLam = input.NgayVaoLam;
                entity.AnhDaiDien = input.AnhNhanVien;
            });

            return entity;
        }
        private async Task<List<string>> getAllNameNhanVien(NhanVienFilterInput input = null)
        {
            var listNameNhanVien = await QueryFilter(input).Select(s => s.TenNhanVien).ToListAsync();
            return listNameNhanVien ?? throw new Exception("Không lấy ra được tên nhân viên");
        }
    }
}
