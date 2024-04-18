using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ClassLibrary1
{
    /*
     * https://learn.microsoft.com/en-us/ef/core/cli/dotnet
     * //Add BO migration
       1. Open: View -> Other windows -> Package Manager Console
       2. Select project: BackOffice\BackOfficeModel
       3. Type: add-migration script_name
       4. Copy script to Up method: migrationBuilder.Sql("");
       5. "dotnet ef dbcontext optimize" in folder in CMD
     */

    public class BackOfficeModelContainer : DbContext
    {
        private string _connectionString;

        public BackOfficeModelContainer(string pgPort)
        {
            string connectionString = $"Host=localhost;Port={pgPort};Database=BackOffice_servicestest;Username=postgres;Password=1;ApplicationName=botests;Timeout=300;CommandTimeout=300;TcpKeepalive=true;Pooling=true;MaxPoolSize=10;ApplicationName=test;";

            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackOfficeUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(x => x.FullName).HasComputedColumnSql("CASE WHEN \"FirstName\" IS NULL THEN \"LastName\" WHEN \"LastName\"  IS NULL THEN \"FirstName\" ELSE \"FirstName\" || ' ' || \"LastName\" END", true);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, builder =>
            {
                builder.EnableRetryOnFailure(6, TimeSpan.FromSeconds(10), null);
                builder.UseNodaTime();
            });

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<BackOfficeUser> BackOfficeUsers { get; set; }

        public void BulkInsert<T>(IEnumerable<T> list, bool keepIdentity = false) where T : class
        {
            if (!list.Any())
                return;

            this.BulkInsert(list, bulk =>
            {
                bulk.InsertKeepIdentity = keepIdentity;
                bulk.AutoMapOutputDirection = true;
                bulk.Log = s =>
                {
                    //Console.WriteLine(s);
                };
                var databaseCurrentTransaction = this.Database.CurrentTransaction;
                if (databaseCurrentTransaction != null)
                    bulk.Transaction = ((RelationalTransaction)databaseCurrentTransaction).GetDbTransaction();
            });
        }

        public void BulkUpdate<T>(IEnumerable<T> list) where T : class
        {
            if (!list.Any())
                return;
            this.BulkUpdate(list, bulk =>
            {
                bulk.AutoMapOutputDirection = true;
                bulk.Log = s =>
                {
                    //Console.WriteLine(s);
                };
                var databaseCurrentTransaction = this.Database.CurrentTransaction;
                if (databaseCurrentTransaction != null)
                    bulk.Transaction = ((RelationalTransaction)databaseCurrentTransaction).GetDbTransaction();
            });
        }
    }
}