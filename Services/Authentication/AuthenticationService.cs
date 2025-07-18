using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using NoFlowEngine.Helpers.Security;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using NoFlowEngine.Repositories;
using System.Threading.Tasks;
using System.Security.Claims;
using NoFlowEngine.Models;

namespace NoFlowEngine.Services.Authentication
{
    /// <summary>
    /// Provides authentication and registration services for users.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="userRepository">The repository for user data access.</param>
        /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
        public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers a new user with the specified username and password.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <returns>An <see cref="AuthenticationResult"/> indicating the success or failure of the registration.</returns>
        public async Task<AuthenticationResult> Register(string username, string password)
        {
            var existingUser = await _userRepository.GetUserByUsername(username);
            if (existingUser != null)
            {
                return AuthenticationResult.Failure(new List<string> { "Username already exists." });
            }

            // Hash password using PasswordHasher
            string hashedPassword = PasswordHasher.HashPassword(password, out var salt);

            var newUser = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                PasswordSalt = salt
            };

            await _userRepository.AddUser(newUser);

            await SignInUser(newUser, false); // Auto sign-in after registration
            return AuthenticationResult.Success();
        }

        /// <summary>
        /// Logs in an existing user with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="rememberMe">A boolean indicating whether the user should be remembered.</param>
        /// <returns>An <see cref="AuthenticationResult"/> indicating the success or failure of the login.</returns>
        public async Task<AuthenticationResult> Login(string username, string password, bool rememberMe)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return AuthenticationResult.Failure(new List<string> { "Invalid username or password." });
            }

            // Verify password using PasswordHasher
            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash!, user.PasswordSalt!))
            {
                return AuthenticationResult.Failure(new List<string> { "Invalid username or password." });
            }

            await SignInUser(user, rememberMe);
            return AuthenticationResult.Success();
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Signs in the specified user by creating an authentication cookie.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="isPersistent">A boolean indicating whether the cookie should be persistent.</param>
        private async Task SignInUser(User user, bool isPersistent)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = isPersistent ? DateTimeOffset.UtcNow.AddDays(30) : null
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }

    /// <summary>
    /// Defines the interface for authentication services.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="username">The username for registration.</param>
        /// <param name="password">The password for registration.</param>
        /// <returns>An <see cref="AuthenticationResult"/> indicating the outcome of the registration.</returns>
        Task<AuthenticationResult> Register(string username, string password);

        /// <summary>
        /// Logs in an existing user.
        /// </summary>
        /// <param name="username">The username for login.</param>
        /// <param name="password">The password for login.</param>
        /// <param name="rememberMe">A boolean indicating whether to remember the user.</param>
        /// <returns>An <see cref="AuthenticationResult"/> indicating the outcome of the login.</returns>
        Task<AuthenticationResult> Login(string username, string password, bool rememberMe);

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        Task Logout();
    }

    /// <summary>
    /// Represents the result of an authentication operation.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the authentication operation was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets a list of errors that occurred during the authentication operation.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Creates a successful <see cref="AuthenticationResult"/>.
        /// </summary>
        /// <returns>A successful <see cref="AuthenticationResult"/>.</returns>
        public static AuthenticationResult Success()
        {
            return new AuthenticationResult { Succeeded = true };
        }

        /// <summary>
        /// Creates a failed <see cref="AuthenticationResult"/> with the specified errors.
        /// </summary>
        /// <param name="errors">A list of error messages.</param>
        /// <returns>A failed <see cref="AuthenticationResult"/> with the specified errors.</returns>
        public static AuthenticationResult Failure(List<string> errors)
        {
            return new AuthenticationResult { Succeeded = false, Errors = errors };
        }
    }
}