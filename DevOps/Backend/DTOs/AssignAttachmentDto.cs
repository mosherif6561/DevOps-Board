using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AssignAttachmentDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TaskId { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}