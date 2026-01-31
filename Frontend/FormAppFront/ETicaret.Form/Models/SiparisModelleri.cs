using System.Collections.Generic;

namespace ETicaret.UI.Models
{
    public class SepetItemModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Toplam => Miktar * BirimFiyat;
    }
    public class CreateSiparisRequest
    {
        public int MusteriId { get; set; }
        public decimal ToplamTutar { get; set; }
        public List<SepetItemModel> Sepet { get; set; }
    }
}