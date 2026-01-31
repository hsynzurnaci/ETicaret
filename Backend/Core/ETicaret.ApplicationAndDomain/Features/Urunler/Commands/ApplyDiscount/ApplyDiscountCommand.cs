using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.ApplyDiscount
{
    public class ApplyDiscountCommand : IRequest<bool>
    {
        public string Kategori { get; set; }
        public int Oran { get; set; }
    }
}