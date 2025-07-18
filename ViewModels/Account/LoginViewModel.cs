using System.ComponentModel.DataAnnotations;

namespace NoFlowEngine.ViewModels.Account
{
    /// <summary>
    /// Represents the view model for user login.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to be remembered.
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}