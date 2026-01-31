using ETicaret.UI.Services;
using System.Windows;

namespace ETicaret.UI.Views
{
    public partial class AnalizEkleWindow : Window
    {
        private readonly ApiService _apiService;
        private readonly int _urunId;

        public AnalizEkleWindow(int urunId)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _urunId = urunId;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtFiyat.Text, out decimal fiyat))
            {
                var command = new
                {
                    EkonomikDurum = txtDurum.Text,
                    FiyatOnerisi = fiyat,
                    Notlar = txtNotlar.Text,
                    UrunId = _urunId
                };

                bool sonuc = await _apiService.PostAsync("PiyasaAnalizi/Ekle/Ekle", command);

                if (sonuc)
                {
                    System.Windows.MessageBox.Show("Analiz eklendi!");
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Hata oluştu.");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Lütfen geçerli bir fiyat girin.");
            }
        }
    }
}