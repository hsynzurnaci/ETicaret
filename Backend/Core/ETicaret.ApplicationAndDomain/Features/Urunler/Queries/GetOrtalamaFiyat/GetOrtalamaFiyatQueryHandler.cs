using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Queries.GetOrtalamaFiyat
{
    public class GetOrtalamaFiyatQueryHandler : IRequestHandler<GetOrtalamaFiyatQuery, decimal>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public GetOrtalamaFiyatQueryHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }



        public async Task<decimal> Handle(GetOrtalamaFiyatQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                var sql = "SELECT fn_kategori_ortalama_fiyat(@Kategori)";
                return await connection.ExecuteScalarAsync<decimal>(sql, new { Kategori = request.Kategori });
            }
        }
    }
}