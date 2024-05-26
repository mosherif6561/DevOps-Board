using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public record AssignedProjectDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public string? Status { get; set; }
    }
}