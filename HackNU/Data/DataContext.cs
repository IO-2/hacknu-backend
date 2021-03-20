using HackNU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.Replication.PgOutput.Messages;

namespace HackNU.Data
{
    public class DataContext : IdentityDbContext<UserModel>
    {
        public DbSet<EventModel> Events { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}