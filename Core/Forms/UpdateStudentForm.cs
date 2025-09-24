using System.ComponentModel.DataAnnotations;

namespace Core.Forms
{
    public class UpdateStudentForm
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        public string Email { get; set; } = null!;
    }
}
