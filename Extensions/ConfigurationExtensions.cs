using Microsoft.Extensions.Configuration;

namespace NoFlowEngine.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Retrieves the connection string named "DefaultConnection" from the configuration.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
        /// <returns>The value of the "DefaultConnection" connection string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the "DefaultConnection" connection string is not found in the configuration.</exception>
        public static string GetDefaultConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
    }
}