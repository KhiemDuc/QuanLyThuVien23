using DAL.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Services.TheLoais;
using DAL.Services.TheLoais.DTO;

namespace GUI.Form_TheLoai
{
    public partial class TheLoai_DanhSach : Form
    {
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        TheLoai_BLL TheLoai_BLL = new TheLoai_BLL(new TheLoaiService());
        TheLoaiFilterInput TheLoai_Filter = new TheLoaiFilterInput();
        public TheLoai_DanhSach()
        {
            InitializeComponent();
        }

        private void TheLoai_DanhSach_Load(object sender, EventArgs e)
        {
            showDuLieuNV().ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    MessageBox.Show("Lỗi show dữ liệu nhân viên ban đầu");
                }
            });
        }

        private async Task showDuLieuNV()
        {
            var pageResultDTO = await TheLoai_BLL.LayDanhSachNV(pageNumber, pageSize, TheLoai_Filter);
            var listTheLoai = pageResultDTO.Items.ToList();
            totalCount = pageResultDTO.TotalCount;
            maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
            dtgTheLoai.Rows.Clear();
            foreach (var nv in listTheLoai)
            {
                int rowIndex = dtgTheLoai.Rows.Add();
                dtgTheLoai.Rows[rowIndex].Cells["ID"].Value = nv.Id;
                dtgTheLoai.Rows[rowIndex].Cells["TenTheLoai"].Value = nv.TenTheLoai;
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

    public class TheLoai_BLL
    {
        private ITheLoaiService iTheLoaiService;
        public TheLoai_BLL(ITheLoaiService sv)
        {
            iTheLoaiService = sv;
        }
        public async Task<PageResultDTO<TheLoai_DTO>> LayDanhSachNV(int pageNumber, int pageSize, TheLoaiFilterInput input = null)
        {
            var pageInput = new PagingInput<TheLoaiFilterInput>(input)
            {
                SkipCount = pageSize * pageNumber,
                MaxResultCount = pageSize,
            };
            return await iTheLoaiService.Paging(pageInput);
        }
        public async Task<bool> xoaNVTheoId(string Id)
        {
            var xoaNVTheoId = await iTheLoaiService.DeleteTheLoaiById(Id);
            return xoaNVTheoId;
        }
    }
}

