using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ETicaret.UI.Models;
using ETicaret.UI.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ETicaret.UI.ViewModels
{
    public partial class UrunlerViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<UrunModel> _urunler;

        [ObservableProperty]
        private ObservableCollection<string> _kategoriler;
        [ObservableProperty]
        private string _secilenKategori;

        partial void OnSecilenKategoriChanged(string value)
        {
            LoadUrunlerCommand.Execute(null);
        }
        [ObservableProperty]
        private string _aramaMetni;
        [ObservableProperty]
        private string _minFiyat;
        [ObservableProperty]
        private string _maxFiyat;

        [ObservableProperty]
        private string _ortalamaFiyatMetni; 




        [ObservableProperty]
        private UrunModel _formUrun;
        [ObservableProperty]
        private UrunModel _secilenSatir;
        partial void OnSecilenSatirChanged(UrunModel value)
        {
            if (value != null)
            {
                FormUrun = new UrunModel
                {
                    UrunId = value.UrunId,
                    UrunAdi = value.UrunAdi,
                    Kategori = value.Kategori,
                    Fiyat = value.Fiyat,
                    SaticiId = value.SaticiId
                };
            }
        }

        public ICommand OpenIndirimDialogCommand { get; }



        public UrunlerViewModel()
        {
            _apiService = new ApiService();
            Urunler = new ObservableCollection<UrunModel>();
            Kategoriler = new ObservableCollection<string>();
            OpenIndirimDialogCommand = new RelayCommand(async () => await IndirimUygulaAsync());
            LoadData();
            FormUrun = new UrunModel(); 
        }

        private async Task IndirimUygulaAsync()
        {
            var kategoriListesi = new List<string>(Kategoriler);
            kategoriListesi.Remove("Tümü"); 
            var window = new Views.IndirimWindow(kategoriListesi);
            bool? sonuc = window.ShowDialog();

            if (sonuc == true)
            {
                string kategori = window.SecilenKategori;
                int oran = window.GirilenOran;

                var cevap = System.Windows.MessageBox.Show(
                    $"{kategori} kategorisine %{oran} indirim yapılacak.\nOnaylıyor musunuz?",
                    "Kampanya Onayı",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (cevap == MessageBoxResult.Yes)
                {
                    var veri = new
                    {
                        Kategori = kategori,
                        Oran = oran
                    };
                    bool basarili = await _apiService.PostAsync("Urunler/KategoriIndirimi", veri);

                    if (basarili)
                    {
                        System.Windows.MessageBox.Show("Kampanya Başarıyla Uygulandı!");
                        await LoadUrunler(); 
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Bir hata oluştu, işlem yapılamadı.");
                    }
                }
            }
        }

        [RelayCommand]
        public void YeniKayit()
        {
            SecilenSatir = null; 
            FormUrun = new UrunModel(); 
        }

        [RelayCommand]
        public async Task Kaydet()
        {
            if (string.IsNullOrEmpty(FormUrun.UrunAdi))
            {
                System.Windows.MessageBox.Show("Ürün adı boş olamaz!"); return;
            }

            bool basarili = false;

            if (FormUrun.UrunId == 0)
            {
                var command = new
                {
                    UrunAdi = FormUrun.UrunAdi,
                    Kategori = FormUrun.Kategori,
                    Fiyat = FormUrun.Fiyat,
                    SaticiId = 1,
                    StokAdedi = FormUrun.StokAdedi
                };
                basarili = await _apiService.PostAsync("Urunler/Ekle/Ekle", command);
            }
            else
            {
                var command = new
                {
                    UrunId = FormUrun.UrunId,
                    UrunAdi = FormUrun.UrunAdi,
                    Kategori = FormUrun.Kategori,
                    Fiyat = FormUrun.Fiyat,
                    StokAdedi = FormUrun.StokAdedi
                };
                basarili = await _apiService.PutAsync("Urunler/Guncelle/Guncelle", command);
            }

            if (basarili)
            {
                System.Windows.MessageBox.Show("İşlem Başarılı!");
                await LoadUrunler(); 
                YeniKayit(); 
            }
            else
            {
                System.Windows.MessageBox.Show("Hata oluştu!");
            }
        }


        private async void LoadData()
        {
            await LoadKategoriler(); 
            await LoadUrunler(); 
        }
        public class KategoriDto
        {
            public string KategoriAdi { get; set; }
        }
        public async Task LoadKategoriler()
        {
            var data = await _apiService.GetAsync<List<KategoriDto>>("Urunler/GetKategoriler/Kategoriler");

            if (data != null)
            {
                Kategoriler.Clear();
                Kategoriler.Add("Tümü");

                foreach (var item in data)
                {
                    Kategoriler.Add(item.KategoriAdi);
                }

                SecilenKategori = "Tümü";
            }
        }

        [RelayCommand]
        public void AnalizEkle(int urunId)
        {
            var window = new Views.AnalizEkleWindow(urunId);
            window.ShowDialog();
        }

        [RelayCommand] 
        public async Task LoadUrunler() 
        {

            string url = "Urunler/Filtrele/Filtrele?";
            if (!string.IsNullOrWhiteSpace(SecilenKategori) && SecilenKategori != "Tümü")
            {
                url += $"Kategori={SecilenKategori.Trim()}&";
            }
            if (!string.IsNullOrWhiteSpace(AramaMetni))
                url += $"AramaMetni={AramaMetni.Trim()}&";

            if (decimal.TryParse(MinFiyat, out decimal min))
                url += $"MinFiyat={min}&";

            if (decimal.TryParse(MaxFiyat, out decimal max))
                url += $"MaxFiyat={max}&";

            if (!string.IsNullOrEmpty(SecilenKategori) && SecilenKategori != "Tümü")
            {
                string ortUrl = $"Urunler/GetOrtalamaFiyat/OrtalamaFiyat?kategori={SecilenKategori}";
                decimal ortalama = await _apiService.GetAsync<decimal>(ortUrl);

                OrtalamaFiyatMetni = $"Ortalama {SecilenKategori} Fiyatı: {ortalama:C2}";
            }
            else
            {
                OrtalamaFiyatMetni = "Kategori Seçin. (Ortalama Fiyat)"; 
            }

            var data = await _apiService.GetAsync<List<UrunModel>>(url);

            if (data != null)
            {
                Urunler.Clear();
                foreach (var urun in data)
                {
                    Urunler.Add(urun);
                }
            }
        }
        [RelayCommand]
        public async Task FiyatGuncelle()
        {
            var result = System.Windows.MessageBox.Show("Piyasa analizine göre fiyatlar güncellenecek. \nOnaylıyor musunuz?",
                                         "Fiyat Güncelleme", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool basarili = await _apiService.PostAsync("Urunler/UpdateFiyatlar/FiyatlariGuncelle", new { });

                if (basarili)
                {
                    System.Windows.MessageBox.Show("Fiyatlar başarıyla güncellendi!", "Başarılı");
                    await LoadUrunler(); 
                }
                else
                {
                    System.Windows.MessageBox.Show("İşlem sırasında hata oluştu.", "Hata");
                }
            }
        }
    }
}