using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication2;
using WebApplication2.Configuration.Mapping;
using WebApplication2.Core.Models;
using WebApplication2.Data.DbContexts;
using WebApplication2.Data.Seed;
using WebApplication2.Services;
using WebApplication2.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true; 
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
            ClockSkew = TimeSpan.Zero
        };
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(builder => builder.AddProfiles(AutoMapperProfiles.GetProfiles()));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
{
    o.AddPolicy("frontend", p => p
        .WithOrigins("http://localhost:3000", "https://localhost:3001") // tu Next.js
        .AllowAnyHeader()                     // incluye Authorization, Content-Type, etc.
        .AllowAnyMethod()                     // GET/POST/PUT/DELETE/OPTIONS
        //.AllowCredentials()                  
    );
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.AllowedForNewUsers = true;
    options.Password.RequiredLength = 7;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Evita redirect a login en APIs
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IDirectorService, DirectorService>();
builder.Services.AddScoped<ICoordinadorService, CoordinadorService>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();
builder.Services.AddScoped<IAspiranteService, AspiranteService>();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IPlanEstudioService, PlanEstudiosService>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IGrupoService, GrupoService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IPeriodoAcademicoService, PeriodoAcademicoService>();
builder.Services.AddScoped<ICampusService, CampusService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IMateriaPlanService, MateriaPlanService>();
builder.Services.AddScoped<IParcialesService, ParcialesService>();
builder.Services.AddScoped<IMateriaPlanService, MateriaPlanService>();
builder.Services.AddScoped<ICalificacionesService, CalificacionesService>();
builder.Services.AddScoped<ICatalogoService, CatalogoService>();

var app = builder.Build();

app.Services.InsertInitialData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
