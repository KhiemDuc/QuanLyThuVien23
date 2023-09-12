using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.Services.Sachs;
using DAL.Services.Sachs.DTO;
using DevExpress.XtraCharts;
using DevExpress.Utils.DirectXPaint;
using DAL.Services.PhieuMuon_PhieuMuon_Sachs;
using DAL.Services.PhieuMuon_Sach_Sachs;
using DAL.Services.TheLoais;
using DAL.Services.TacGias;

namespace GUI.BaoCao
{
    public partial class BaoCao : DevExpress.XtraEditors.XtraForm
    {
        private readonly ISachService _sachService;
        private readonly IPhieuMuon_SachsService _phieumuon_sachService;
        private readonly ITheLoaiService _theLoaiService;
        private readonly ITacGiaService _tacGiaService;

        public BaoCao()
        {
            InitializeComponent();
            _sachService = new SachService();
            _phieumuon_sachService = new PhieuMuon_SachsService();
            _theLoaiService = new TheLoaiService();
            _tacGiaService = new TacGiaService();
        }

        private void XtraForm2_Load(object sender, EventArgs e)
        {
            
            lblTongSach.Text = _sachService.getTongSach().ToString();
            lblSachChoMuon.Text = _phieumuon_sachService.getTongSachChoMuon().ToString();
            lblSoLuongTheLoai.Text = _theLoaiService.SoLuongTheLoai().ToString();
            lblSoLuongTacGia.Text = _tacGiaService.SoLuongTacGia().ToString();
            FillChartPie();
            FillChart();
            FillChartLine();

        }
        
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        public void FillChartPie()
        {
            chartTyLeTheLoai.Titles.Clear(); // Xóa các tiêu đề hiện có (nếu có)
            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Tỷ Lệ Thể Loại Các Loại Sách"; // Đặt nội dung tiêu đề
            chartTyLeTheLoai.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _sachService.GetTongSachTheoTheLoai();
            chartTyLeTheLoai.Series[0].Points.Clear();

            foreach (var item in tongSachTheoTheLoai)
            {
                var point = new SeriesPoint(item.Key, item.Value);
                chartTyLeTheLoai.Series[0].Points.Add(point);
                chartTyLeTheLoai.Series[0].Name = item.Key;
            }

            // Đặt tên của trục x và y
            chartTyLeTheLoai.Series[0].ArgumentDataMember = "Argument"; // Trục x (Thể loại)
            chartTyLeTheLoai.Series[0].ValueDataMembers.AddRange("Value"); // Trục y (Số lượng)

            // Đặt tên cho series (Legend)

            // Hiển thị chú thích (Legend)
            chartTyLeTheLoai.Series[0].ShowInLegend = true;
            chartTyLeTheLoai.Series[0].LegendTextPattern = "{A}"; // Hiển thị item.key trong chú thích

            // Cập nhật dữ liệu biểu đồ




        }
        public void FillChartLine()
        {
            chartDocGiaTheoThang.Titles.Clear(); // Xóa các tiêu đề hiện có (nếu có)
            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Số Lương Thể Loại Các Loại Sách Được Mượn"; // Đặt nội dung tiêu đề
            chartDocGiaTheoThang.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _sachService.GetBookCategoryStatistics(7,2023);
            chartDocGiaTheoThang.Series[0].Points.Clear();

            foreach (var item in tongSachTheoTheLoai)
            {
                var point = new SeriesPoint(item.Key, item.Value);
                chartDocGiaTheoThang.Series[0].Points.Add(point);
               
            }
        }
        public void FillChart()
        {
            chartDanhSoTheoTheLoai.Titles.Clear(); // Xóa các tiêu đề hiện có (nếu có)

            var chartTitle = new ChartTitle();
            chartTitle.Text = "Biểu Đồ Số Sách Mượn Qua Các Tháng"; // Đặt nội dung tiêu đề
            chartDanhSoTheoTheLoai.Titles.Add(chartTitle); // Thêm tiêu đề vào biểu đồ

            var tongSachTheoTheLoai = _phieumuon_sachService.GetSoLuongSachTrongThang(7,2023);
            chartDanhSoTheoTheLoai.Series[0].Points.Clear();

                var point = new SeriesPoint(tongSachTheoTheLoai, tongSachTheoTheLoai);
                chartDanhSoTheoTheLoai.Series[0].Points.Add(point);
                chartDanhSoTheoTheLoai.Series[0].Name = "Biểu Đồ \nĐộc Giả Mượn Sách \nTheo Tháng";

            ((XYDiagram)chartDanhSoTheoTheLoai.Diagram).AxisX.NumericScaleOptions.AutoGrid = false;
            ((XYDiagram)chartDanhSoTheoTheLoai.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1;
            
        }

        private void chartTyLeTheLoai_Click(object sender, EventArgs e)
        {

        }

        private void labelControl22_Click(object sender, EventArgs e)
        {

        }
    }
}