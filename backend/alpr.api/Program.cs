using alpr.api.Database;
using alpr.api.Helpers;
using alpr.api.Services;
using alpr.api.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AlprDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register ALPR services
//builder.Services.AddSingleton<IAlprEngine, FakeAlprEngine>(); // TODO: Replace with real implementation2
builder.Services.AddSingleton<IVideoMetadataService, FfprobeMetadataService>();
builder.Services.AddHostedService<VideoProcessingService>();
builder.Services.AddSingleton<IAlprEngine, AlprEngine>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Increase size limits for video uploads
// Form options
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = VideoConstants.MAX_FILE_SIZE;
});

// Kestrel server options
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = VideoConstants.MAX_FILE_SIZE;
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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "frames")),
    RequestPath = "/frames"
});

app.Run();