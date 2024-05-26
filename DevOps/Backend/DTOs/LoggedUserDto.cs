using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoggedUserDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Role { get; set; }
    }
}