using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.ApplyDiscount
{
    public class ApplyDiscountCommandHandler : IRequestHandler<ApplyDiscountCommand, bool>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public ApplyDiscountCommandHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Handle(ApplyDiscountCommand request, CancellationToken cancellationToken)
        {
            using (var conn = _dbConnection.CreateConnection())
            {
                var sql = "CALL KategoriIndirimiYap(@KategoriAdi, @IndirimOrani)";
                await conn.ExecuteAsync(sql, new
                {
                    KategoriAdi = request.Kategori,
                    IndirimOrani = request.Oran
                });

                return true;
            }
        }
    }
}