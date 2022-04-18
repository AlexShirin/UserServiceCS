using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApp1
{
    public class ApplicationContext : DbContext
    {
        private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);
        public DbSet<Person> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=123");
            // optionsBuilder.LogTo(_logStream.WriteLine);
            optionsBuilder.LogTo(_logStream.WriteLine, LogLevel.Debug);
            optionsBuilder.LogTo(_logStream.WriteLine, new[] { RelationalEventId.CommandExecuted });
            optionsBuilder.LogTo(_logStream.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }
    }
}
