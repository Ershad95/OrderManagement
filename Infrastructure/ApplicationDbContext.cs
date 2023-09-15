using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var part = new Part("بخش 1") { Id = 1 };
        var user = new User("ershad", "12345", "001010010", "waveershad21@gmail.com") { Id = 1 };
        
        modelBuilder.Entity<Part>().HasData(
            part,
            new Part("بخش 2") { Id = 2 },
            new Part("بخش 3") { Id = 3 }
        );
        modelBuilder.Entity<Product>().HasData(
            new Product("محصول 1") { Id = 1 },
            new Product("محصول 2") { Id = 2 },
            new Product("محصول 3") { Id = 3 }
        );

        modelBuilder.Entity<User>()
            .HasMany(e => e.Parts)
            .WithMany(e => e.Users)
            .UsingEntity(
                "PartUser",
                l => l.HasOne(typeof(Part)).WithMany().HasForeignKey("PartId").HasPrincipalKey(nameof(Part.Id)),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
                j => j.HasKey("PartId", "UserId"))
            .HasData(user);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
}

public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(@$"{currentDirectory}\..\WebHost")
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}