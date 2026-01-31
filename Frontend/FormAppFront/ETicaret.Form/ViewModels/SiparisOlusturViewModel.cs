using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ETicaret.UI.Models;
using ETicaret.UI.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;

namespace ETicaret.UI.ViewModels
{
    public partial class SiparisOlusturViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        [ObservableProperty]
        private ObservableCollection<UrunModel> _urunListesi;

        [ObservableProperty]
        private UrunModel _secilenUrun;

        [ObservableProperty]
        private ObservableCollection<SepetItemModel> _sepet;

        [ObservableProperty]
        private string _musteriId = "1";

        [ObservableProperty]
        private string _miktar = "1";

        public string GenelToplamFormatli => $"Toplam: {Sepet.Sum(x => x.Toplam):C2}";

        public SiparisOlusturViewModel()
        {
            _apiService = new ApiService();
            UrunListesi = new ObservableCollection<UrunModel>();
            Sepet = new ObservableCollection<SepetItemModel>();

            _ = LoadUrunler();
        }

        [RelayCommand] 
        public async Task LoadUrunler()
        {
            var data = await _apiService.GetAsync<List<UrunModel>>("Urunler/Filtrele/Filtrele");

            if (data != null)
            {
                UrunListesi.Clear(); 
                foreach (var item in data)
                {
                    UrunListesi.Add(item);
                }
            }
        }

        [RelayCommand]
        public void SepeteEkle()
        {
            if (SecilenUrun != null && int.TryParse(Miktar, out int adet) && adet > 0)
            {
                Sepet.Add(new SepetItemModel
                {
                    UrunId = SecilenUrun.UrunId,
                    UrunAdi = SecilenUrun.UrunAdi,
                    Miktar = adet,
                    BirimFiyat = SecilenUrun.Fiyat
                });

                OnPropertyChanged(nameof(GenelToplamFormatli));
            }
            else
            {
                System.Windows.MessageBox.Show("Lütfen ürün seçin ve geçerli bir miktar girin.");
            }
        }
        [RelayCommand]
        public void SepetiTemizle()
        {
 
            if (Sepet.Count == 0) return;

            var cevap = System.Windows.MessageBox.Show("Sepetteki tüm ürünler silinecek. Emin misiniz?",
                                        "Sepeti Temizle",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Warning);

            if (cevap == MessageBoxResult.Yes)
            {
                Sepet.Clear();
                OnPropertyChanged(nameof(GenelToplamFormatli));
            }
        }
        [RelayCommand]
        public async Task SiparisiTamamla()
        {
            if (Sepet.Count == 0) return;

            var request = new CreateSiparisRequest
            {
                MusteriId = int.Parse(MusteriId),
                ToplamTutar = Sepet.Sum(x => x.Toplam),
                Sepet = Sepet.ToList()
            };

            bool sonuc = await _apiService.PostAsync("Siparisler/Create", request);

            if (sonuc)
            {
                System.Windows.MessageBox.Show("Sipariş başarıyla oluşturuldu! ✅");
                Sepet.Clear();
                OnPropertyChanged(nameof(GenelToplamFormatli));
            }
            else
            {
                System.Windows.MessageBox.Show("Hata oluştu. ❌");
            }
        }
    }
}