using Dapper;
using ETicaret.ApplicationAndDomain.Interfaces;
using MediatR;

namespace ETicaret.ApplicationAndDomain.Features.Siparisler.Queries.GetSiparisDetay
{
    public class GetSiparisDetayQueryHandler : IRequestHandler<GetSiparisDetayQuery, SiparisMasterDto>
    {
        private readonly IApplicationDbConnection _dbConnection;

        public GetSiparisDetayQueryHandler(IApplicationDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<SiparisMasterDto> Handle(GetSiparisDetayQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                connection.Open();

         
                var sqlMaster = @"
                    SELECT 
                        s.siparis_id AS SiparisId,
                        s.tarih AS Tarih,
                        s.toplam_tutar AS ToplamTutar,
                        CONCAT(m.isim, ' ', m.soyisim) AS MusteriAdiSoyadi
                    FROM siparis s
                    JOIN musteri m ON s.musteri_id = m.musteri_id
                    WHERE s.siparis_id = @Id";

                var master = await connection.QueryFirstOrDefaultAsync<SiparisMasterDto>(sqlMaster, new { Id = request.SiparisId });

                if (master == null) return null;

            
                var sqlDetail = @"
                    SELECT 
                        u.urun_adi AS UrunAdi,
                        sd.miktar AS Miktar,
                        sd.satis_fiyati AS BirimFiyat
                    FROM siparis_detay sd
                    JOIN urun u ON sd.urun_id = u.urun_id
                    WHERE sd.siparis_id = @Id";

                var details = await connection.QueryAsync<SiparisItemDto>(sqlDetail, new { Id = request.SiparisId });

    
                master.Urunler = details.ToList();

                return master;
            }
        }
    }
}