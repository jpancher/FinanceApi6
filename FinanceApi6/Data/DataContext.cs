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
            string conString = (Environment.GetEnvironmentVariable("ASPNETCORE_ConString")!=null) ?
                Environment.GetEnvironmentVariable("ASPNETCORE_ConString") :
                _config["ASPNETCORE_ConString"];

            string path = (Environment.GetEnvironmentVariable("ASPNETCORE_EnvWalletPath") != null) ?
                Environment.GetEnvironmentVariable("ASPNETCORE_EnvWalletPath") :
                _config["ASPNETCORE_EnvWalletPath"];

            string TNSName = (Environment.GetEnvironmentVariable("ASPNETCORE_TNSName")) != null ?
                Environment.GetEnvironmentVariable("ASPNETCORE_TNSName") :
                _config["ASPNETCORE_TNSName"];

            string TNSConnectionString = (Environment.GetEnvironmentVariable("ASPNETCORE_TNSConnectionString")) != null ?
                Environment.GetEnvironmentVariable("ASPNETCORE_TNSConnectionString") :
                _config["ASPNETCORE_TNSConnectionString"];

            if (path != null)
            {
                if (OracleConfiguration.TnsAdmin != path)
                {
                    OracleConfiguration.TnsAdmin = path;
                    OracleConfiguration.WalletLocation = path;
                }
            }
            else if (TNSName != null)
            {
                OracleConfiguration.OracleDataSources.Add(TNSName, TNSConnectionString);
            }
            if (conString != null)
            {
                var option = optionsBuilder.UseOracle(conString);
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
