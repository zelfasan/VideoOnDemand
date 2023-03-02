using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOD.Membership.Database.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace VOD.Membership.Database.Contexts
{
    public class VODContext : DbContext
    {
        public VODContext(DbContextOptions<VODContext> options) : base(options)
        {
        }

        public DbSet<Director> Directors => Set<Director>();
        public DbSet<Film> Films => Set<Film>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<FilmGenre> FilmGenres => Set<FilmGenre>();
        public DbSet<SimilarFilms> SimilarFilms =>
         Set<SimilarFilms>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Film>(entity =>
            {
                entity.HasMany(d => d.SimilarFilms)
                .WithOne(p => p.ParentFilm)
                .HasForeignKey(d => d.ParentFilmId)
                .OnDelete(DeleteBehavior.ClientSetNull);
                 entity.HasMany(d => d.Genres)
                .WithMany(p => p.Films)
                .UsingEntity<FilmGenre>()
                .ToTable("FilmGenres");
            });
            builder.Entity<FilmGenre>().HasKey(fg => new { fg.FilmId, fg.GenreId });
            builder.Entity<SimilarFilms>().HasKey(sf => new { sf.ParentFilmId, sf.SimilarFilmId });

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
