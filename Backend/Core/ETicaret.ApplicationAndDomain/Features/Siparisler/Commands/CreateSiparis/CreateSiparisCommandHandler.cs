using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static ETicaret.ApplicationAndDomain.Features.Siparisler.Commands.CreateSiparis.SiparisUrunDto;

namespace ETicaret.ApplicationAndDomain.Features.Siparisler.Commands.CreateSiparis
{
    public class CreateSiparisCommandHandler : IRequestHandler<CreateSiparisCommandRequest, bool>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public CreateSiparisCommandHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Handle(CreateSiparisCommandRequest request, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

            
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                 
                        var sqlSiparis = @"
                            INSERT INTO siparis (musteri_id, toplam_tutar, tarih) 
                            VALUES (@MusteriId, @ToplamTutar, NOW()) 
                            RETURNING siparis_id";

                        int yeniSiparisId = await connection.ExecuteScalarAsync<int>(sqlSiparis, new
                        {
                            request.MusteriId,
                            request.ToplamTutar
                        }, transaction);

                        var sqlDetay = @"
                            INSERT INTO siparis_detay (siparis_id, urun_id, miktar, satis_fiyati) 
                            VALUES (@SiparisId, @UrunId, @Miktar, @BirimFiyat)";

                        if (request.Sepet != null)
                        {
                            foreach (var urun in request.Sepet)
                            {
                                await connection.ExecuteAsync(sqlDetay, new
                                {
                                    SiparisId = yeniSiparisId,
                                    urun.UrunId,
                                    urun.Miktar,
                                    urun.BirimFiyat
                                }, transaction);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (System.Exception ex)
                    {
               transaction.Rollback();
                   
                        throw ex;
                    }
                }
            }
        }
    }
}