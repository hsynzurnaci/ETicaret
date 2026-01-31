using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Raporlar.Queries.RunCustomSql
{
    public class RunCustomSqlQuery : IRequest<List<object>>
    {
        public string SqlSorgusu { get; set; }
    }
}