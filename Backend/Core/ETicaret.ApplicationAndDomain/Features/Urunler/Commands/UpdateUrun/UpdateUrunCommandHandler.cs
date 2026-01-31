using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.UpdateUrun
{
    public class UpdateUrunCommandHandler : IRequestHandler<UpdateUrunCommand, bool>
    {
        private readonly IApplicationDbConnection _dbConnection;
        public UpdateUrunCommandHandler(IApplicationDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<bool> Handle(UpdateUrunCommand request, CancellationToken cancellationToken)
        {
            using (var conn = _dbConnection.CreateConnection())
            {
                var sql = "UPDATE urun SET urun_adi=@UrunAdi, kategori=@Kategori, fiyat=@Fiyat,  stok_adedi=@StokAdedi  WHERE urun_id=@UrunId";
                var rows = await conn.ExecuteAsync(sql, request);
                return rows > 0;
            }
        }
    }
}