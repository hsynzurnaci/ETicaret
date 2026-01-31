using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ETicaret.UI.Models;
using ETicaret.UI.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ETicaret.UI.ViewModels
{
    public partial class RaporlarViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<RaporModel> _raporListesi;

        public RaporlarViewModel()
        {
            _apiService = new ApiService();
            RaporListesi = new ObservableCollection<RaporModel>();
            LoadRapor();
        }

        [RelayCommand]
        public async Task LoadRapor()
        {
            string url = "Raporlar/GetUrunStokRaporu/UrunStok";
            var data = await _apiService.GetAsync<List<RaporModel>>(url);

            if (data != null)
            {
                RaporListesi.Clear();
                foreach (var item in data) RaporListesi.Add(item);
            }
        }

        [RelayCommand]
        public void RaporYazdir()
        {
            if (RaporListesi != null && RaporListesi.Count > 0)
            {
                var pencere = new Views.RaporWindow(RaporListesi.ToList());
                pencere.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Yazdırılacak veri yok! Önce listeyi yenileyin.");
            }
        }
    }
}