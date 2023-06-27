using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monoregion.Web.Entities;
using System.Text;

namespace Monoregion.Web
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            SQLitePCL.Batteries_V2.Init();

            try
            {
                if (Database.EnsureCreated() && Database.IsSqlite())
                {
                    InstallTriggersOnUpdate();
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("DB exception");
            }
        }

        public DbSet<DirectionModel> Directions { get; set; }

        public DbSet<RecordModel> Records { get; set; }

        public DbSet<GlobalEnvironmentVariable> GlobalEnvironmentVariables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var timestampProps = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(byte[]) && p.ValueGenerated == ValueGenerated.OnAddOrUpdate);
            var converter = new ValueConverter<byte[], string>(
                v => Encoding.UTF8.GetString(v),
                v => Encoding.UTF8.GetBytes(v)
            );
            foreach (var property in timestampProps)
            {
                property.SetValueConverter(converter);
                property.SetDefaultValueSql("STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW')");
            }
            base.OnModelCreating(builder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTrackedEntitites();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTrackedEntitites();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateTrackedEntitites()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is EntityTableData).ToList();

            var dateTimeNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var tableData = entry.Entity as EntityTableData;
                tableData.UpdatedAt = dateTimeNow;

                if (entry.State == EntityState.Added)
                {
                    if (string.IsNullOrWhiteSpace(tableData.Id))
                    {
                        tableData.Id = Guid.NewGuid().ToString("N");
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    tableData.Deleted = true;
                }
            }
        }

        private void InstallTriggersOnUpdate()
        {
            foreach (var table in Model.GetEntityTypes())
            {
                var props = table.GetProperties().Where(prop => prop.ClrType == typeof(byte[]) && prop.ValueGenerated == ValueGenerated.OnAddOrUpdate);
                foreach (var property in props)
                {
                    var sql = $@"
                 CREATE TRIGGER s_{table.GetTableName()}_{property.Name}_UPDATE AFTER UPDATE ON  {table.GetTableName()}
                 BEGIN
                     UPDATE {table.GetTableName()}
                     SET {property.Name} = STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW')
                     WHERE rowid = NEW.rowid;
                 END
             ";
                    Database.ExecuteSqlRaw(sql);
                }
            }

            SaveChangesAsync();
        }
    }
}
