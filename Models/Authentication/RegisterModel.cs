using System.ComponentModel.DataAnnotations;

namespace NoFlowEngine.Models.Authentication
{
    /// <summary>
    /// Represents the model for user registration.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the username for registration.
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for registration.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation for registration.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}