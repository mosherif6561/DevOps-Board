using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        //User Roles 1.developer 2.team-leader
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Roles Role { get; set; }

        [JsonIgnore] //To prevent possible cycle
        public ICollection<AssignedTasks>? AssignedTasks { get; set; }
        [JsonIgnore] // To prevent possible cycle
        public ICollection<ProjectsStatus>? AssignedProjects { get; set; }
    }
}