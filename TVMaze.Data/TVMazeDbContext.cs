using TVMaze.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TVMaze.Data
{
    public class TVMazeDbContext : DbContext
    {
        public TVMazeDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Person> Actors { get; set; }
        public DbSet<ShowCast> ShowCasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureShow(modelBuilder);
            ConfigurePerson(modelBuilder);
            ConfigureShowCasts(modelBuilder);
        }

        private static void ConfigureShowCasts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShowCast>()
                            .HasKey(x => new { x.ShowId, x.PersonId });

            modelBuilder.Entity<ShowCast>()
                .HasOne(sc => sc.Show)
                .WithMany(s => s.Casts)
                .HasForeignKey(sh => sh.ShowId);

            modelBuilder.Entity<ShowCast>()
                .HasOne(sc => sc.Person)
                .WithMany(p => p.Casts)
                .HasForeignKey(sh => sh.PersonId);
        }

        private static void ConfigurePerson(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                            .HasKey(p => p.Id);

            modelBuilder.Entity<Person>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired(true)
                .IsUnicode(true);
        }

        private static void ConfigureShow(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>()
                            .HasKey(s => s.Id);

            modelBuilder.Entity<Show>()
                .Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired(true)
                .IsUnicode(true);
        }
    }
}
