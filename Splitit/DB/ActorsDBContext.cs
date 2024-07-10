using Microsoft.EntityFrameworkCore;
using Splitit.Models;
namespace Splitit.EntityFM
{

    public class ActorsDbContext : DbContext
    {
        public ActorsDbContext(DbContextOptions<ActorsDbContext> options)
    : base(options)
        {

        }
        public DbSet<Actor> Actors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Rank).IsRequired();
            });
        }
    }
}
