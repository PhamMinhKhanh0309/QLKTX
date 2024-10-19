using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
    [Table("User")]
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Passwords { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
    }
}
