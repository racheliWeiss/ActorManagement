using ActorManagement.Data;
using ActorManagement.Services;
using ActorManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ActorDbContext>(options =>
        options.UseInMemoryDatabase("ActorDb"));
builder.Services.AddScoped<ActorDbContext>();
builder.Services.AddScoped <IActorService, ActorService>();
builder.Services.AddScoped<IScraperService, ImdbScraperService>();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
    await actorService.PreloadActorsFromIMDb();
}
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
