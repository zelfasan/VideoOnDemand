using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using VOD.Membership.Database.Services;
using VOD.Membership.Database.Entities;
using VOD.Common.DTOs;

namespace VOD.Membership.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly IDbService _db;

        public DirectorController(IDbService db) => _db = db;

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                List<DirectorDTO>? directors = await _db.GetAsync<Director, DirectorDTO>();

                return directors;
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
                var director = await _db.SingleAsync<Director, DirectorDTO>(c => c.Id.Equals(id));
                if (director is null) return Results.NotFound();

                return director;
            }
            catch
            {
            }
            return Results.NotFound();
        }

        [HttpPost]
        public async Task<IResult> Post([FromBody] DirectorDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest();

                var director = await _db.AddAsync<Director, DirectorDTO>(dto);

                var success = await _db.SaveChangesAsync();

                if (!success) return Results.BadRequest();

                return Results.Created(_db.GetURI<Director>(director), director);
            }
            catch
            {
            }

            return Results.BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IResult> Put(int id, [FromBody] DirectorDTO dto)
        {
            try
            {
                if (dto == null) return Results.BadRequest("No entity provided");
                if (!id.Equals(dto.Id)) return Results.BadRequest("Differing ids");

                var exists = await _db.AnyAsync<Director>(c => c.Id.Equals(id));
                if (!exists) return Results.NotFound("Could not find entity");

                _db.Update<Director, DirectorDTO>(dto.Id, dto);

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
                var success = await _db.DeleteAsync<Director>(id);

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
