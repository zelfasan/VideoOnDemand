using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace VOD.Membership.Database.Entities
{
    public class Film : IEntity
    {
        public int Id { get; set; }

        [MaxLength(80)]
        public string? Title { get; set; }

        [MaxLength(1024)]
        public string? FilmUrl { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
        public DateTime Released { get; set; }
        public int DirectorId { get; set; }
        public bool Free { get; set; }

        public virtual Director Director { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<SimilarFilms> SimilarFilms { get; set;}
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}
