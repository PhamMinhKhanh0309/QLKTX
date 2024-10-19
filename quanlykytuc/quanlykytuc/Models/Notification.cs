using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int NoticeID { get; set; }
        public string? Title { get; set; }
        public string? Contents { get; set; }
        public DateTime? Created_At { get; set; }
        public DateTime? Deleted_At { get; set; }
        public bool  IsActive { get; set; }
    }
}
