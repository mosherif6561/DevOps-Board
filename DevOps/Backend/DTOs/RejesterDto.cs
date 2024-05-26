using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class RejesterDto
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(50)]
        public string Email { get; set; }
        [Required, StringLength(50)]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
