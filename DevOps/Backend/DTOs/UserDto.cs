using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs
{
    public record UserDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}