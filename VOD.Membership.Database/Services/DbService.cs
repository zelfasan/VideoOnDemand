using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.DTOs;
using VOD.Membership.Database.Contexts;
using VOD.Membership.Database.Entities;

namespace VOD.Membership.Database.Services
{
    public class DbService : IDbService
    {
        private readonly VODContext _db;
        private readonly IMapper _mapper;
        public DbService(VODContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<TEntity> AddAsync<TEntity, TDto>(TDto dto)
            where TEntity : class
            where TDto : class
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _db.Set<TEntity>().AddAsync(entity);

            return entity;
        }

        public bool Delete<TReferenceEntity, TDto>(TDto dto)
            where TReferenceEntity : class
            where TDto : class
        {
            try
            {
                var entity = _mapper.Map<TReferenceEntity>(dto);
                if (entity is null) return false;
                _db.Remove(entity);
            }
            catch
            {
                throw;
            }

            return true;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _db.SaveChangesAsync()) >= 0;
        }

        async Task<bool> IDbService.AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            return await _db.Set<TEntity>().AnyAsync(expression);
        }

        async Task<bool> IDbService.DeleteAsync<TEntity>(int id)
        {
            try
            {
                var entity = await SingleAsync<TEntity>(x => x.Id == id);
                if (entity is null) return false;
                _db.Remove(entity);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<TDto>> GetAsync<TEntity, TDto>(
        Expression<Func<TEntity, bool>> expression)
        where TEntity : class, IEntity
        where TDto : class
        {
            var entities = await _db.Set<TEntity>().Where(expression).ToListAsync();
            return _mapper.Map<List<TDto>>(entities);
        }

        async Task<List<TDto>> IDbService.GetAsync<TEntity, TDto>()
        {
            var entities = await _db.Set<TEntity>().ToListAsync();

            return _mapper.Map<List<TDto>>(entities);
        }

        private async Task<TEntity?> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class, IEntity
            => await _db.Set<TEntity>().SingleOrDefaultAsync(expression);

        async Task<TDto> IDbService.SingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression)
        {
            var entity = await SingleAsync(expression);
            return _mapper.Map<TDto>(entity);
        }

        void IDbService.Update<TEntity, TDto>(int id, TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity.Id = id;
            _db.Set<TEntity>().Update(entity);
        }

        public void Include<TEntity>() where TEntity : class
        {
            var propertyNames = _db.Model.FindEntityType(typeof(TEntity))?.GetNavigations().Select(e => e.Name);

            if (propertyNames is null) return;

            foreach (var name in propertyNames)
                _db.Set<TEntity>().Include(name).Load();
        }

        public string GetURI<TEntity>(TEntity entity) where TEntity : class, IEntity
        => $"/{typeof(TEntity).Name.ToLower()}/{entity.Id}";

        public string GetURI(FilmGenre fg)
        => $"/filmgenre/";

        public string GetURI(SimilarFilms sf)
        => $"/similarfilm/";
    }
}
