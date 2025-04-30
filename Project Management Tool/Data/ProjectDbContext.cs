using System;
using Microsoft.EntityFrameworkCore;
namespace Project_Management_Tool.Data;

public class ProjectDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }

    public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
        : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One User has many Projects
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserProjects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // One Project has many Tasks
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Make sure passwords are not mapped as plain fields if needed
        modelBuilder.Entity<User>()
            .Ignore(u => u.Password);
    }
}
