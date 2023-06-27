using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DbSet<GlobalEnvironmentVariable> globalenvironmentvariable { get; set; }

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

        public async Task BackupDBAsync()
        {
            var result = await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.StorageRead>();

            if (result == PermissionStatus.Granted)
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, Configuration.Instance.DbSourceFileName);

                var destPath = Path.Combine("/storage/emulated/0/Download", Configuration.Instance.DbSourceFileName);

                if (File.Exists(destPath))
                {
                    File.Delete(destPath);

                    await Task.Delay(100);
                }

                File.Copy(path, destPath);
            }
        }

        public async Task RestoreDBAsync()
        {
            try
            {
                var result = await Permissions.RequestAsync<Permissions.StorageWrite>();
                var r = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (result == PermissionStatus.Granted)
                {
                    var sourcePath = Path.Combine("/storage/emulated/0/Download", Configuration.Instance.DbSourceFileName);

                    var destPath = Path.Combine(FileSystem.AppDataDirectory, Configuration.Instance.DbSourceFileName);

                    if (File.Exists(sourcePath))
                    {
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);

                            await Task.Delay(100);
                        }

                        File.Copy(sourcePath, destPath);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
