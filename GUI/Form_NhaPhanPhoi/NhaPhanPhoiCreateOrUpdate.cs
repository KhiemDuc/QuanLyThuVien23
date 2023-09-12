using DAL.Services.NhaPhanPhois;
using DAL.Services.NhaPhanPhois.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace GUI.Form_NhaPhanPhoi
{
    public partial class NhaPhanPhoiCreateOrUpdate : Form
    {
        INhaPhanPhoiService _iNhaPhanPhoiService = new NhaPhanPhoiService();
        public NhaPhanPhoiCreateOrUpdate()
        {
            InitializeComponent();
        }
        string ID_CapNhat;
        public NhaPhanPhoiCreateOrUpdate( string ID) : this()
        {
            this.ID_CapNhat = ID;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var nhaPhanPhoiMoi = new NhaPhanPhoiCreateInput
            {
                NhaPhanPhoiId = txtMaNhaPhanPhoi.Text,
                TenNhaPhanPhoi = txtTenNhaPhanPhoi.Text,
                SoDienThoai = txtSoDienThoai.Text,
                DiaChi = txtDiaChi.Text,
            };
            if(!string.IsNullOrEmpty(ID_CapNhat))
            {
                txtMaNhaPhanPhoi.Enabled = false;
               
                _iNhaPhanPhoiService.UpdateNhaPhanPhoi(ID_CapNhat, nhaPhanPhoiMoi);
                MessageBox.Show("Cập Nhật Thành Công Nhà Phân Phối");
                this.Close();
            }
            else
            {
                if(_iNhaPhanPhoiService.QueryFilter().Any(x => x.ID == nhaPhanPhoiMoi.NhaPhanPhoiId))
                {
                    MessageBox.Show("Mã Nhà Phân Phối Đã Được Dùng. Vui Lòng Chọn Mã Khác");
                }
                else
                {
                    _iNhaPhanPhoiService.CreateNhaPhanPhoi(nhaPhanPhoiMoi);
                    MessageBox.Show("Thêm Mới Thành Công Nhà Phân Phối Vào Trong CSDL");
                    this.Close();
                }  
            }
            
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void NhaPhanPhoiCreateOrUpdate_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ID_CapNhat))
            {
                var nhaPhanPhoiCapNhat = await _iNhaPhanPhoiService.GetById(ID_CapNhat);
                txtMaNhaPhanPhoi.Text = nhaPhanPhoiCapNhat.ID;
                txtTenNhaPhanPhoi.Text = nhaPhanPhoiCapNhat.TenNhaPhanPhoi;
                txtDiaChi.Text = nhaPhanPhoiCapNhat.DiaChi ?? string.Empty;
                txtSoDienThoai.Text = nhaPhanPhoiCapNhat.SoDienThoai ?? string.Empty;
            }
        }
    }
}
