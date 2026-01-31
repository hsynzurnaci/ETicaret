using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Siparisler.Queries.GetSiparisDetay
{
    public class GetSiparisDetayQuery : IRequest<SiparisMasterDto>
    {
        public int SiparisId { get; set; } 
    }
}