using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NoFlowEngine.Models;
using NoFlowEngine.Data;

namespace NoFlowEngine.Repositories
{
    /// <summary>
    /// UserRepository class implementing the IUserRepository interface.
    /// Provides data access methods for User entities.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application's database context.</param>
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a user from the database based on the username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The User object to add.</param>
        public async Task AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    /// <summary>
    /// IUserRepository interface defining the contract for User data access.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username to search for.</param>
        /// <returns>The User object if found; otherwise, null.</returns>
        Task<User?> GetUserByUsername(string username);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The User object to add.</param>
        Task AddUser(User user);
    }
}