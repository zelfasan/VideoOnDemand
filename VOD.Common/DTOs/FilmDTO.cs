using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VOD.Common.DTOs
{
    public class FilmDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FilmUrl { get; set; }
        public string? Description { get; set; }
        public DateTime Released { get; set; }
        public int DirectorId { get; set; }
        public bool Free { get; set; }
        public DirectorDTO Director { get; set; } = new();
        public List<GenreDTO> Genres { get; set; } = new();
    }

    public class FilmCreateDTO
    {
        public string? Title { get; set; }
        public string? FilmUrl { get; set; }
        public string? Description { get; set; }
        public DateTime Released { get; set; }
        public int DirectorId { get; set; }
        public bool Free { get; set; }
        public List<GenreDTO> Genres { get; set; } = new();
    }

    public class FilmEditDTO : FilmCreateDTO
    {
        public int Id { get; set; }
    }
}
