using HaikuLive.Models;
using HaikuLive.Controllers;
using HaikuLive.Hubs;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var dbConnString = Environment.GetEnvironmentVariable("DB_CONN_STRING");
builder.Services.AddDbContext<DatabaseContext>(
  opt =>
  {
    opt.UseNpgsql(dbConnString);
    if (builder.Environment.IsDevelopment())
      {
        opt
          .LogTo(Console.WriteLine, LogLevel.Information)
          .EnableSensitiveDataLogging()
          .EnableDetailedErrors();
      }
  }
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();
app.MapControllers();
app.MapHub<TopicHub>("/r/topicHub");

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
