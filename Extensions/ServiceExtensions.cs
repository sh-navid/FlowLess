using Microsoft.AspNetCore.Authentication.Cookies;
using NoFlowEngine.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using NoFlowEngine.Repositories;
using NoFlowEngine.Data;

namespace NoFlowEngine.Extensions
{
    /// <summary>
    /// Contains extension methods for configuring services in the dependency injection container.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures the database context to use SQLite with the provided connection string.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <param name="connectionString">The SQLite connection string.</param>
        public static void ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
        }

        /// <summary>
        /// Configures various application services, including authentication, user repository,
        /// and HTTP context accessor. Also sets up cookie-based authentication with security enhancements.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Scoped lifetime services:  created once per client request.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Singleton lifetime service:  created the first time they are requested.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configures cookie-based authentication.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // Mitigates XSS attacks by preventing client-side scripts from accessing the cookie.
                    options.Cookie.HttpOnly = true;

                    // Ensures the cookie is only sent over HTTPS.
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                    // Specifies the path to the login page.
                    options.LoginPath = "/Account/Login";

                    // Specifies the path to the access denied page.
                    options.AccessDeniedPath = "/Account/AccessDenied";

                    // Sets the cookie expiration time to 30 days.
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);

                    // Allows the cookie expiration to slide, extending it each time the cookie is used.
                    options.SlidingExpiration = true;
                });

            // Configures authorization policies.
            services.AddAuthorization();
        }
    }
}