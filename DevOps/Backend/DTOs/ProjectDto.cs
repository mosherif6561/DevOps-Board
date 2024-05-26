using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class ProjectDto
    {
        [Required]
        public string Title { get; set; }

    }
}