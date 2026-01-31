using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.RunCustomSql
{
    public class RunCustomSqlQueryHandler : IRequestHandler<RunCustomSqlQuery, List<object>>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public RunCustomSqlQueryHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<object>> Handle(RunCustomSqlQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _dbConnection.CreateConnection())
            {
                var result = await conn.QueryAsync(request.SqlSorgusu);
                return result.Select(x => (object)x).ToList();
            }
        }
    }
}