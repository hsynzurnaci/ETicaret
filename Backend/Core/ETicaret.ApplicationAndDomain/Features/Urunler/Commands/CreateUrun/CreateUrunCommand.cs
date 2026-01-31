using MediatR;
namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.CreateUrun
{
    public class CreateUrunCommand : IRequest<bool>
    {
        public string UrunAdi { get; set; }
        public string Kategori { get; set; }
        public decimal Fiyat { get; set; }
        public int StokAdedi { get; set; }
        public int SaticiId { get; set; } 
    }
}