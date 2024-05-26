using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public record AssignedTaskDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TaskId { get; set; }
        public string? Attachments { get; set; }
    }
}