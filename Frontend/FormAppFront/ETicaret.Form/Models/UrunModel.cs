namespace ETicaret.UI.Models
{
    public class UrunModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public string Kategori { get; set; }
        public decimal Fiyat { get; set; }
        public int StokAdedi { get; set; }
        public int? SaticiId { get; set; }
        public string FiyatFormatli => $"{Fiyat:N2}  ₺";
    }
}