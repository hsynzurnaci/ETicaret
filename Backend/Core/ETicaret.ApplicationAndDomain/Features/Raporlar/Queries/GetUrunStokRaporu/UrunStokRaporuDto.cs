namespace ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.GetUrunStokRaporu
{

    public class UrunStokRaporuDto
    {
        public int UrunId { get; set; } 
        public string UrunAdi { get; set; }   
        public string Kategori { get; set; }  
        public decimal Fiyat { get; set; } 
        public int StokAdedi { get; set; }  
        public string SaticiAdi { get; set; } 
        public decimal ToplamStokDegeri { get; set; }
    }
}