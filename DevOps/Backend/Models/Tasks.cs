using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required, StringLength(25)]
        public string Status { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId"), Required]
        public Projects Project { get; set; }

        [JsonIgnore]
        public ICollection<AssignedTasks>? AssignedTasks { get; set; }
    }
}