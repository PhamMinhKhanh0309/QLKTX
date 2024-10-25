using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
    [Table("RoomRegistration")]
    public class RoomRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        public int StudentID { get; set; }
        public int RoomID { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string? Phone {  get; set; }
        public string? Email { get; set; }
        public virtual Student? Student { get; set; }  // Navigation property
        public virtual Room? Room { get; set; }  // Navigation property

    }
}
