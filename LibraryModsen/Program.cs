using LibraryModsen;
using LibraryModsen.Application;
using LibraryModsen.Application.Common;
using LibraryModsen.Application.Mappers;
using LibraryModsen.Extensions;
using LibraryModsen.Middlewares;
using LibraryModsen.Persistence;
using LibraryModsen.Utilities;
using LibraryModsen.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<GlobalExceptionMiddleware>();
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.EnableFormBindingSourceAutomaticValidation = true;
});

builder.Services.Configure<JwtOptions>(config.GetSection("JWT"));

builder.Services.AddMemoryCache();

builder.Services.ConfigureValidators();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();

builder.Services.AddSingleton<CurrentUser>();

builder.Services.AddAutoMapper(config => config.AddProfile(new MapProfile()));

builder.Services.AddAuth(config.GetSection("JWT").Get<JwtOptions>()!);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
if (args.Contains("/seed"))
{
    SeedData.EnsureSeedData(app);
    return;
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();