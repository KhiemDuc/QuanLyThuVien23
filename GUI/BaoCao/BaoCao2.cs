using DAL.Services.PhieuMuon_PhieuMuon_Sachs;
using DAL.Services.PhieuMuon_Sach_Sachs;
using DAL.Services.PhieuMuons;
using DAL.Services.Sachs.DTO;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.BaoCao
{
    public partial class BaoCao2 : UserControl
    {
        private readonly ISachService _sachService;
        private readonly IPhieuMuonService _phieuMuonService;
        private readonly IPhieuMuon_SachsService _phieumuon_sachService;

        public BaoCao2()
        {
            InitializeComponent();
            _sachService = new SachService();
            _phieuMuonService = new PhieuMuonService();
            _phieumuon_sachService = new PhieuMuon_SachsService();
            LoadData();
            LoadComBoBox();
        }
        void LoadData(int? month = null, int? year = null)
        {
            if (!month.HasValue)
            {
                month = DateTime.Now.Month;
            }
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }
            FillChart(month, year);
            LoadTop5(month, year);
            FillChart2(month, year);
            

        }
        public void FillChart(int? month = null, int? year = null)
        {
            chartTheLoaiTheoThang.Titles.Clear(); // Xóa các tiêu đề hiện có (nếu có)

            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Số Lượng Thể Loại Sách Mượn Trong Tháng " + month.ToString() + "/"+year.ToString(); // Đặt nội dung tiêu đề
            chartTheLoaiTheoThang.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _sachService.GetBookCategoryStatistics(month, year);
            chartTheLoaiTheoThang.Series[0].Points.Clear();
            foreach (var item in tongSachTheoTheLoai)
            {
                var point = new SeriesPoint(item.Key, item.Value);
                chartTheLoaiTheoThang.Series[0].Points.Add(point);
                chartTheLoaiTheoThang.Series[0].Name = item.Key;
            }
            

            ((XYDiagram)chartTheLoaiTheoThang.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            ((XYDiagram)chartTheLoaiTheoThang.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

        }
        public void FillChart2(int? month = null , int? year = null)
        {
            chartSachTheoThang.Titles.Clear(); // Xóa các tiêu đề hiện có (nếu có)

            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Số Lượng Sách Mượn Trong Tháng " + month.ToString() +"/"+ year.ToString(); // Đặt nội dung tiêu đề
            chartSachTheoThang.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _phieumuon_sachService.GetNgayMuonVaSoLuong(month, year);
            chartSachTheoThang.Series[0].Points.Clear();
            foreach (var item in tongSachTheoTheLoai)
            {
                var point = new SeriesPoint(item.Key, item.Value);
                chartSachTheoThang.Series[0].Points.Add(point);
                chartSachTheoThang.Series[0].Name = item.Key.ToString();
            }


            ((XYDiagram)chartSachTheoThang.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            ((XYDiagram)chartSachTheoThang.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;

        }
        void LoadTop5(int? month = null, int? year= null)
        {
            groupControl2.Text = "Top 5 Sách Được Mượn Nhiều Nhất Trong Tháng " + month.ToString() + "/" + year.ToString();
            var danhSachTop5 = _sachService.GetTop5SachByMonth(month, year);
            for (int i = 0; i < 5; i++)
            {
                if (i < danhSachTop5.Count)
                {
                    switch (i)
                    {
                        case 0:
                            lblTenSachTop1.Text = danhSachTop5[i] ?? string.Empty;
                            break;
                        case 1:
                            lbTenSachTop2.Text = danhSachTop5[i] ?? string.Empty;
                            break;
                        case 2:
                            lbTenSachTop3.Text = danhSachTop5[i] ?? string.Empty;
                            break;
                        case 3:
                            lbTenSachTop4.Text = danhSachTop5[i] ?? string.Empty;
                            break;
                        case 4:
                            lbTenSachTop5.Text = danhSachTop5[i] ?? string.Empty;
                            break;
                    }
                }
            }
        }
        void LoadComBoBox()
        {
            int currentYear = DateTime.Now.Year;
            List<int> years = _phieuMuonService.GetNamTrongPhieuMuon();
            cmbNam.DataSource = years;
            cmbNam.SelectedIndex = cmbNam.Items.IndexOf(currentYear.ToString());
            cmbThang.SelectedIndex = DateTime.Now.Month - 1;
        }
        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            string selectedValue = cmbNam.SelectedItem.ToString();
            int month = cmbThang.SelectedIndex + 1;
            int intValue;
            if (int.TryParse(selectedValue, out intValue))
            {
                LoadData(month, intValue);
            }
        }
    }
}
