using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class ProjectsStatus
    {
        //Foregin Keys
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        //Navigation Properties
        [ForeignKey("ProjectId")]
        public virtual Projects? Projects { get; set; }
        [ForeignKey("UserId")]

        //Rest of the attributes
        public virtual Users? Users { get; set; }
        public string? Status { get; set; }
    }
}