using Microsoft.EntityFrameworkCore;
using NoFlowEngine.Models;

namespace NoFlowEngine.Data
{
    /// <summary>
    /// Represents the application's database context using Entity Framework Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the DbSet for the User entity.  This allows querying and saving User entities to the database.
        /// </summary>
        public DbSet<User> Users { get; set; }
    }
}