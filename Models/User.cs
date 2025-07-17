using System.ComponentModel.DataAnnotations;

namespace NoFlowEngine.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        [Display(Name = "Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password hash of the user.
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the password salt of the user.
        /// </summary>
        public byte[]? PasswordSalt { get; set; }
    }
}