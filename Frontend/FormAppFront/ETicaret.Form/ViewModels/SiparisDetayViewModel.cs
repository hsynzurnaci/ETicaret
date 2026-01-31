using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ETicaret.UI.Models;
using ETicaret.UI.Services;
using System.Threading.Tasks;
using System.Windows;

namespace ETicaret.UI.ViewModels
{
    public partial class SiparisDetayViewModel : ObservableObject
    {
        private readonly ApiService _apiService;


        [ObservableProperty]
        private string _siparisIdText = "1"; 
        [ObservableProperty]
        private SiparisMasterModel _siparisDetay;

        public SiparisDetayViewModel()
        {
            _apiService = new ApiService();
        }

        [RelayCommand]
        public async Task Getir()
        {
            if (int.TryParse(SiparisIdText, out int id))
            {
                string url = $"Siparisler/GetDetay/{id}/Detay";
                var data = await _apiService.GetAsync<SiparisMasterModel>(url);

                if (data != null)
                {
                    SiparisDetay = data;
                }
                else
                {
                    System.Windows.MessageBox.Show("Bu ID'ye ait sipariş bulunamadı veya veri boş.");
                    SiparisDetay = null;
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Lütfen geçerli bir sayı giriniz.");
            }
        }
    }
}