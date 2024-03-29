using Do_An_Tot_Nghiep.Models;
using Microsoft.EntityFrameworkCore;

namespace Do_An_Tot_Nghiep;

public class PublicContext : DbContext
{
    public  DbSet<User> Users { get; set; }
    public  DbSet<Grammar>Grammars  { get; set; }
    private const string connectionString = @"User ID=minhpd;Password=123qwe;Host=103.124.95.246;Port=5432;Database=minh;Pooling=true;MinPoolSize=0;MaxPoolSize=100;Connection Lifetime=0;";
    protected override void  OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString);
    }
}