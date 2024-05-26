using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class UpdateCommentDto
    {
        [Required]
        public int Id { get; set; }
        public string? Comment { get; set; }
    }
}