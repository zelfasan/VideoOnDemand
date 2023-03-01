using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.DTOs;
using VOD.Membership.Database.Entities;
using VOD.Membership.Database.Services;

namespace VOD.Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IDbService _db;
        public GenreController(IDbService db) => _db = db;

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                List<GenreDTO>? genres = await _db.GetAsync<Genre, GenreDTO>();

                return genres;
            }
            catch
            {
            }

            return Results.NotFound();
        }

        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                var genre = await _db.SingleAsync<Genre, GenreDTO>(c => c.Id.Equals(id));
                if (genre is null) return Results.NotFound();

                return genre;
            }
            catch
            {
            }
            return Results.NotFound();
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] GenreDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest();

                var director = await _db.AddAsync<Genre, GenreDTO>(dto);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.Created(_db.GetURI<Genre>(director), director);
            }
            catch
            {
            }

            return Results.BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(int id, [FromBody] GenreDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest("No entity provided");
                if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

                var exists = await _db.AnyAsync<Genre>(c => c.Id.Equals(id));
                if (!exists) return Results.NotFound("Could not find entity");

                _db.Update<Genre, GenreDTO>(dto.Id, dto);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.NoContent();
            }
            catch
            {
            }

            return Results.BadRequest("Unable to update the entity");

        }

        [HttpDelete("{id}")]
        public async Task<IResult> Delete(int id)
        {
            try
            {
                var success = await _db.DeleteAsync<Genre>(id);

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
