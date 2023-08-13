using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using anime_climax_api.Models;
using Npgsql;

namespace anime_climax_api.Database;
public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<Animes> Animes { get; set; }
    public DbSet<Clips> Clips { get; set; }

    public DbSet<Buckets> Buckets { get; set; }

    public DbSet<Accounts> Accounts { get; set; }
    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // public DataContext(DbContextOptions<DataContext> options) : base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
        // connectionStringBuilder.Username = "pichan";
        // connectionStringBuilder.Password = "05N6AQXVg18ayJ22FnbFnQ";
        // connectionStringBuilder.Host = "publisher-11849.7tt.cockroachlabs.cloud";
        // connectionStringBuilder.Port = 26257;
        // connectionStringBuilder.Database = "anime_climax";
        // connectionStringBuilder.SslMode = SslMode.VerifyFull;
        options.UseNpgsql("Host=publisher-11849.7tt.cockroachlabs.cloud;Port=26257;Database=anime_climax;Username=pichan;Password=05N6AQXVg18ayJ22FnbFnQ;SslMode=VerifyFull");
    } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();
    }

}