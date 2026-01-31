using MediatR;
namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.UpdateUrun
{
    public class UpdateUrunCommand : IRequest<bool>
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public string Kategori { get; set; }
        public int StokAdedi { get; set; }
        public decimal Fiyat { get; set; }
    }
}