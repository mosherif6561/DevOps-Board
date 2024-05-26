using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CreateTaskDto
    {
        public int? Id { get; set; } //We only need the id if we want to update the task
        public string? Title { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        [Required]
        public int ProjectId { get; set; }
    }
}