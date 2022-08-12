using BlogAPI;
using BlogAPI.Entities;
using BlogAPI.Middleware;
using BlogAPI.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<BlogPostSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<BlogDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<BlogPostSeeder>();


seeder.Seed();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
