using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.GetUrunStokRaporu
{
    public class GetUrunStokRaporuQueryHandler : IRequestHandler<GetUrunStokRaporuQuery, List<UrunStokRaporuDto>>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public GetUrunStokRaporuQueryHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<List<UrunStokRaporuDto>> Handle(GetUrunStokRaporuQuery request, CancellationToken cancellationToken)
        {
            string sql = @"
                SELECT 
                    urun_id AS UrunId,
                    urun_adi AS UrunAdi,
                    kategori AS Kategori,
                    fiyat AS Fiyat,
                    stok_adedi AS StokAdedi,
                    satici_adi AS SaticiAdi,
                    toplam_stok_degeri AS ToplamStokDegeri
                FROM vw_urun_stok_raporu";

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<UrunStokRaporuDto>(sql);
                return result.ToList();
            }
        }
    }
}