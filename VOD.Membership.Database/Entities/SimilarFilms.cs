using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Membership.Database.Entities
{
    public class SimilarFilms : IReferenceEntity
    {
        public int ParentFilmId { get; set; }
        public int SimilarFilmId { get; set; }

        public virtual Film? ParentFilm { get; set; }
        public virtual Film? SimilarFilm { get; set; }
    }
}
