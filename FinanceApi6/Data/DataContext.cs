using financeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace financeAPI.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var config = GetConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            string path = config["walletPath"];
           
            if (OracleConfiguration.TnsAdmin != path)
            {
                OracleConfiguration.TnsAdmin = path;
                OracleConfiguration.WalletLocation = path;
            }
            var option = optionsBuilder.UseOracle(config["ASPNETCORE_ConString"]);
            return new DataContext(optionsBuilder.Options, config);
        }

        IConfiguration GetConfiguration()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var dir = Directory.GetParent(AppContext.BaseDirectory);
            do
                dir = dir.Parent;
            while (dir.Name != "bin");
            dir = dir.Parent;
            var path = dir.FullName;
            
            var builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile($"appsettings.{environmentName}.json", true)
                    .AddUserSecrets("660fed9a-7275-44dc-9452-b13a29eee6f2")
                    .AddEnvironmentVariables();
            return builder.Build();
        }
    }

    public class DataContext:DbContext
    {
        private readonly IConfiguration _config;
        private readonly DbContextOptions<DataContext> _options;

        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            _options = options;
        }

        public DataContext(DbContextOptions<DataContext> options, IConfiguration config) : base(options)
        {
            _config = config;
            _options = options;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_TNSName") != null)
            {
                OracleConfiguration.OracleDataSources.Add(Environment.GetEnvironmentVariable("ASPNETCORE_TNSName"), Environment.GetEnvironmentVariable("ASPNETCORE_TNSConnectionString"));
                var option = optionsBuilder.UseOracle(Environment.GetEnvironmentVariable("ASPNETCORE_ConString"));
                base.OnConfiguring(option);
            }
            else if (_config["ASPNETCORE_ConString"]!=null)
            {
                OracleConfiguration.OracleDataSources.Add(_config["ASPNETCORE_TNSName"], _config["ASPNETCORE_TNSConnectionString"]);
                var option = optionsBuilder.UseOracle(_config["ASPNETCORE_ConString"]);
                base.OnConfiguring(option);
            }
        }

        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<CostCenter> CostCenter { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Models.Transaction> Transaction{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CostCenter>()
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

            modelBuilder.Entity<ChartOfAccounts>()
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

            modelBuilder.Entity<BankAccount>()
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

            modelBuilder.Entity<Supplier>()                            
                .HasIndex(p => new { p.Name })
                .IsUnique(true);

            modelBuilder.Entity<Supplier>()
                .HasIndex(p => new { p.Document })
                .IsUnique(true);

        }
    }
}
