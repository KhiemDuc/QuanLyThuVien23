using DAL.Model;
using DAL.QLTVExceptions;
using DAL.Services.DocGias;
using DAL.Services.NhanVien;
using DAL.Services.PhieuMuon_PhieuMuon_Sachs;
using DAL.Services.PhieuMuon_Sach_Sachs;
using DAL.Services.PhieuMuon_Sachs;
using DAL.Services.PhieuMuons;
using DAL.Services.PhieuMuons.DTO;
using DAL.Services.Sachs.DTO;
using DAL.Services.TacGias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Form_PhieuMuon
{
    public partial class PhieuMuon_CRUD : Form
    {
        #region Khai báo
        Sach_BLL sach_BLL = new Sach_BLL();
        SachFilterInput sachFilterInput;
        ISachService _iSachService = new SachService();
        ITacGiaService _iTacGiaService = new TacGiaService();
        IPhieuMuonService _iphieuMuonService = new PhieuMuonService();
        IPhieuMuon_SachsService phieuMuon_SachsService = new PhieuMuon_SachsService();
        IDocGiaService docGiaService = new DocGiaService();
        INhanVienService nhanVienService = new NhanVienService();
        List<PhieuMuon_SachCreateInput> listSachMuon = new List<PhieuMuon_SachCreateInput>();
        #endregion
        public PhieuMuon_CRUD()
        {
            InitializeComponent();
            //cbbTacGiaSM.Items.Clear();

        }


        private async void PhieuMuon_CRUD_Load(object sender, EventArgs e)
        {
            #region Thêm dữ liệu cbb

            txtMaDocGia.Items.Clear();
            var listMaDocGia = docGiaService.QueryFilter().Select(x => x.ID).ToList();
            foreach (var item in listMaDocGia)
                txtMaDocGia.Items.Add(item);
            var listMaNhanVien = nhanVienService.QueryFilter().Select(x => x.ID).ToList();
            foreach (var item in listMaNhanVien)
                txtMaNhanVien.Items.Add(item);
            #endregion
            await showDuLieuSach();
        }

       



        private async Task showDuLieuSach()
        {
            var pageResultDTO = await sach_BLL.LayDanhSachSach(0, 0, sachFilterInput);
            var listSach = pageResultDTO.Items.ToList();

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

        #region xử lý các button
        private async void btnTim_ClickAsync(object sender, EventArgs e)
        {
            sachFilterInput = new SachFilterInput()
            {
                TenSach = txtTenSach.Text,
                TenTacGia = txtTacGia.Text,
            };
            await showDuLieuSach();
        }

       
        private void btnThem_Sm_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dtgSach.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                foreach (DataGridViewRow selectedRow in dtgSach.SelectedRows)
                {
                    int selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                    int rowIndexToUpdate = -1;

                    // Tìm xem ID_Sm đã tồn tại trong dtgSm chưa
                    for (int i = 0; i < dtgSm.Rows.Count; i++)
                    {
                        int existingId = Convert.ToInt32(dtgSm.Rows[i].Cells["ID_Sm"].Value);
                        if (existingId == selectedId)
                        {
                            rowIndexToUpdate = i;
                            break;
                        }
                    }

                    if (rowIndexToUpdate >= 0)
                    {
                        // Nếu đã tồn tại thì tăng số lượng lên 1
                        int currentQuantity = Convert.ToInt32(dtgSm.Rows[rowIndexToUpdate].Cells["SoLuong_Sm"].Value);
                        dtgSm.Rows[rowIndexToUpdate].Cells["SoLuong_Sm"].Value = currentQuantity + 1;
                    }
                    else
                    {
                        // Nếu chưa tồn tại thì thêm mới dòng
                        var rowIndex = dtgSm.Rows.Add();
                        dtgSm.Rows[rowIndex].Cells["ID_Sm"].Value = selectedId;
                        dtgSm.Rows[rowIndex].Cells["Ten_Sm"].Value = selectedRow.Cells["TenSach"].Value;
                        dtgSm.Rows[rowIndex].Cells["TacGia_Sm"].Value = selectedRow.Cells["TenTacGia"].Value;
                        dtgSm.Rows[rowIndex].Cells["SoLuong_Sm"].Value = 1;
                    }
                }
            }
        }

        private async void btnTaoPhieuMuon_Click(object sender, EventArgs e)
        {
            listSachMuon = await AddDuLieuPHieuMuon_Sach();
            try
            {
                if(listSachMuon.Count > 0)
                {
                    var phieuMuonCreate = new PhieuMuonCreateInput
                    {
                        PhieuMuonId = txtMaPhieuMuon.Text,
                        DocGiaId = Int32.Parse(txtMaDocGia.Text),
                        NhanVienId = Int32.Parse(txtMaNhanVien.Text),
                        GhiChu = txtGhiChu.Text,
                        NgayMuon = dtpNgayMuon.Value,
                        NgayTra = dtpNgayTra.Value,
                        TrangThaiId = 1,
                        ListSachMuon = listSachMuon
                    };
                    await _iphieuMuonService.CreatePhieuMuon(phieuMuonCreate);
                    MessageBox.Show("Tạo thành công phiếu mượn");
                    await showDuLieuSach();
                }
                else
                {
                    MessageBox.Show("Không có cuốn sách nào trong phiếu mượn");
                }
            }catch(PhieuMuon_SachException ex)
            {
                MessageBox.Show(ex.Message);
                await showDuLieuSach();
            }
            finally
            {
                
            }
        }
        private async Task<List<PhieuMuon_SachCreateInput>> AddDuLieuPHieuMuon_Sach()
        {
            List<PhieuMuon_SachCreateInput> list = new List<PhieuMuon_SachCreateInput>();
            foreach (DataGridViewRow row in dtgSm.Rows)
            {
                var phieuMuonMoi = new PhieuMuon_SachCreateInput
                {
                    SachId = Int32.Parse( row.Cells["Id_Sm"].Value.ToString()),
                    SoLuong = Int32.Parse( row.Cells["SoLuong_Sm"].Value.ToString()),
                };
                list.Add(phieuMuonMoi);
            }
            return list;
        }
        #endregion


        #region xử lý textChange
        private void txtMaDocGia_TextChanged(object sender, EventArgs e)
        {
            var DocGia = docGiaService.QueryFilter().FirstOrDefault(x => x.ID.ToString() == txtMaDocGia.Text);
            if (DocGia != null)
            {
                txtTenDocGia.Text = DocGia.TenDocGia.ToString();
            }
            else
                txtTenDocGia.Text = string.Empty;

        }

        private void txtMaNhanVien_TextChanged(object sender, EventArgs e)
        {

            var NhanVien = nhanVienService.QueryFilter().FirstOrDefault(x => x.ID.ToString() == txtMaNhanVien.Text);
            if (NhanVien != null)
            {
                txtTenNhanVien.Text = NhanVien.TenNhanVien.ToString();
            }
            else
                txtTenNhanVien.Text = string.Empty;
        }
        private void txtMaPhieuMuon_TextChanged(object sender, EventArgs e)
        {
        }



        #endregion

        private void txtThoiHan_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtThoiHan.Text))
            {
                dtpNgayTra.Value = dtpNgayMuon.Value.AddDays(Int32.Parse(txtThoiHan.Text));
            }
        }

        private void btnTaoPhieuMuon_Click_1(object sender, EventArgs e)
        {

        }
    }
    public class PhieuMuon_SachsBLL
    {
       

    }
}
