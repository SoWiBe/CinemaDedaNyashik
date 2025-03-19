using System.Reflection;
using AutoMapper;
using Back.Api.Data;
using Back.Api.Data.Repositories;
using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Mappers;
using Back.Api.Infrastructure.Middleware;
using Back.Api.Infrastructure.Repository;
using Back.Api.Infrastructure.Services;
using Back.Api.Infrastructure.Services.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Настройка Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CinemaDedaNyashikApi",
        Description = "CinemaDedaNyashikApi",
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });

    // Настройка форматирования JSON
    c.CustomSchemaIds(x => x.FullName);

    // Настройка отображения XML-комментариев
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile)); 

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
// Repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMediaContentRepository, MediaContentRepository>();
// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMediaContentService, MediaContentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaDedaNyashikApi V1");
        c.RoutePrefix = string.Empty; // Показывать Swagger на корневом пути
    });
}

app.UseMiddleware<ExceptionsMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();


app.MapControllers();

app.Run();