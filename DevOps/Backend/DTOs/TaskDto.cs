using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.Sent
{
    public class TaskDto
    {
        [Required]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required, StringLength(25)]
        public string Status { get; set; }
        [Required]
        public bool Editable { get; set; }
    }
}