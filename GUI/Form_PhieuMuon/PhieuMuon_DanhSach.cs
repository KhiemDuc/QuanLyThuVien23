using DAL.Common;
using DAL.Services.PhieuMuons;
using DAL.Services.PhieuMuons.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Form_PhieuMuon
{
    public partial class PhieuMuon_DanhSach : Form
    {
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        PhieuMuon_BLL PhieuMuon_BLL = new PhieuMuon_BLL();
        PhieuMuonFilterInput PhieuMuon_Filter = new PhieuMuonFilterInput();
        public PhieuMuon_DanhSach()
        {
            InitializeComponent();
        }

        private void PhieuMuon_DanhSach_Load(object sender, EventArgs e)
        {
            showDuLieuPM().ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    MessageBox.Show("Lỗi show dữ liệu nhân viên ban đầu");
                }
            });
        }

        private async Task showDuLieuPM()
        {
            var pageResultDTO = await PhieuMuon_BLL.LayDanhSachPhieuMuon(pageNumber, pageSize, PhieuMuon_Filter);
            var listPhieuMuon = pageResultDTO.Items.ToList();
            totalCount = pageResultDTO.TotalCount;
            maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
            dtgPhieuMuon.Rows.Clear();
            foreach (var pm in listPhieuMuon)
            {
                int rowIndex = dtgPhieuMuon.Rows.Add();
                dtgPhieuMuon.Rows[rowIndex].Cells["ID"].Value = pm.PhieuMuonId;
                dtgPhieuMuon.Rows[rowIndex].Cells["TenPhieuMuon"].Value = pm.TenPhieuMuon;
                dtgPhieuMuon.Rows[rowIndex].Cells["TenDocGia"].Value = pm.TenDocGia;
                dtgPhieuMuon.Rows[rowIndex].Cells["TenNhanVien"].Value = pm.TenNhanVien;
                dtgPhieuMuon.Rows[rowIndex].Cells["TrangThai"].Value = pm.TenTrangThai;
                dtgPhieuMuon.Rows[rowIndex].Cells["NgayMuon"].Value = pm.NgayMuon != DateTime.MinValue ? pm.NgayMuon.ToString("dd/MM/yyyy") : "string.Empty";
                dtgPhieuMuon.Rows[rowIndex].Cells["NgayHenTra"].Value = pm.NgayTra != DateTime.MinValue ? pm    .NgayTra.ToString("dd/MM/yyyy") : "string.Empty";
            }
            if (pageNumber <= 0)
            {
                pageNumber = 0;
                btnTruoc.Enabled = false;
                btnTrangDau.Enabled = false;
            }
            else
            {
                btnTruoc.Enabled = true;
                btnTrangDau.Enabled = true;
            }

            if (pageNumber + 1 >= maxPage)
            {
                pageNumber = maxPage - 1;
                btnSau.Enabled = false;
                btnTrangCuoi.Enabled = false;
            }
            else
            {
                btnTrangCuoi.Enabled = true;
                btnSau.Enabled = true;
            }
            txtTrang.Text = (pageNumber + 1).ToString() + "/" + maxPage.ToString();
        }
    }
    public class PhieuMuon_BLL
    {
        private IPhieuMuonService iPhieuMuonService;
        public PhieuMuon_BLL()
        {
            iPhieuMuonService = new PhieuMuonService();
        }
        public async Task<PageResultDTO<PhieuMuon_DTO>> LayDanhSachPhieuMuon(int pageNumber, int pageSize, PhieuMuonFilterInput input = null)
        {
            var pageInput = new PagingInput<PhieuMuonFilterInput>(input)
            {
                SkipCount = pageSize * pageNumber,
                MaxResultCount = pageSize,
            };
            return await iPhieuMuonService.Paging(pageInput);
        }
        public async Task<bool> xoaPhieuMuonById(string Id)
        {
            var xoaNVTheoId = await iPhieuMuonService.DeletePhieuMuonById(Id);
            return xoaNVTheoId;
        }
        public async Task<string> themPhieuMuon(PhieuMuonCreateInput input)
        {
            var themPm = await iPhieuMuonService.CreatePhieuMuon(input);
            return themPm;
        }
    }

}

