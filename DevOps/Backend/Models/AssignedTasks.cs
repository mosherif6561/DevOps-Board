using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class AssignedTasks
    {
        // Foreign keys
        [Key]
        public int UserId { get; set; }
        [Key]
        public int TaskId { get; set; }

        // Navigation Properties
        [ForeignKey("UserId")]
        public Users Users { get; set; }
        [ForeignKey("TaskId")]
        public Tasks Tasks { get; set; }
        // Cannot make it of type List..

        // Rest of the attributes
        [FileExtensions(Extensions = "jpg,jpeg,png")]
        [DataType(DataType.ImageUrl)]
        public string? Attachments { get; set; }
    }
}