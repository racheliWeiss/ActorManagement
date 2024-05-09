using ActorManagement.Data;
using ActorManagement.Models;
using ActorManagement.Services;
using ActorManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ActorDbContext>(options =>
        options.UseInMemoryDatabase("ActorDb"));

builder.Services.AddScoped<ActorDbContext>();
builder.Services.AddTransient <IActorService, ActorService>();
builder.Services.AddScoped<IScraperService, ScraperService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.Configure<SettingsUrl>(builder.Configuration.GetSection("SettingsUrl"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var scraperService = scope.ServiceProvider.GetRequiredService<IScraperService>();
    await scraperService.PreloadActors();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
