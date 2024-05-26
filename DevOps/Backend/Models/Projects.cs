using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Projects
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [JsonIgnore] //To prevent possible cycle
        public ICollection<Tasks> Tasks { get; set; }
    }
}