using DAL.Services.NhanVien;
using DAL.Services.NhanVien.DTO;
using DAL.Services.VaiTros;
using DevExpress.XtraEditors;
using GUI.Form_Sach;
using GUI.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Form_NhanVien
{
    public partial class NhanVien_ThongTinTaiKhoan : Form
    {
        public int Role_Id = 1;
        int ID_CapNhat = 0;
        INhanVienService _iNhanVienService = new NhanVienService();
        IVaiTroService _iVaiTroService = new VaiTroService();
        public NhanVien_ThongTinTaiKhoan()
        {
            InitializeComponent();
        }

        public NhanVien_ThongTinTaiKhoan(int Id):this()
        {
            ID_CapNhat = Id;
        }
        private async void btnLuu_Click(object sender, EventArgs e)
        {
            
            NhanVienCreateInput nhanVien = new NhanVienCreateInput()
            {
                TenNhanVien = txtTenNhanVien.Text,
                CCCD = txtCCCD.Text,
                DiaChi = txtDiaChi.Text,
                AnhNhanVien = XuLyAnh.ImageToByteArray(ptbAnhNhanVien.Image),
                GioiTinh = cbbGioiTinh.Text,
                SoDienThoai = txtSoDienThoai.Text,
                VaiTroId = _iVaiTroService.QueryFilter().Where(x => x.TenRole == txtVaiTro.Text).Select(x => x.ID).FirstOrDefault(),
            };
            if (!string.IsNullOrEmpty(dtpNgaySinh.Text))
            {
                nhanVien.NgaySinh = dtpNgaySinh.DateTime;
            }
            if (!string.IsNullOrEmpty(dtpNgayVaoLam.Text))
            {
                nhanVien.NgayVaoLam = dtpNgayVaoLam.DateTime;
            }
            var tenTaiKhoan = _iNhanVienService.QueryFilter().Any(x=> x.TaiKhoan == txtTenDangNhap.Text);
            if(tenTaiKhoan && ID_CapNhat == 0)
            {
                MessageBox.Show("Tên đăng nhập này đã tồn tại, vui lòng chọn một tên đăng nhập mới");
            }
            else
            {
                nhanVien.TaiKhoan = txtTenDangNhap.Text;
                if (ID_CapNhat == 0)
                {

                    nhanVien.MatKhau = txtMatKhauHienTai.Text;
                    await _iNhanVienService.CreateNhanVien(nhanVien);
                    MessageBox.Show("Thêm Thành Công Nhân Viên Vào Trong CSDL");
                    this.Close();
                }
                else
                {

                    var nhanVienX = await _iNhanVienService.GetById(ID_CapNhat);
                    if (string.IsNullOrEmpty(txtMatKhauHienTai.Text) && string.IsNullOrEmpty(txtMatKhauMoi.Text) && string.IsNullOrEmpty(txtXacNhanMatKhauMoi.Text))
                    {
                        // Cập nhật thông tin thông thường
                        nhanVien.MatKhau = nhanVienX.MatKhau;
                        await _iNhanVienService.UpdateNhanVien(ID_CapNhat, nhanVien);
                        MessageBox.Show("Cập nhật thành công thông tin nhân viên");
                        this.Close();
                    }
                    else
                    {
                        // Cập Nhật Thông Tin và Mật Khẩu
                        if (txtMatKhauHienTai.Text == nhanVienX.MatKhau)
                        {
                            if (txtMatKhauMoi.Text == txtXacNhanMatKhauMoi.Text)
                            {
                                nhanVien.MatKhau = txtMatKhauMoi.Text;
                                await _iNhanVienService.UpdateNhanVien(ID_CapNhat, nhanVien);
                                MessageBox.Show("Cập nhật thành công thông tin nhân viên");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Mật khẩu xác nhận không đúng! Vui lòng kiểm tra lại");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu không đúng! Vui lòng kiểm tra lại");
                        }
                    }
                }
            }
            
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void NhanVien_ThongTinTaiKhoan_Load(object sender, EventArgs e)
        {

            Role_Id = Login_form.Role_Id;
            if(Role_Id != 1)
            {
                 controlVaiTro.HideToCustomization();
            }
            if(ID_CapNhat == 0)
            {
                controlMatKhauHienTai.Text = "Mật Khẩu";
                conTrolMatKhauMoi.HideToCustomization();
                controlXacNhanMatKhauMoi.HideToCustomization();
            }
            else
            {
                var nhanVien = await _iNhanVienService.GetById(ID_CapNhat);
                txtTenNhanVien.Text = nhanVien.TenNhanVien;
                dtpNgaySinh.Text = nhanVien?.NgaySinh.ToString() ?? string.Empty;
                dtpNgayVaoLam.Text = nhanVien?.NgayVaoLam.ToString() ?? string.Empty;
                txtSoDienThoai.Text = nhanVien?.SoDienThoai.ToString() ?? string.Empty;
                txtCCCD.Text = nhanVien.CCCD ?? string.Empty;
                txtDiaChi.Text = nhanVien.DiaChi ?? string.Empty;
                ptbAnhNhanVien.Image = XuLyAnh.ByteArrayToImage(nhanVien.AnhDaiDien);
                cbbGioiTinh.Text = nhanVien.GioiTinh ?? string.Empty;
                txtTenDangNhap.Text = nhanVien.TaiKhoan ??string.Empty;
                txtVaiTro.Text = nhanVien.VaiTro.TenRole;
            }
        }

        private void NhanVien_ThongTinTaiKhoan_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Owner != null)
                ((NhanVien_DanhSach)this.Owner).NhanVien_DanhSach_Load(null, null);
        }
    }
}
