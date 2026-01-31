namespace ETicaret.UI.Models
{
    public class RaporModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }


            public string Kategori { get; set; }
        public decimal Fiyat { get; set; }

        public int StokAdedi { get; set; }
        public string SaticiAdi { get; set; }
        public decimal ToplamStokDegeri { get; set; } 
        public decimal ToplamDeger => ToplamStokDegeri;
        public string FiyatFormatli => $"{Fiyat:N2} ₺";
        public string ToplamDegerFormatli => $"{ToplamStokDegeri:N2} ₺";
    }
}