using System.Threading.Tasks;
using System.Windows;

namespace ETicaret.UI.Views
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            StartLoading(); // Yüklemeyi başlat
        }

        private async void StartLoading()
        {
            // 3 Saniye bekle (Burada veritabanı kontrolü vs. yapılabilir)
            await Task.Delay(3000);

            // Ana Ekranı Aç
            MainWindow main = new MainWindow();
            main.Show();

            // Splash Ekranını Kapat
            this.Close();
        }
    }
}