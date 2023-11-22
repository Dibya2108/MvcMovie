using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Data_Access
{
    public class MovieDBContext : DbContext
    {
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<WatchListMovieAssociation> WatchListMovieAssociations { get; set; }

        public DbSet<WatchList> WatchList { get; set; }

        public DbSet<ShowTime> ShowTime { get; set; }
        public DbSet<MovieShowSeatAssociation> MovieShowSeatAssociation { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movies>()
        .HasRequired(m => m.Language)
        .WithMany(l => l.Movies)
        .HasForeignKey(m => m.SelectedLanguageID)
        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movies>()
    .Property(m => m.IsDeleted)
    .HasColumnType("bit")
    .IsRequired();
        }
    }
}

