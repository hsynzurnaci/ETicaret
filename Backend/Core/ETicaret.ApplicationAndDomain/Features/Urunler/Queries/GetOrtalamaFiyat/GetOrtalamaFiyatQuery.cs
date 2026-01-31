using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Queries.GetOrtalamaFiyat
{
    public class GetOrtalamaFiyatQuery : IRequest<decimal>
    {
        public string Kategori { get; set; }
    }
}