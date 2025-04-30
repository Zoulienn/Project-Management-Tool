using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Project_Management_Tool.Data
{
    public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public ProjectDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=ProjectManagementDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;");

            return new ProjectDbContext(optionsBuilder.Options);
        }
    }
}