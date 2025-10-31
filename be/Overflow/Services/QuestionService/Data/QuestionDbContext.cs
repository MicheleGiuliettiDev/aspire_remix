using Microsoft.EntityFrameworkCore;

namespace Overflow.Services.QuestionsService.Data;

public class QuestionDbContext : DbContext
{
    public QuestionDbContext(DbContextOptions<QuestionDbContext> options)
        : base(options) { }

    public DbSet<Models.Question> Questions => Set<Models.Question>();
    public DbSet<Models.Tag> Tags => Set<Models.Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Question>().HasKey(q => q.Id);

        modelBuilder.Entity<Models.Tag>().HasKey(t => t.Id);

        modelBuilder
            .Entity<Models.Tag>()
            .HasData(
                new Models.Tag
                {
                    Id = "csharp",
                    Name = "csharp",
                    Slug = "csharp",
                    Description = "Questions related to C# programming language.",
                },
                new Models.Tag
                {
                    Id = "efcore",
                    Name = "efcore",
                    Slug = "efcore",
                    Description = "Questions related to Entity Framework Core.",
                },
                new Models.Tag
                {
                    Id = "aspnetcore",
                    Name = "aspnetcore",
                    Slug = "aspnetcore",
                    Description = "Questions related to ASP.NET Core framework.",
                },
                new Models.Tag
                {
                    Id = "database",
                    Name = "database",
                    Slug = "database",
                    Description = "Questions related to databases and data management.",
                },
                new Models.Tag
                {
                    Id = "keycloak",
                    Name = "keycloak",
                    Slug = "keycloak",
                    Description = "Questions related to Keycloak identity and access management.",
                }
            );
    }
}
