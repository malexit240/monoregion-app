using System.Text;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monoregion.Web.Entities;
using Monoregion.Web.Helpers;

namespace Monoregion.Web
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            InitializeForSqlite();
        }

        public DbSet<DirectionModel> Directions { get; set; }

        public DbSet<RecordModel> Records { get; set; }

        public DbSet<GlobalEnvironmentVariable> GlobalEnvironmentVariables { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureVersionFields(builder);

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
            var entries = ChangeTracker.Entries().Where(e => e.Entity is EntityTableData);

            foreach (var entry in entries)
            {
                var tableRow = entry.Entity as EntityTableData;
                tableRow.UpdatedAt = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    if (string.IsNullOrWhiteSpace(tableRow.Id))
                    {
                        tableRow.Id = IdGenerationHelper.GetNext();
                    }
                }
                else if (entry.State == EntityState.Deleted)
                {
                    tableRow.Deleted = true;
                }
            }
        }

        private void InitializeForSqlite()
        {
            SQLitePCL.Batteries_V2.Init();

            try
            {
                if (Database.EnsureCreated())
                {
                    InstallTriggersOnUpdate();
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("DB exception");
            }
        }

        private void InstallTriggersOnUpdate()
        {
            foreach (var table in Model.GetEntityTypes())
            {
                var properties = table.GetProperties().Where(p => p.ClrType == typeof(byte[]) && p.ValueGenerated == ValueGenerated.OnAddOrUpdate);

                foreach (var property in properties)
                {
                    var sql = GetTriggerSqlQuery(table, property);

                    Database.ExecuteSqlRaw(sql);
                }
            }

            SaveChangesAsync();
        }

        private void ConfigureVersionFields(ModelBuilder builder)
        {
            var timestampProperties = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(byte[]) && p.ValueGenerated == ValueGenerated.OnAddOrUpdate);

            var converter = new ValueConverter<byte[], string>(
                v => Encoding.UTF8.GetString(v),
                v => Encoding.UTF8.GetBytes(v)
            );
            foreach (var property in timestampProperties)
            {
                property.SetValueConverter(converter);
                property.SetDefaultValueSql("STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW')");
            }
        }

        private string GetTriggerSqlQuery(IEntityType table, IProperty property)
        {
            return $@"
    CREATE TRIGGER s_{table.GetTableName()}_{property.Name}_UPDATE 
    AFTER   UPDATE ON  {table.GetTableName()}
        BEGIN
            UPDATE {table.GetTableName()}
            SET {property.Name} = STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW')
            WHERE rowid = NEW.rowid;
        END";
        }
    }
}
