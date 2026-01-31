using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Kategoriler.Queries.GetAllKategoriler
{
    public class GetAllKategorilerQueryHandler : IRequestHandler<GetAllKategorilerQuery, List<KategoriDto>>
    {
        private readonly IApplicationDbConnection _dbConnection;
        public GetAllKategorilerQueryHandler(IApplicationDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<List<KategoriDto>> Handle(GetAllKategorilerQuery request, CancellationToken cancellationToken)
        {
      
            var sql = "SELECT DISTINCT kategori AS KategoriAdi FROM urun ORDER BY kategori";

            using (var connection = _dbConnection.CreateConnection())
            {
                return (await connection.QueryAsync<KategoriDto>(sql)).ToList();
            }
        }
    }
}
