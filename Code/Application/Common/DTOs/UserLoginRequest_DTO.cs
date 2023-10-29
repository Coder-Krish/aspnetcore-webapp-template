using System.ComponentModel.DataAnnotations;

namespace Application.Common.dto
{
    public class UserLoginRequest_DTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
