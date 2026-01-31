using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Urunler.Commands.UpdateFiyatlar
{
    public class UpdateFiyatlarCommandHandler : IRequestHandler<UpdateFiyatlarCommand, string>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public UpdateFiyatlarCommandHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> Handle(UpdateFiyatlarCommand request, CancellationToken cancellationToken)
        {
            string sql = "CALL sp_fiyatlari_guncelle_cursor()";

            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql);
                return "Fiyat güncelleme işlemi (Cursor) başarıyla tetiklendi.";
            }
        }
    }
}