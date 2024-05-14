using ActorManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Options;

namespace ActorManagement.Data
{
    public class ActorDbContext : DbContext
    {
        public ActorDbContext(DbContextOptions<ActorDbContext> options) : base(options)
        {
        }
        public DbSet<Actor> Actors { get; set; }
    }
}
