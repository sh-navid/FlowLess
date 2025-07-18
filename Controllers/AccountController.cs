using NoFlowEngine.Services.Authentication;
using NoFlowEngine.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace NoFlowEngine.Controllers
{
    /// <summary>
    /// Handles user account operations such as registration, login, and logout.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service to use for user authentication.</param>
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Displays the registration form.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles the registration process.
        /// </summary>
        /// <param name="model">The registration model containing user details.</param>
        /// <returns>Redirects to the Home page on successful registration; otherwise, returns the registration view with validation errors.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Register(model.Username, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Displays the login form.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles the login process.
        /// </summary>
        /// <param name="model">The login model containing user credentials.</param>
        /// <returns>Redirects to the Home page on successful login; otherwise, returns the login view with a validation error.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Login(model.Username, model.Password, model.RememberMe);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>Redirects to the Home page after logout.</returns>
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}