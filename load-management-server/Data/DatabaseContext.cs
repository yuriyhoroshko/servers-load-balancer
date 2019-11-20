using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data
{
    public class LoadManagerContext : DbContext
    {
    internal DbSet<WorkerServer> WorkerServers { get; set; }

    internal DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-L52JV7B;Database=Load Manager; User Id = sa; Password=123456;");
        }
    }
}