using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.GetUrunStokRaporu
{
    public class GetUrunStokRaporuQuery : IRequest<List<UrunStokRaporuDto>>
    {
    }
}