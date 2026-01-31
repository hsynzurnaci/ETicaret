using ETicaret.ApplicationAndDomain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Abstractions
{
    public class ApplicationDbConnection : IApplicationDbConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ApplicationDbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            // appsettings.json dosyasındaki "DefaultConnection" alanını okur
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Her çağrıldığında yeni bir bağlantı nesnesi döner.
        // using bloğu içinde kullanıldığında işi bitince otomatik kapanır.
        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
