using alpr.api.Database;
using alpr.api.Services;
using alpr.api.Services.Interfaces;
using alpr.api.Workers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AlprDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register background workers
builder.Services.AddHostedService<VideoProcessingWorker>();

// Register ALPR services
builder.Services.AddSingleton<IAlprEngine, FakeAlprEngine>(); // TODO: Replace with real implementation2
builder.Services.AddSingleton<IVideoMetadataService, FfprobeMetadataService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();