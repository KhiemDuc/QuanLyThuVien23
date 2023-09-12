
using DAL.Common;
using DAL.Model;
using DAL.Services.Sachs.DTO;
using GUI.Form_PhieuMuon;
using GUI.Form_Sach;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Sach_DanhSach : Form
    {
        int pageNumber = 0;
        int pageSize = 10;
        int totalCount = 0;
        int maxPage;
        Sach_BLL sach_BLL = new Sach_BLL(new SachService());
        SachFilterInput sach_Filter = new SachFilterInput();    
        public Sach_DanhSach()
        {
            InitializeComponent();
        }
       
       
        public async void Sach_DanhSach_Load(object sender, EventArgs e)
        {
            await showDuLieuSach();
        }
        
        private async Task showDuLieuSach()
        {
            try
            {
                var pageResultDTO = await sach_BLL.LayDanhSachSach(pageNumber, pageSize, sach_Filter);
                var listSach = pageResultDTO.Items.ToList();
                totalCount = pageResultDTO.TotalCount;
                maxPage = (int)Math.Ceiling(totalCount / (float)pageSize);
                dtgSach.Rows.Clear();
                foreach (var sach in listSach)
                {
                    int rowIndex = dtgSach.Rows.Add();
                    dtgSach.Rows[rowIndex].Cells["ID"].Value = sach.SachId;
                    dtgSach.Rows[rowIndex].Cells["TenSach"].Value = sach.TenSach;
                    dtgSach.Rows[rowIndex].Cells["TenTacGia"].Value = sach.TenTacGia != null ? sach.TenTacGia : string.Empty;
                    dtgSach.Rows[rowIndex].Cells["TenTheLoai"].Value = sach.TenTheLoai;
                    dtgSach.Rows[rowIndex].Cells["NhaPhanPhoi"].Value = sach.TenNhaPhanPhoi;
                    dtgSach.Rows[rowIndex].Cells["NamXB"].Value = sach.NgayXb.ToString("dd/MM/yyyy");
                    dtgSach.Rows[rowIndex].Cells["SoLuong"].Value = sach.SoLuong;
                    dtgSach.Rows[rowIndex].Cells["DonGia"].Value = sach.DonGia;
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
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

            if ( pageNumber + 1 >= maxPage)
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
        } // Show Dữ liệu 
        
        #region button Điều hướng

        private void btnTrangDau_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            showDuLieuSach().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ trở về trang đầu tiên"); } });
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            pageNumber--;
            showDuLieuSach().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ trở về trước"); } });
        }

        private  void btnSau_Click(object sender, EventArgs e)
        {
            pageNumber++;
            showDuLieuSach().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ next trang"); } });
        }

        private void btnTrangCuoi_Click(object sender, EventArgs e)
        {
            pageNumber = maxPage - 1;
            showDuLieuSach().ContinueWith(x => { if (x.IsFaulted) { MessageBox.Show("Lỗi chỗ đến trang cuối"); } });
        }
        #endregion
        #region xử lý cell content click
        private async void dtgSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string MaSach = dtgSach.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                if (e.ColumnIndex == dtgSach.Columns["Xoa"].Index && e.RowIndex >= 0)
                {

                    DialogResult result = MessageBox.Show("Bạn có muốn xóa cuốn sách này khỏi cơ sở dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var skXoa = await sach_BLL.xoaSachTheoId(Int32.Parse(MaSach));
                        if (skXoa)
                            await showDuLieuSach();
                        else
                            throw new Exception("Lỗi xóa sách");
                    }
                    else if (result == DialogResult.No)
                    {
                        Console.WriteLine("Bạn đã chọn 'Không'");
                    }
                }
                if (e.ColumnIndex == dtgSach.Columns["ChiTiet"].Index && e.RowIndex >= 0)
                {
                    var sachCrud = new SachCreateOrUpdate(Int32.Parse(MaSach));
                    sachCrud.FormClosed += childFormClose;
                    sachCrud.Show(this);
                }

            }
        }
        #endregion

        #region button sukien
        //Button Tìm kiếm
    
        //Button Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            var sachCrud = new SachCreateOrUpdate();
            sachCrud.FormClosed += childFormClose;
            sachCrud.Show(this);
           
        }

        private void childFormClose(object sender, FormClosedEventArgs e)
        {
            Sach_DanhSach_Load(null, null);
        }
        #endregion

        private void btnTim_Click(object sender, EventArgs e)
        {
            sach_Filter.TenTacGia = txtTenTacGia.Text;
            sach_Filter.TenSach = txtTenSach.Text;
            sach_Filter.TenNhaPhanPhoi = txtTenNhaPhanPhoi.Text;
            
            showDuLieuSach().ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    MessageBox.Show("Lỗi chỗ tìm kiếm");
                }
            });
        }
        }

    public class Sach_BLL
    {
        private ISachService isachService;
        public Sach_BLL(ISachService sv)
        {
            this.isachService = sv;
        }
        public Sach_BLL()
        {
            isachService = new SachService();
        }
        public async Task<PageResultDTO<Sach_DTO>> LayDanhSachSach(int pageNumber = 0, int pageSize = 0, SachFilterInput input = null)
        {
            var pageInput = new PagingInput<SachFilterInput>(input);
            if (pageNumber == 0 && pageSize == 0)
            
                pageInput.MaxResultCount = -1;           
            else
            {
                pageInput.SkipCount = pageSize * pageNumber;
                pageInput.MaxResultCount = pageSize;
            };
            return await isachService.Paging(pageInput);
        }
        
        public async Task<bool> xoaSachTheoId(int Id)
        {
            var xoaSachTheoId = await isachService.DeleteSachById(Id);
            return xoaSachTheoId;
        }
    }



}
