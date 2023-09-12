
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Common;
using DAL.Services.NhaPhanPhois;
using DAL.Services.NhaPhanPhois.DTO;
using GUI.Form_DocGia;

namespace GUI.Form_NhaPhanPhoi
{
    public partial class NhaPhanPhoi_DanhSach : Form
    {
        #region Khai Báo
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        NhaPhanPhoi_BLL NhaPhanPhoi_BLL = new NhaPhanPhoi_BLL(new NhaPhanPhoiService());
        NhaPhanPhoiFilterInput NhaPhanPhoi_Filter = new NhaPhanPhoiFilterInput();
        #endregion
        public NhaPhanPhoi_DanhSach()
        {
            InitializeComponent();
        }
        public void NhaPhanPhoi_DanhSach_Load(object sender, EventArgs e)
        {
            showDuLieuNhaPhanPhoi().ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    MessageBox.Show("Lỗi show dữ liệu nhân viên ban đầu");
                }
            });
        }

        private async Task showDuLieuNhaPhanPhoi()
        {
            var pageResultDTO = await NhaPhanPhoi_BLL.LayDanhSachNPP(pageNumber, pageSize, NhaPhanPhoi_Filter);
            var listNhaPhanPhoi = pageResultDTO.Items.ToList();
            totalCount = pageResultDTO.TotalCount;
            maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
            dtgNhaPhanPhoi.Rows.Clear();
            foreach (var nv in listNhaPhanPhoi)
            {
                int rowIndex = dtgNhaPhanPhoi.Rows.Add();
                dtgNhaPhanPhoi.Rows[rowIndex].Cells["ID"].Value = nv.Id;
                dtgNhaPhanPhoi.Rows[rowIndex].Cells["TenNhaPhanPhoi"].Value = nv.TenNhaPhanPhoi;
                dtgNhaPhanPhoi.Rows[rowIndex].Cells["DiaChi"].Value = nv.DiaChi != null ? nv.DiaChi : string.Empty;
                dtgNhaPhanPhoi.Rows[rowIndex].Cells["SoDienThoai"].Value = nv.SoDienThoai != null ? nv.SoDienThoai : string.Empty;
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

        private async void dtgNhaPhanPhoi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string MaNhaPhanPhoi = dtgNhaPhanPhoi.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                if (e.ColumnIndex == dtgNhaPhanPhoi.Columns["Xoa"].Index && e.RowIndex >= 0)
                {

                    DialogResult result = MessageBox.Show("Bạn có muốn xóa toàn bộ thông tin nhà phân phối này khỏi cơ sở dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var skXoa = await NhaPhanPhoi_BLL.xoaNPPTheoId(MaNhaPhanPhoi);
                        if (skXoa)
                            await showDuLieuNhaPhanPhoi();
                        else
                            throw new Exception("Lỗi xóa nhà phân phối");
                    }
                    else if (result == DialogResult.No)
                    {
                        Console.WriteLine("Bạn đã chọn 'Không'");
                    }
                }
                if (e.ColumnIndex == dtgNhaPhanPhoi.Columns["ChiTiet"].Index && e.RowIndex >= 0)
                {
                    var nhaPhanPhoiCapNhat = new NhaPhanPhoiCreateOrUpdate(MaNhaPhanPhoi);
                    nhaPhanPhoiCapNhat.FormClosed += childFormClose;
                    nhaPhanPhoiCapNhat.Show(this);
                }
            }
        }

        private void childFormClose(object sender, FormClosedEventArgs e)
        {
            NhaPhanPhoi_DanhSach_Load(null, null);
        }

        private async void btnTim_ClickAsync(object sender, EventArgs e)
        {
            NhaPhanPhoi_Filter.TenNhaPhanPhoi = txtTenNhaPhanPhoi.Text;
            NhaPhanPhoi_Filter.MaNhaPhanPhoi = txtMaNhaPhanPhoi.Text;
            NhaPhanPhoi_Filter.DiaChi = txtDiaChi.Text;
            await showDuLieuNhaPhanPhoi();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var themNhaPhanPhoi = new NhaPhanPhoiCreateOrUpdate();
            themNhaPhanPhoi.FormClosed += childFormClose;
            themNhaPhanPhoi.Show(this);
        }
    }
    public class NhaPhanPhoi_BLL
    {
        private INhaPhanPhoiService iNhaPhanPhoiService;
        public NhaPhanPhoi_BLL(INhaPhanPhoiService sv)
        {
            iNhaPhanPhoiService = sv;
        }
        public async Task<PageResultDTO<NhaPhanPhoi_DTO>> LayDanhSachNPP(int pageNumber, int pageSize, NhaPhanPhoiFilterInput input = null)
        {
            var pageInput = new PagingInput<NhaPhanPhoiFilterInput>(input)
            {
                SkipCount = pageSize * pageNumber,
                MaxResultCount = pageSize,
            };
            return await iNhaPhanPhoiService.Paging(pageInput);
        }
        public async Task<bool> xoaNPPTheoId(string Id)
        {
            var xoaNVTheoId = await iNhaPhanPhoiService.DeleteNhaPhanPhoiById(Id);
            return xoaNVTheoId;
        }
    }
}
