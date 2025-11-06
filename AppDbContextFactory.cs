using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

// 🛑 IMPORTANTE: Confirme se o namespace é este mesmo!
namespace MotoMonitoramento.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // O EF Core (em design-time) usa esta fábrica para ler a string de conexão
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                // O .AddJsonFile deve apontar para o appsettings que contém sua Connection String
                .AddJsonFile("appsettings.json")
                .Build();

            // Puxa a string de conexão com a chave "OracleConnection"
            var connectionString = configuration.GetConnectionString("OracleConnection");

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            // Configura o provedor de banco de dados Oracle
            builder.UseOracle(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}