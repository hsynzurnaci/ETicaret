using Newtonsoft.Json; 
using System;
using System.Collections.Generic;

namespace ETicaret.UI.Models
{
    public class SiparisItemModel
    {
        [JsonProperty("urunAdi")]
        public string UrunAdi { get; set; }

        [JsonProperty("miktar")]
        public int Miktar { get; set; }

        [JsonProperty("birimFiyat")]
        public decimal BirimFiyat { get; set; }

        [JsonProperty("satirToplami")]
        public decimal SatirToplami { get; set; }

        public string BirimFiyatFormatli => $"{BirimFiyat:N2} ₺";
        public string SatirToplamiFormatli => $"{SatirToplami:N2} ₺";
    }

    public class SiparisMasterModel
    {
        [JsonProperty("siparisId")]
        public int SiparisId { get; set; }

        [JsonProperty("tarih")]
        public DateTime Tarih { get; set; }

        [JsonProperty("toplamTutar")]
        public decimal ToplamTutar { get; set; }

        [JsonProperty("musteriAdiSoyadi")]
        public string MusteriAdiSoyadi { get; set; }

        [JsonProperty("urunler")]
        public List<SiparisItemModel> Urunler { get; set; } = new List<SiparisItemModel>();

        public string ToplamTutarFormatli => $"{ToplamTutar:N2} ₺";
    }
}