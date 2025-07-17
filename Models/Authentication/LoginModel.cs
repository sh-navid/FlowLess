using System.ComponentModel.DataAnnotations;

namespace NoFlowEngine.Models.Authentication
{
    /// <summary>
    /// Represents the model for user login.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the username for login.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for login.
        /// </summary>
        [Required]
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