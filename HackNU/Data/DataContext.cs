using HackNU.Models;
using Microsoft.EntityFrameworkCore;

namespace HackNU.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        
        public DataContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=hacknudb;Username=hacknuadmin;Password=HackNU2021TeamIO");
        }
    }
}