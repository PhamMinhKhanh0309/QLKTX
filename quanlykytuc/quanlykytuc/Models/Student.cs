using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
    [Table("Student")]
    public class Student
    {
        public int StudentID { get; set; }
        public string? Name { get; set; }
        public string? Birth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? RoomID { get; set; }
        public int Priority { get; set; }
        public int UserID { get; set; }
        public string? StudentImage { get; set; }
        public virtual Room? Room { get; set; } //Thuộc tính điều hướng đến Phòng
    }
}
