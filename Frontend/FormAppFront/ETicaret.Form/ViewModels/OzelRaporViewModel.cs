using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ETicaret.UI.Services;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace ETicaret.UI.ViewModels
{
    public partial class OzelRaporViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private string _sqlSorgusu;
        public string SqlSorgusu
        {
            get => _sqlSorgusu;
            set => SetProperty(ref _sqlSorgusu, value);
        }

        private DataTable _raporSonucu;
        public DataTable RaporSonucu
        {
            get => _raporSonucu;
            set => SetProperty(ref _raporSonucu, value);
        }

        public OzelRaporViewModel()
        {
            _apiService = new ApiService();
            SqlSorgusu = "SELECT * FROM urun ORDER BY fiyat DESC";
        }

        [RelayCommand]
        public async Task SorguyuCalistir()
        {
            if (string.IsNullOrWhiteSpace(SqlSorgusu)) return;

            var request = new { SqlSorgusu = SqlSorgusu };

            try
            {
                var jsonString = await _apiService.PostAndGetStringAsync("Raporlar/Calistir/OzelRapor", request);

                if (!string.IsNullOrEmpty(jsonString))
                {
                    var tablo = JsonConvert.DeserializeObject<DataTable>(jsonString);
                    RaporSonucu = tablo;
                    System.Windows.MessageBox.Show($"{tablo.Rows.Count} satır veri getirildi.");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"HATA: {ex.Message}\n\nSorgunuzu kontrol edin.");
            }
        }
    }
}