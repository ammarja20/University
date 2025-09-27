using System.ComponentModel.DataAnnotations;

namespace Core.Forms
{
    public class CreateCourseForm
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; } = null!;

        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100.")]
        public int Weight { get; set; }
    }
}
