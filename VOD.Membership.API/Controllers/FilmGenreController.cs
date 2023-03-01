using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.DTOs;
using VOD.Membership.Database.Entities;
using VOD.Membership.Database.Services;

namespace VOD.Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmGenreController : ControllerBase
    {
        private readonly IDbService _db;
        public FilmGenreController(IDbService db) => _db = db;

        [HttpPost]
        public async Task<object> Post([FromBody] FilmGenreDTO fg)
        {
            try
            {
                if (fg == null) return Results.BadRequest();

                var filmgenre = await _db.AddAsync<FilmGenre, FilmGenreDTO>(fg);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.Created(_db.GetURI(filmgenre), filmgenre);
            }
            catch
            {
            }

            return Results.BadRequest();
        }

        [HttpDelete]
        public async Task<object> Delete(FilmGenreDTO dto) 
        {
            try
            {
                var success = _db.Delete<FilmGenre, FilmGenreDTO>(dto);

                if (!success) return Results.NotFound();

                success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest();
        }
    }
}
