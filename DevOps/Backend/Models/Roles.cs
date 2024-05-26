using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }
        [StringLength(15)]
        public string Role { get; set; }
        [JsonIgnore] // To avoid A possible cycle
        public ICollection<Users> Users { get; set; }

    }
}
