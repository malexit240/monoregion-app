using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Monoregion.App.Entites;
using Xamarin.Essentials;

namespace Monoregion.App
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        public DbSet<DirectionModel> Directions { get; set; }

        public DbSet<RecordModel> Records { get; set; }

        public DbSet<GlobalEnvironmentVariable> GlobalEnvironmentVariable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Path.Combine(FileSystem.AppDataDirectory, Configuration.Instance.DbSourceFileName)}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var timestampProps = builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime));

            var converter = new ValueConverter<DateTime, string>(
                v => (new DateTimeOffset(v)).ToUnixTimeMilliseconds().ToString(),
                v => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddMilliseconds(long.Parse(v))
            );
            foreach (var property in timestampProps)
            {
                property.SetValueConverter(converter);
            }
            base.OnModelCreating(builder);
        }
    }
}
