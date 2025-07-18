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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}$",
            ErrorMessage = "Passwords must be at least 8 characters and contain: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
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