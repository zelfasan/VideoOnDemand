using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.DTOs
{
    public class SimilarFilmsDTO
    {
        public SimilarFilmsDTO(int parentFilmId, int similarFilmId)
        {
            ParentFilmId = parentFilmId;
            SimilarFilmId = similarFilmId;
        }

        public int ParentFilmId { get; set; }
        public int SimilarFilmId { get; set; }
    }
}
