using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NoFlowEngine.Data;

namespace NoFlowEngine.Helpers
{
    /// <summary>
    /// Provides a static method to initialize the database.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Initializes the database by creating it if it doesn't exist and optionally seeds data.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Get the database context.
                    var context = services.GetRequiredService<AppDbContext>();

                    // Ensure the database is created.  If it doesn't exist, create it.  If it exists, no action is taken.
                    context.Database.EnsureCreated();

                    // Seed data here if required.  This section can be expanded to include initial data population.
                    // Example:
                    // if (!context.SomeEntity.Any()) {
                    //     context.SomeEntity.Add(new SomeEntity { ... });
                    //     context.SaveChanges();
                    // }
                }
                catch (Exception ex)
                {
                    // Log any errors that occur during database creation or seeding.
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}