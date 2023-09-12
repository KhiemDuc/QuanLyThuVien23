using DAL.Services.NhaPhanPhois;
using DAL.Services.Sachs.DTO;
using DAL.Services.TacGias;
using DAL.Services.TheLoais;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Form_Sach
{
    public partial class SachCreateOrUpdate : Form
    {
        ISachService _iSachService = new SachService();
        ITheLoaiService _iTheLoaiService = new TheLoaiService();
        ITacGiaService _iTacGiaService = new TacGiaService();
        INhaPhanPhoiService _iNhaPhanPhoiService = new NhaPhanPhoiService();
        int ID_CapNhat = 0;
        public SachCreateOrUpdate()
        {
            InitializeComponent();
        }

        
        public SachCreateOrUpdate(int ID) : this()
        {
            this.ID_CapNhat = ID;
        }
        private async void SachCreateOrUpdate_Load(object sender, EventArgs e)
        {
            loadDuLieuComboBox();

            if (ID_CapNhat != 0)
            {
                var sachCapNhat = await _iSachService.GetById(ID_CapNhat);
                txtTenSach.Text = sachCapNhat.TenSach;
                txtMaTacGia.Text = sachCapNhat.ID_TacGia != null ? sachCapNhat.TacGia.ID.ToString() : string.Empty;
                txtMaNhaPhanPhoi.Text = sachCapNhat.ID_NhaPhanPhoi != null ? sachCapNhat.NhaPhanPhoi.ID.ToString() : string.Empty;
                txtSoLuong.Text = sachCapNhat.SoLuong.ToString();
                txtDonGia.Text = sachCapNhat.DonGia.ToString();
                ptbAnhSach.Image = XuLyAnh.ByteArrayToImage(sachCapNhat.AnhSach);
                txtMoTa.Text = sachCapNhat.MoTa.ToString();
                var listTheLoai = sachCapNhat.TheLoais.Select(x => x.TenTheLoai).ToList();
                foreach (CheckedListBoxItem item in cbbTheLoai.Properties.GetItems().Cast<CheckedListBoxItem>())
                {
                    string giaTri = item.Value.ToString();
                    item.CheckState = listTheLoai.Contains(giaTri) ? CheckState.Checked : CheckState.Unchecked;
                }
            }
        }

        private void loadDuLieuComboBox()
        {
            var listTheLoai = _iTheLoaiService.QueryFilterDto().Select(x => x.TenTheLoai).ToList();
            cbbTheLoai.Properties.DataSource = listTheLoai;
            var listNhaPhanPhoi = _iNhaPhanPhoiService.QueryFilterDto().Select(x => x.Id).ToList();
            txtMaNhaPhanPhoi.Properties.Items.AddRange(listNhaPhanPhoi);
            var listTacGia = _iTacGiaService.QueryFilterDto().Select(x => x.Id).ToList();
            txtMaTacGia.Properties.Items.AddRange(listTacGia);

        }

   

        private async void txtMaNhaPhanPhoi_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaNhaPhanPhoi.Text))
            {
                var TenNhaPhanPhoi = _iNhaPhanPhoiService.QueryFilter().FirstOrDefault(x => x.ID == txtMaNhaPhanPhoi.Text);
                if (TenNhaPhanPhoi != null)
                {
                    txtTenNhaPhanPhoi.Text = TenNhaPhanPhoi.TenNhaPhanPhoi;
                }
                else
                {
                    txtTenNhaPhanPhoi.Text = "Không tồn tại trong CSDL";
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(errLoi.GetError(txtTenSach)) && string.IsNullOrEmpty(errLoi.GetError(cbbTheLoai)) && string.IsNullOrEmpty(errLoi.GetError(txtMaNhaPhanPhoi)) 
                && string.IsNullOrEmpty(errLoi.GetError(txtMaTacGia)) && string.IsNullOrEmpty(errLoi.GetError(dtpNgayXB)) && string.IsNullOrEmpty(errLoi.GetError(txtDonGia)) && 
                string.IsNullOrEmpty(errLoi.GetError(txtSoLuong)))

            {
                var sachCRUD = new SachCreateInput
                {
                    TenSach = txtTenSach.Text,
                    TacGiaId = Int32.Parse(txtMaTacGia.Text),
                    NhaPhanPhoiId = txtMaNhaPhanPhoi.Text,
                    NgayXb = dtpNgayXB.DateTime,
                    SoLuong = Int32.Parse(txtSoLuong.Text),
                    DonGia = float.Parse(txtDonGia.Text),
                    ListTenTheLoai = cbbTheLoai.Properties.Items
                                                        .Where(item => item.CheckState == CheckState.Checked)
                                                        .Select(item => item.Value.ToString())
                                                        .ToList(),
                    AnhSach = XuLyAnh.ImageToByteArray(ptbAnhSach.Image),
                    MoTa = txtMoTa.Text
                };
                if (ID_CapNhat != 0)
                {
                    var sachCapNhat = _iSachService.UpdateSach(ID_CapNhat, sachCRUD);
                    MessageBox.Show("Cập Nhật Thành Công Vào Trong CSDL");
                }
                else
                {
                    var sachCapNhat = _iSachService.CreateSach(sachCRUD);
                    MessageBox.Show("Thêm Thành Công Vào Trong CSDL");
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui Lòng Điền Đúng Định Dạng");
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMaTacGia_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMaTacGia.Text))
            {
                int maTacGia = Int32.Parse(txtMaTacGia.Text);
                var TenTacGia = _iTacGiaService.QueryFilter().FirstOrDefault(x => x.ID == maTacGia);
                if (TenTacGia != null)
                {
                    txtTenTacGia.Text = TenTacGia.TenTacGia;
                }
                else
                {
                    txtTenTacGia.Text = "Không tồn tại trong CSDL";
                }
            }
        }
        #region Validate
        private void txtTenSach_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenSach.Text))
                errLoi.SetError(txtTenSach, "Tên sách không được để trống");
            else errLoi.ClearErrors();
        }

        private void cbbTheLoai_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(cbbTheLoai.Text))
                errLoi.SetError(cbbTheLoai, "Thể Loại không được để trống");
            else errLoi.ClearErrors();
        }

        private void txtMaTacGia_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTacGia.Text))
                errLoi.SetError(txtMaTacGia, "Mã Tác Giả không được để trống");
            else
                errLoi.ClearErrors();
        }

        private void txtTenTacGia_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtMaNhaPhanPhoi_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhaPhanPhoi.Text))
                errLoi.SetError(txtMaNhaPhanPhoi, "Mã Nhà Phân Phối không được để trống");
            else
                errLoi.ClearErrors();
        }

        private void dtpNgayXB_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(dtpNgayXB.Text))
                errLoi.SetError(dtpNgayXB, "Ngày Xuất Bản không được để trống");
            else
                errLoi.ClearErrors();
        }

        private void txtSoLuong_Validating(object sender, CancelEventArgs e)
        {
            if (txtSoLuong.Value < 0)
                errLoi.SetError(txtSoLuong, "Số lượng không được nhỏ hơn 0");
            else errLoi.ClearErrors();
        }

        private void txtDonGia_Validating(object sender, CancelEventArgs e)
        {
            if (txtDonGia.Value < 0)
                errLoi.SetError(txtDonGia, "Đơn giá không được nhỏ hơn 0");
            else
                errLoi.ClearErrors();
        }
        #endregion
    }
    public class XuLyAnh
    {
        public XuLyAnh() { }
        public static byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Chọn định dạng ảnh tùy theo yêu cầu
                return ms.ToArray();
            }
        }
        public static Image ByteArrayToImage(byte[] img)
        {
            if (img == null)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream(img))
            {
                ImageConverter imageConverter = new ImageConverter();
                Image image = (Image)imageConverter.ConvertFrom(img);
                return image;
            }
        }
    }
}
