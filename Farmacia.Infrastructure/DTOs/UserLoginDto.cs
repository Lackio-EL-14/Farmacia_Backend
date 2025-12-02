using System.ComponentModel.DataAnnotations;

namespace Farmacia.Infrastructure.DTOs
{
    public class UserLoginDto
    {
        [Required]
        public string Usuario { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}