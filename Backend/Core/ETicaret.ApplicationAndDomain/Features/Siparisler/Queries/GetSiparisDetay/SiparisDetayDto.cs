namespace ETicaret.ApplicationAndDomain.Features.Siparisler.Queries.GetSiparisDetay
{

    public class SiparisMasterDto
    {
        public int SiparisId { get; set; }
        public DateTime Tarih { get; set; }
        public decimal ToplamTutar { get; set; }
        public string MusteriAdiSoyadi { get; set; } 
        public List<SiparisItemDto> Urunler { get; set; } = new();
    }

    public class SiparisItemDto
    {
        public string UrunAdi { get; set; }
        public int Miktar { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal SatirToplami => Miktar * BirimFiyat;
    }
}