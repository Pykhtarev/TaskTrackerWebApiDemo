using System.Reflection;
using System.Text.Json.Serialization;
using DemoWebApi_BAL.Services;
using DemoWebApi_DAL.Data;
using DemoWebApi_DAL.Interface;
using DemoWebApi_DAL.Model;
using DemoWebApi_DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Task Tracker",
        Description = "An ASP.NET Core Web API for managing Task items",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    c.EnableAnnotations();
});
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddScoped<IRepository<EntityProject>, RepositoryProject>();
builder.Services.AddScoped<IRepository<EntityTask>, RepositoryTask>();
builder.Services.AddScoped<ServiceTrackerManager, ServiceTrackerManager>();
builder.Services.AddDbContext<TrackerContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
