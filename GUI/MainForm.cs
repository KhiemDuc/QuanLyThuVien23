using DevExpress.XtraBars;
using GUI.Form_DocGia;
using GUI.Form_NhanVien;
using GUI.Form_NhaPhanPhoi;
using GUI.Form_PhieuMuon;
using GUI.Form_TacGia;
using GUI.Form_TheLoai;
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

namespace GUI
{
    public partial class MainForm : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public int RoleId = Login_form.Role_Id;
        public int ID_Login = Login_form.User_Id;
        public MainForm()
        {
            
            InitializeComponent();
        }

        private Form activeForm = null;
        public void showChildForm(Form childForm) 
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            Container.Controls.Add(childForm);
            Container.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        private void btnSach_Click(object sender, EventArgs e)
        {
            
        }

        
        private void btnBaoCao_Click(object sender, EventArgs e)
        {

        }

      

        private void accordionControlElement1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if(RoleId != 1)
            {
                btnNhanVien.Visible = false;
            }
            
        }
        private void btnKhoSach_Click(object sender, EventArgs e)
        {
            showChildForm(new Sach_DanhSach());
        }

        private void btnTacGia_Click(object sender, EventArgs e)
        {
            showChildForm(new TacGia_DanhSach());
        }

        private void btnTheLoai_Click(object sender, EventArgs e)
        {
            showChildForm(new TheLoai_DanhSach());
        }

        private void btnNhaPhanPhoi_Click(object sender, EventArgs e)
        {
            showChildForm(new NhaPhanPhoi_DanhSach());
        }

        private void btnDocGia_Click(object sender, EventArgs e)
        {
            showChildForm(new DocGia_DanhSach());
        }

        private void btnPhieuMuon_Click(object sender, EventArgs e)
        {
            showChildForm(new PhieuMuon_DanhSach());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            showChildForm(new NhanVien_DanhSach());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            showChildForm(new NhanVien_ThongTinTaiKhoan(1));
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (Owner != null && !Owner.Disposing && !Owner.IsDisposed && !Owner.Visible)
                Owner.Show();
            this.Close();
        }
    }
}
