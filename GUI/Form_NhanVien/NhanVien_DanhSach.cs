using DAL.Common;
using DAL.Services.NhanVien;
using DAL.Services.NhanVien.DTO;
using DAL.Services.Sachs.DTO;
using GUI.Form_NhanVien;
using GUI.Form_Sach;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class NhanVien_DanhSach : Form
    {
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        NhanVien_BLL nhanVien_BLL = new NhanVien_BLL(new NhanVienService());
        NhanVienFilterInput nhanVien_Filter = new NhanVienFilterInput();
        public NhanVien_DanhSach()
        {
            InitializeComponent();
        }

        public void NhanVien_DanhSach_Load(object sender, EventArgs e)
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
            var pageResultDTO = await nhanVien_BLL.LayDanhSachNV(pageNumber, pageSize, nhanVien_Filter);
            var listNhanVien = pageResultDTO.Items.ToList();
            totalCount = pageResultDTO.TotalCount;
            maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
            dtgNhanVien.Rows.Clear();
            foreach (var nv in listNhanVien)
            {
                int rowIndex = dtgNhanVien.Rows.Add();
                dtgNhanVien.Rows[rowIndex].Cells["ID"].Value = nv.Id;
                dtgNhanVien.Rows[rowIndex].Cells["TenNhanVien"].Value = nv.TenNhanVien;
                dtgNhanVien.Rows[rowIndex].Cells["DiaChi"].Value = nv.DiaChi != null ? nv.DiaChi : string.Empty;
                dtgNhanVien.Rows[rowIndex].Cells["SoDienThoai"].Value = nv.SoDienThoai != null ? nv.SoDienThoai : string.Empty;
                dtgNhanVien.Rows[rowIndex].Cells["CCCD"].Value = nv.CCCD != null ? nv.CCCD : string.Empty;
                dtgNhanVien.Rows[rowIndex].Cells["VaiTro"].Value = nv.VaiTro;
                dtgNhanVien.Rows[rowIndex].Cells["TaiKhoan"].Value = nv.TaiKhoan != null ? nv.TaiKhoan : string.Empty ;
                dtgNhanVien.Rows[rowIndex].Cells["MatKhau"].Value = nv.MatKhau != null ? nv.MatKhau : string.Empty;
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

        private async void dtgNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string MaNhanVien = dtgNhanVien.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                if (e.ColumnIndex == dtgNhanVien.Columns["Xoa"].Index && e.RowIndex >= 0)
                {

                    DialogResult result = MessageBox.Show("Bạn có muốn xóa tài khoản và toàn bộ thông tin của nhân viên này khỏi cơ sở dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {

                        var skXoa = await nhanVien_BLL.xoaNVTheoId(Int32.Parse(MaNhanVien));
                        if (skXoa)
                            MessageBox.Show("Xóa Thành Công");
                        else
                            throw new Exception("Lỗi xóa sách");
                        await showDuLieuNV();
                    }
                    else if (result == DialogResult.No)
                    {
                        Console.WriteLine("Bạn đã chọn 'Không'");
                    }
                }
                if (e.ColumnIndex == dtgNhanVien.Columns["ChiTiet"].Index && e.RowIndex >= 0)
                {
                    var nhanVienTT = new NhanVien_ThongTinTaiKhoan(Int32.Parse(MaNhanVien));
                    nhanVienTT.Show(this);
                }

            }
        }

        private async void btnTim_Click(object sender, EventArgs e)
        {
            nhanVien_Filter.TenNhanVien = txtTenNhanVien.Text;
            nhanVien_Filter.DiaChi = txtDiaChi.Text;
            nhanVien_Filter.CCCD = txtCCCD.Text;
            nhanVien_Filter.SoDienThoai = txtSoDienThoai.Text;
            await showDuLieuNV();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var themNhanVien = new NhanVien_ThongTinTaiKhoan();
            themNhanVien.Show(this);
        }
    }
    public class NhanVien_BLL
    {
        private INhanVienService inhanVienService;
        public NhanVien_BLL(INhanVienService sv)
        {
            inhanVienService = sv;
        }
        public async Task<PageResultDTO<NhanVien_DTO>> LayDanhSachNV(int pageNumber, int pageSize, NhanVienFilterInput input = null)
        {
            var pageInput = new PagingInput<NhanVienFilterInput>(input)
            {
                SkipCount = pageSize * pageNumber,
                MaxResultCount = pageSize,
            };
            return await inhanVienService.Paging(pageInput);
        }
        public async Task<bool> xoaNVTheoId(int Id)
        {
            var xoaNVTheoId = await inhanVienService.DeleteNhanVienById(Id);
            return xoaNVTheoId;
        }
    }
}
