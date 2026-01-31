using MediatR;
using System;

namespace ETicaret.ApplicationAndDomain.Features.PiyasaAnalizi.Commands.Create
{
    public class CreateAnalizCommand : IRequest<bool>
    {
        public string EkonomikDurum { get; set; }
        public decimal FiyatOnerisi { get; set; }
        public string Notlar { get; set; }
        public int UrunId { get; set; }
    }
}