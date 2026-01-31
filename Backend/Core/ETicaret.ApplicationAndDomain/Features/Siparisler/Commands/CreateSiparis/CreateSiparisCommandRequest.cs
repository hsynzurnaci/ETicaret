using MediatR;
using System.Collections.Generic;

namespace ETicaret.ApplicationAndDomain.Features.Siparisler.Commands.CreateSiparis
{
    public class SiparisUrunDto
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int Miktar { get; set; }
        public decimal BirimFiyat { get; set; }

        public class CreateSiparisCommandRequest : IRequest<bool>
        {
            public int MusteriId { get; set; }
            public decimal ToplamTutar { get; set; }
            public List<SiparisUrunDto> Sepet { get; set; }
        }
    }
}