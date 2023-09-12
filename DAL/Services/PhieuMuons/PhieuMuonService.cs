using DAL.Common;
using DAL.Model;
using DAL.QLTVExceptions;
using DAL.Services.PhieuMuon_Sachs;
using DAL.Services.PhieuMuons.DTO;
using DAL.Services.Sachs.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.PhieuMuons
{
    public class PhieuMuonService : IPhieuMuonService
    {
        #region Khai báo
        public readonly QuanLyThuVienEntities _db;

        public PhieuMuonService()
        {
            _db = new QuanLyThuVienEntities();
        }
        #endregion

        #region QueryFilter and Paging
        public IQueryable<PhieuMuon> QueryFilter(PhieuMuonFilterInput input = null)
        {
            var query = _db.PhieuMuons.AsQueryable();
            if (input != null)
            {
                if (!string.IsNullOrEmpty(input.PhieuMuonId))
                {
                    var lower = input.PhieuMuonId.Trim().ToLower();
                    query = query.Where(p => p.ID.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.TenPhieuMuon))
                {
                    var lower = input.TenPhieuMuon.Trim().ToLower();
                    query = query.Where(p => p.TenPhieuMuon.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.TenDocGia))
                {
                    var lower = input.TenDocGia.Trim().ToLower();
                    query = query.Where(p => p.TenPhieuMuon.ToLower().Contains(lower));
                }
                if (!string.IsNullOrEmpty(input.TenNhanVien))
                {
                    var lower = input.TenNhanVien.Trim().ToLower();
                    query = query.Where(p => p.TenPhieuMuon.ToLower().Contains(lower));
                }
                if (input.TrangThaiId != 0 && input.TrangThaiId != null)
                {
                    query = query.Where(p => p.ID_TrangThai == input.TrangThaiId);
                }
                if (!string.IsNullOrEmpty(input.TenTrangThai))
                {
                    var lower = input.TenTrangThai.Trim().ToLower();
                    query = query.Where(p => p.TrangThai_PhieuMuon.TenTrangThai.ToLower().Contains(lower));
                }
                if (input.NgayMuon1.HasValue || input.NgayMuon2.HasValue)
                {
                    if (input.NgayMuon1.HasValue && input.NgayMuon2.HasValue)
                    {
                        query = query.Where(p => p.NgayMuon >= input.NgayMuon1.Value && p.NgayMuon <= input.NgayMuon2.Value);
                    }
                    else if (input.NgayMuon1.HasValue)
                    {
                        query = query.Where(p => p.NgayMuon >= input.NgayMuon1.Value);
                    }
                    else if (input.NgayMuon2.HasValue)
                    {
                        query = query.Where(p => p.NgayMuon <= input.NgayMuon2.Value);
                    }
                }
            }
            return query;
        }

        public IQueryable<PhieuMuon_DTO> QueryFilterDto(PhieuMuonFilterInput input = null)
        {
            try
            {
                var query = from q in QueryFilter(input).Include(p => p.PhieuMuon_Sachs)
                            select new PhieuMuon_DTO
                            {
                                PhieuMuonId = q.ID,
                                TenPhieuMuon = q.TenPhieuMuon,
                                TenNhanVien = q.NhanVien.TenNhanVien,
                                TenDocGia = q.DocGia.TenDocGia,
                                TenTrangThai = q.TrangThai_PhieuMuon.TenTrangThai,
                                TrangThaiId = q.TrangThai_PhieuMuon.ID,
                                listSachMuon = q.PhieuMuon_Sachs.ToList(),
                                NgayMuon = q.NgayMuon ?? DateTime.MinValue,
                                NgayTra = q.NgayHenTra ?? DateTime.MinValue,
                            };
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi ở chỗ queryDTO\n" + ex.Message);
            }
        }
        public async Task<PageResultDTO<PhieuMuon_DTO>> Paging(PagingInput<PhieuMuonFilterInput> input = null)
        {
            var filtered = QueryFilterDto(input.Filter);
            var totalCount = await filtered.CountAsync();
            filtered = filtered.OrderByDescending(p => p.PhieuMuonId);
            if (input.SkipCount > 0)
            {
                filtered = filtered.Skip(input.SkipCount);
            }
            if (input.MaxResultCount > 0)
            {
                filtered = filtered.Take(input.MaxResultCount);
            }
            var listData = await filtered.ToListAsync();
            return new PageResultDTO<PhieuMuon_DTO>(totalCount, listData);
        }

        #endregion

        #region crud
        public async Task<PhieuMuon> GetById(string id)
        {
            return await QueryFilter().FirstOrDefaultAsync(p => p.ID == id) ?? throw new Exception($"Không tìm thấy Phiếu Mượn có id {id}.");
        }

        public async Task<PhieuMuon_DTO> GetByIdDto(string id)
        {
            return await QueryFilterDto().FirstOrDefaultAsync(p => p.PhieuMuonId == id) ?? throw new Exception($"Không tìm thấy sách id {id}.");
        }
        public async Task<string> CreatePhieuMuon(PhieuMuonCreateInput input)
        { 
            var entity = await MapperCreateInputToEntity(input, new PhieuMuon());
            //Check số lượng sách mượn trước
            foreach(var item in entity.PhieuMuon_Sachs)
            {
                var sachThayDoi = await _db.Saches.FirstOrDefaultAsync(x => item.ID_Sach == x.ID);
                if (item.SoLuong <= sachThayDoi.SoLuong)
                {
                    sachThayDoi.SoLuong -= item.SoLuong;
                }
                else throw new PhieuMuon_SachException($"Cuốn '{sachThayDoi.TenSach}' không đủ số lượng yêu cầu! Không thể tạo phiếu mượn!");
            }
            _db.PhieuMuons.Add(entity);
            await _db.SaveChangesAsync();
            return entity.ID;
        }

        public async Task<bool> DeletePhieuMuonById(string Id)
        {
            var entity = await GetById(Id);
            return false;
        }

        public async Task<bool> UpdatePhieuMuon(string Id, PhieuMuonCreateInput input)
        {
            var entity = await QueryFilter().FirstOrDefaultAsync(x => x.ID == Id);
            entity = await MapperCreateInputToEntity(input, entity);
            return true;
        }




        public async Task<PhieuMuon> MapperCreateInputToEntity(PhieuMuonCreateInput input, PhieuMuon entity)
        {
            await Task.Run(() =>
            {
                entity.ID = input.PhieuMuonId;
                entity.TenPhieuMuon = input.TenPhieuMuon;
                entity.ID_NhanVien = input.NhanVienId;
                entity.ID_DocGia = input.DocGiaId;
                entity.NgayMuon = input.NgayMuon;
                entity.NgayHenTra = input.NgayTra;
                entity.GhiChu = input.GhiChu;
                entity.ID_TrangThai = input.TrangThaiId;
                entity.PhieuMuon_Sachs = new List<Model.PhieuMuon_Sachs>();
                foreach (var item in input.ListSachMuon)
                {
                    entity.PhieuMuon_Sachs.Add(new Model.PhieuMuon_Sachs
                    {
                        ID_PhieuMuon = item.PhieuMuonId,
                        ID_Sach = item.SachId,
                        SoLuong = item.SoLuong
                    });
                }
            });
            return entity;
        }

        #endregion
        private async Task<bool> CheckValidCreateOrUpdate(PhieuMuonCreateInput input)
        {
            var listSachId = input.ListSachMuon.Select(p => p.SachId).ToList();
            var listSachs = await _db.Saches.Where(p => listSachId.Contains(p.ID)).ToListAsync();

            // Gộp số lượng sách theo sachid, trong trường hợp mượn 1 sách mà nhiều lần
            var listSachMuon = input.ListSachMuon.GroupBy(p => p.SachId)
                .Select(p => new PhieuMuon_SachCreateInput
                {
                    SachId = p.Key,
                    SoLuong = p.Sum(a => a.SoLuong)
                }).ToList();
            // ktra xem tổng số lượng sách mượn có lớn hơn số sách trong kho hay ko
            foreach (var sachMuon in listSachMuon)
            {
                var sach = listSachs.FirstOrDefault(p => p.ID == sachMuon.SachId);
                if (sach == null)
                {
                    throw new Exception($"Khong tin thay sach id {sachMuon.SachId}");
                }
                if (sach.SoLuong > sachMuon.SoLuong)
                {
                    throw new Exception($"Sách {sach.TenSach}, số lượng sách mượn ({sachMuon.SoLuong}) không thể nhiều hơn số lượng sách hiện có ({sach.SoLuong})");
                }
            }
            input.ListSachMuon = listSachMuon;
            
            return true;
        }

        //private async Task<int> CreateOrUpdate(PhieuMuonCreateOrUpdateInput input, int Id = 0)
        //{
        //    var entity = new PhieuMuon();
        //    if (Id > 0)
        //    {
        //        entity = _db.PhieuMuons.FirstOrDefault(p => p.ID == Id);
        //        if (entity == null)
        //        {
        //            throw new Exception($"Khong tin thay phieu muon co id {Id}");
        //        }

        //        // xoa ds sach muonj cux
        //        var listSacMuon = await _db.PhieuMuon_Sach.Where(p => p.ID_Sach == entity.ID).ToListAsync();
        //        if (listSacMuon?.Any() == true)
        //        {
        //            _db.PhieuMuon_Sach.RemoveRange(listSacMuon);
        //        }
        //    }
        //    else
        //    {
        //        _db.PhieuMuons.Add(entity);
        //    }
        //    // cap nhat du lieu
        //    entity.ID_NhanVien = input.NhanVienId;
        //    entity.ID_DocGia = input.DocGiaId;
        //    entity.NgayMuon = input.NgayMuon;
        //    entity.NgayHenTra = input.NgayTra;
        //    entity.TenPhieuMuon = input.TenPhieuMuon;
        //    entity.GhiChu = input.GhiChu;
        //    // them ds muon mowis
        //    var listSachMuon = input.ListSachMuon.Select(p => new PhieuMuon_Sach
        //    {
        //        ID_PhieuMuon = entity.ID,
        //        ID_Sach = p.SachId,
        //        SoLuong = p.SoLuong,
        //    }).ToList();
        //    entity.PhieuMuon_Sach = listSachMuon;

        //    await _db.SaveChangesAsync();
        //    return entity.ID;
        //}

    }
}
