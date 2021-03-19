using HackNU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HackNU.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DbSet<EventModel> Events { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}