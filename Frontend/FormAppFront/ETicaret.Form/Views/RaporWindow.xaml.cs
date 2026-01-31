using ETicaret.UI.Models;
using Microsoft.Reporting.WinForms; // Artık burada hata vermez
using System.Collections.Generic;
using System.Windows;

namespace ETicaret.UI.Views
{
    public partial class RaporWindow : Window
    {
        // Değişkeni burada tanımlıyoruz
        private ReportViewer rptViewer;

        public RaporWindow(List<RaporModel> veriler)
        {
            InitializeComponent();

            // 1. Görüntüleyiciyi kodla oluştur
            rptViewer = new ReportViewer();
            rptViewer.Dock = System.Windows.Forms.DockStyle.Fill;

            // 2. XAML'daki Host kutusunun içine koy
            RaporHost.Child = rptViewer;

            // 3. Rapor Ayarları (Aynı mantık devam ediyor)
            rptViewer.LocalReport.ReportPath = "StokRaporu.rdlc";
            rptViewer.LocalReport.DataSources.Clear();

            var rds = new ReportDataSource("DataSet1", veriler);
            rptViewer.LocalReport.DataSources.Add(rds);

            rptViewer.SetDisplayMode(DisplayMode.PrintLayout);
            rptViewer.ZoomMode = ZoomMode.Percent;
            rptViewer.ZoomPercent = 100;

            rptViewer.RefreshReport();
        }
    }
}