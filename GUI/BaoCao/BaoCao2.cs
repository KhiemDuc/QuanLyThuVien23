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
            chartTitle.Text = "Biểu Đồ Số Lượng Sách Mượn Trong Tháng " + month.ToString() + "/"+year.ToString() + " (Theo Thể Loại)"; // Đặt nội dung tiêu đề
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
            chartSachTheoThang.Series[0].Points.Clear();
            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Số Lượng Sách Mượn \nTrong Tháng " + month.ToString() +"/"+ year.ToString() + " (Theo ngày)"; // Đặt nội dung tiêu đề
            chartSachTheoThang.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _phieumuon_sachService.GetNgayMuonVaSoLuong(month, year);

            for (int day = 1; day < dayInMonth((int)month,(int)year); day++)
            {

                // Nếu có sách trong ngày, thêm giá trị vào biểu đồ
                if (tongSachTheoTheLoai.ContainsKey(day))
                {
                    int count = tongSachTheoTheLoai[day];
                    var point = new SeriesPoint(day.ToString(), count);
                    chartSachTheoThang.Series[0].Points.Add(point);
                }
                else
                {
                   
                    var point = new SeriesPoint(day.ToString(), 0);
                    // Nếu không có sách trong ngày, thêm giá trị 0 vào biểu đồ
                    chartSachTheoThang.Series[0].Points.Add(point);
                }
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
            List<int> years = _phieuMuonService.GetNamTrongPhieuMuon();
            List<int> months = _phieuMuonService.GetThangTrongPhieuMuon();

            foreach(int year in years)
                cmbNam.Items.Add(year);
            
            foreach(int month in months)
                cmbThang.Items.Add(month);

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            cmbThang.SelectedItem = currentMonth;
            cmbNam.SelectedItem = currentYear;

        }
        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {
            string selectedValueYear = cmbNam.SelectedItem.ToString();
            string selectedValueMonth = cmbThang.SelectedItem.ToString();

            
            int intValueYear;
            int intValueMonth;
            if (int.TryParse(selectedValueYear, out intValueYear) && int.TryParse(selectedValueMonth,out intValueMonth))
            {
                LoadData(intValueMonth, intValueYear);
            }
        }
        int dayInMonth(int month,int year)
        {
           int daysInMonth = DateTime.DaysInMonth(year, month);
           return daysInMonth;
        }
    }
}
