using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.CreateUrun
{
    public class CreateUrunCommandHandler : IRequestHandler<CreateUrunCommand, bool>
    {
        private readonly IApplicationDbConnection _dbConnection;
        public CreateUrunCommandHandler(IApplicationDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<bool> Handle(CreateUrunCommand request, CancellationToken cancellationToken)
        {
            using (var conn = _dbConnection.CreateConnection())
            {
                var sql = "CALL UrunEkle(@UrunAdi, @Kategori, @Fiyat, @SaticiId, @StokAdedi)";
                await conn.ExecuteAsync(sql, request);
                return true;
            }
        }
    }
}