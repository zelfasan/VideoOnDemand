using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VOD.Common.DTOs;
using VOD.Common.HttpClients;
using VOD.Membership.Database.Contexts;
using VOD.Membership.Database.Entities;
using VOD.Membership.Database.Services;
using static System.Collections.Specialized.BitVector32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(policy => {
    policy.AddPolicy("CorsAllAccessPolicy", opt =>
        opt.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VODContext>(
options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("VODConnection")));
ConfigureAutomapper(builder.Services);

builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddHttpClient<MembershipHttpClient>(client => client.BaseAddress = new Uri("https://localhost:6001/api/"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsAllAccessPolicy");

app.UseAuthorization();

app.MapControllers();
app.Run();

static void ConfigureAutomapper(IServiceCollection services)
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<Film, FilmDTO>().ReverseMap();
        cfg.CreateMap<FilmEditDTO, Film>()
            .ForMember(dest => dest.Director, src => src.Ignore())
            .ForMember(dest => dest.Genres, src => src.Ignore());

        cfg.CreateMap<FilmCreateDTO, Film>()
            .ForMember(dest => dest.Director, src => src.Ignore())
            .ForMember(dest => dest.Genres, src => src.Ignore());
        cfg.CreateMap<Genre, GenreDTO>().ReverseMap();
        cfg.CreateMap<Director, DirectorDTO>().ReverseMap();
        cfg.CreateMap<FilmGenre, FilmGenreDTO>().ReverseMap();
        cfg.CreateMap<SimilarFilms, SimilarFilmsDTO>().ReverseMap();
    });
    var mapper = config.CreateMapper();
    services.AddSingleton(mapper);
}