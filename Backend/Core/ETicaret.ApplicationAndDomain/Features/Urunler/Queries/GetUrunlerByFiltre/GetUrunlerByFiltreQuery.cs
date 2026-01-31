using ETicaret.ApplicationAndDomain.Entities;
using MediatR;

public class GetUrunlerByFiltreQuery : IRequest<List<Urun>>
{
    public string? AramaMetni { get; set; }
    public decimal? MinFiyat { get; set; }
    public decimal? MaxFiyat { get; set; }


    public string? Kategori { get; set; }
}