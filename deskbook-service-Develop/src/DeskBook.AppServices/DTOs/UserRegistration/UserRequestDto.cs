using System.ComponentModel.DataAnnotations;

namespace DeskBook.AppServices.DTOs.UserRegistration
{
    public class UserRequestDto
    {
        [Required(ErrorMessage = "Please enter your EmailId")]
        [MaxLength(80, ErrorMessage = "Your Email Id cannot exceed 80 characters")]
        [EmailAddress(ErrorMessage = "Please enter valid EmailId.")]
        [RegularExpression(@"^[a-zA-Z0-9]([.]?[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(\.[a-zA-Z]+|(\.[a-zA-Z]+\.[a-zA-Z])+)+$", ErrorMessage = "Please enter valid EmailId")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password.")]
        [MaxLength(25, ErrorMessage = "Your Password cannot exceed 25 characters.")]
        [MinLength(8, ErrorMessage = "Password must be a at least of 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!_&#])[A-Za-z\d@$!_&#]{8,25}$", ErrorMessage = "At least 1 uppercase alphabet, 1 Special Character(@$! _& #) and 1 numeric value required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your first Name.")]
        [MaxLength(100, ErrorMessage = "Your first name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "First name must be of 2 characters long.")]
        [RegularExpression(@"^[A-Za-z]+(('[a-zA-Z])?[a-zA-Z]*)?$", ErrorMessage = "Please enter a valid First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last Name.")]
        [MaxLength(100, ErrorMessage = "Your last name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Last name must be of 2 characters long.")]
        [RegularExpression(@"^[A-Za-z]+(('[a-zA-Z])?[a-zA-Z]*)?$", ErrorMessage = "Please enter a valid Last Name")]
        public string LastName { get; set; }
    }
}