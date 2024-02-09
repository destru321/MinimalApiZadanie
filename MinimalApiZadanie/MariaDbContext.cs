using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MinimalApiZadanie;

public class MariaDbContext : DbContext
{
    public MariaDbContext(DbContextOptions<MariaDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { set; get; }
    public DbSet<Project> Projects { set; get; }
}