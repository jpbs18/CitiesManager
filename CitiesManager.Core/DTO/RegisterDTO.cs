using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email name can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper format")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email already registered")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number can't be blank")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Email should be in proper format")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirmation Password can't be blank")]
        [Compare("Password", ErrorMessage = "Password and confirmation password should match")]
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}
