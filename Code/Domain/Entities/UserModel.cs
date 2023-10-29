using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
