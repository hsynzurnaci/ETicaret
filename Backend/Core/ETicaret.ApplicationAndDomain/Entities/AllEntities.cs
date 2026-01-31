using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Entities
{
    public class Musteri
    {
        public int MusteriId { get; set; }      //
        public string Isim { get; set; }        //
        public string Soyisim { get; set; }     //
        public string Eposta { get; set; }      //
        public string Telefon { get; set; }     //
        public decimal Bakiye { get; set; }     //
    }

    // Kampanya tablosu karşılığı
    public class Kampanya
    {
        public int KampanyaId { get; set; }           //
        public DateTime BaslangicTarihi { get; set; } //
        public DateTime BitisTarihi { get; set; }     //
        public decimal IndirimOrani { get; set; }     //
    }

    // Piyasa Analizi tablosu karşılığı
    public class PiyasaAnalizi
    {
        public int AnalizId { get; set; }             //
        public DateTime Tarih { get; set; }           //
        public string EkonomikDurum { get; set; }     //
        public decimal FiyatOnerisi { get; set; }     //
    }

    // Satıcı tablosu karşılığı (Müşteri ile ilişkili)
    public class Satici
    {
        public int SaticiId { get; set; }    //
        public int MusteriId { get; set; }   // FK
        public string SaticiAdi { get; set; } //
        public string Baglanti { get; set; }  //
    }

    // Portföy tablosu karşılığı
    public class Portfoy
    {
        public int PortfoyId { get; set; }    //
        public int MusteriId { get; set; }    // FK
        public decimal VarlikDegeri { get; set; } //
    }

    // Ürün tablosu karşılığı
    public class Urun
    {
        public int UrunId { get; set; }       //
        public string UrunAdi { get; set; }   //
        public string Kategori { get; set; }  //
        public decimal Fiyat { get; set; }    //
        public int? SaticiId { get; set; }    // FK (Nullable olabilir)
    }

    // Stok tablosu karşılığı
    public class Stok
    {
        public int StokId { get; set; }   //
        public int UrunId { get; set; }   // FK & Unique
        public int Miktar { get; set; }   //
    }

    // Sipariş Başlık tablosu karşılığı
    public class Siparis
    {
        public int SiparisId { get; set; }        //
        public int MusteriId { get; set; }        // FK
        public DateTime Tarih { get; set; }       //
        public decimal ToplamTutar { get; set; }  //
    }

    // Sipariş Detay tablosu karşılığı (Ara Tablo)
    public class SiparisDetay
    {
        public int SiparisId { get; set; } // Composite Key Part 1
        public int UrunId { get; set; }    // Composite Key Part 2
        public int Miktar { get; set; }    //
        public decimal SatisFiyati { get; set; }
    }

    // Fiyatlandırma tablosu karşılığı
    public class Fiyatlandirma
    {
        public int FiyatId { get; set; }          //
        public int UrunId { get; set; }           // FK
        public int PiyasaAnalizId { get; set; }   // FK
        public decimal BelirlenenFiyat { get; set; }
    }

    // Ürün Kampanya İlişkisi (Ara Tablo)
    public class UrunKampanya
    {
        public int UrunId { get; set; }
        public int KampanyaId { get; set; }
    }

    // İşlem Geçmişi tablosu karşılığı
    public class IslemGecmisi
    {
        public int GecmisId { get; set; }   //
        public int MusteriId { get; set; }  // FK
        public int? SiparisId { get; set; } // FK (Nullable)
        public DateTime Tarih { get; set; } //
    }
}
