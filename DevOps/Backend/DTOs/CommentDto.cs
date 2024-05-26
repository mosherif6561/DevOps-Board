using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CommentDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TaskId { get; set; }
        public string? Comment { get; set; }
    }
}