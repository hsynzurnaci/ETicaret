using Dapper;
using ETicaret.ApplicationAndDomain.Entities;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Queries.GetUrunlerByFiltre
{
    public class GetUrunlerByFiltreQueryHandler : IRequestHandler<GetUrunlerByFiltreQuery, List<Urun>>
    {
        private readonly IApplicationDbConnection _dbConnection;
        public GetUrunlerByFiltreQueryHandler(IApplicationDbConnection dbConnection) => _dbConnection = dbConnection;

   

        public async Task<List<Urun>> Handle(GetUrunlerByFiltreQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnection.CreateConnection())
            {

                var sql = @"
            SELECT 
                urun_id AS UrunId, 
                urun_adi AS UrunAdi, 
                kategori AS Kategori, 
                fiyat AS Fiyat, 
                satici_id AS SaticiId 
            FROM urun 
            WHERE 1=1";


                if (!string.IsNullOrEmpty(request.AramaMetni))
                    sql += " AND urun_adi ILIKE '%' || @AramaMetni || '%'";


                if (!string.IsNullOrEmpty(request.Kategori) && request.Kategori != "Tümü")
                {
                    sql += " AND kategori = @Kategori";
                }


                if (request.MinFiyat.HasValue)
                    sql += " AND fiyat >= @MinFiyat";

                if (request.MaxFiyat.HasValue)
                    sql += " AND fiyat <= @MaxFiyat";

                return (await connection.QueryAsync<Urun>(sql, request)).ToList();
            }
        }
    }
}