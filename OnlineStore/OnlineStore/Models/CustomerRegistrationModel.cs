using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class CustomerRegistrationModel
    {
        /// <summary>
        /// User username
        /// </summary>
        [Required, MinLength(4), MaxLength(16)]
        public string Username { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required, MinLength(6), MaxLength(16)]
        public string Password { get; set; }
        /// <summary>
        /// User first name
        /// </summary>
        [Required, MinLength(3), MaxLength(16)]
        public string Firstname { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        [Required, MinLength(3), MaxLength(16)]
        public string Lastname { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        [Required, MinLength(3), MaxLength(16), EmailAddress]
        public string Email { get; set; }
    }
}
