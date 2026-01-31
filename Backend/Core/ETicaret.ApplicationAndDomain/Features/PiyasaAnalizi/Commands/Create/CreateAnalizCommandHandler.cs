using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaret.ApplicationAndDomain.Features.PiyasaAnalizi.Commands.Create
{
    public class CreateAnalizCommandHandler : IRequestHandler<CreateAnalizCommand, bool>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public CreateAnalizCommandHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Handle(CreateAnalizCommand request, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
           
                        var sqlAnaliz = @"
                            INSERT INTO piyasa_analizi (tarih, ekonomik_durum, fiyat_onerisi, notlar)
                            VALUES (CURRENT_DATE, @EkonomikDurum, @FiyatOnerisi, @Notlar)
                            RETURNING analiz_id";

                        int analizId = await connection.ExecuteScalarAsync<int>(sqlAnaliz, request, transaction);


                        var sqlEslesme = @"
                            INSERT INTO fiyatlandirma (urun_id, piyasa_analiz_id)
                            VALUES (@UrunId, @AnalizId)";

                        await connection.ExecuteAsync(sqlEslesme, new { request.UrunId, AnalizId = analizId }, transaction);

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}