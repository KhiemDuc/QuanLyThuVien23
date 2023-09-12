using DAL.Common;
using DAL.Services.TacGias.DTO;
using DAL.Services.TacGias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUI.Form_Sach;

namespace GUI.Form_TacGia
{
    public partial class TacGia_DanhSach : Form
    {
        #region Khai báo 
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        TacGia_BLL TacGia_BLL = new TacGia_BLL(new TacGiaService());
        TacGiaFilterInput TacGia_Filter = new TacGiaFilterInput();
        #endregion
        public TacGia_DanhSach()
        {
            InitializeComponent();
        }

        public void TacGia_DanhSach_Load(object sender, EventArgs e)
        {
            showDuLieuTG().ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    MessageBox.Show("Lỗi show dữ liệu nhân viên ban đầu");
                }
            });
        }

        private async Task showDuLieuTG()
        {
            var pageResultDTO = await TacGia_BLL.LayDanhSachTG(pageNumber, pageSize, TacGia_Filter);
            var listTacGia = pageResultDTO.Items.ToList();
            totalCount = pageResultDTO.TotalCount;
            maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
            dtgTacGia.Rows.Clear();
            foreach (var tg in listTacGia)
            {
                var test = tg;
                int rowIndex = dtgTacGia.Rows.Add();
                dtgTacGia.Rows[rowIndex].Cells["ID"].Value = tg.Id;
                dtgTacGia.Rows[rowIndex].Cells["TenTacGia"].Value = tg.TenTacGia;
                dtgTacGia.Rows[rowIndex].Cells["DiaChi"].Value = tg.DiaChi != null ? tg.DiaChi : string.Empty;
                dtgTacGia.Rows[rowIndex].Cells["SoDienThoai"].Value = tg.SoDienThoai != null ? tg.SoDienThoai : string.Empty;
                dtgTacGia.Rows[rowIndex].Cells["NamSinh"].Value =   tg.NamSinh?.ToString("dd/MM/yyyy") ?? string.Empty;
                dtgTacGia.Rows[rowIndex].Cells["NamMat"].Value = tg.NamMat?.ToString("dd/MM/yyyy") ?? string.Empty;
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

        #region sự kiện
        private void btnThem_Click(object sender, EventArgs e)
        {
            var tacGiaCrud = new TacGiaCreateOrUpdate();
            tacGiaCrud.Show(this);
            tacGiaCrud.Owner = this;
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }
        #endregion
        #region cellcontentClick
        private async void dtgTacGia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string MaTacGia = dtgTacGia.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                if (e.ColumnIndex == dtgTacGia.Columns["Xoa"].Index && e.RowIndex >= 0)
                {

                    DialogResult result = MessageBox.Show("Bạn có muốn xóa tác giả này khỏi cơ sở dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var skXoa = await TacGia_BLL.xoaTacGiaById(Int32.Parse(MaTacGia));
                        if (skXoa)
                            await showDuLieuTG();
                        else
                            throw new Exception("Lỗi xóa tác giả");
                    }
                    else if (result == DialogResult.No)
                    {
                        Console.WriteLine("Bạn đã chọn 'Không'");
                    }
                }
                if (e.ColumnIndex == dtgTacGia.Columns["ChiTiet"].Index && e.RowIndex >= 0)
                {
                    var tacGiaCrud = new TacGiaCreateOrUpdate(Int32.Parse(MaTacGia));
                    tacGiaCrud.Show(this);
                }

            }
        }
        #endregion
        #region button Điều hướng

        private void btnTrangDau_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            showDuLieuTG().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ trở về trang đầu tiên"); } });
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            pageNumber--;
            showDuLieuTG().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ trở về trước"); } });
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            pageNumber++;
            showDuLieuTG().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ next trang"); } });
        }

        private void btnTrangCuoi_Click(object sender, EventArgs e)
        {
            pageNumber = maxPage - 1;
            showDuLieuTG().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ đến trang cuối"); } });
        }
        #endregion

       
    }
    public class TacGia_BLL
    {
        private ITacGiaService iTacGiaService;
        public TacGia_BLL(ITacGiaService sv)
        {
            iTacGiaService = sv;
        }
        public async Task<PageResultDTO<TacGia_DTO>> LayDanhSachTG(int pageNumber, int pageSize, TacGiaFilterInput input = null)
        {
            var pageInput = new PagingInput<TacGiaFilterInput>(input)
            {
                SkipCount = pageSize * pageNumber,
                MaxResultCount = pageSize,
            };
            return await iTacGiaService.Paging(pageInput);
        }
        public async Task<bool> xoaTacGiaById(int Id)
        {
            var xoaNVTheoId = await iTacGiaService.DeleteTacGiaById(Id);
            return xoaNVTheoId;
        }
    }
}
