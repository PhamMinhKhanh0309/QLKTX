using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
    [Table("Room")]
    public class Room
    {
        public int RoomID { get; set; }
        public string? RoomName { get; set; }
        /*public int Price { get; set; }*/
        public int Quantity { get; set; }
        public int Max { get; set; }
        public string? Description { get; set; }
        public ICollection<Student>? Students { get; set; } // Navigation property for students in the room
        public ICollection<RoomCostPrice>? RoomCostPrices { get; set; } // Navigation property for students in the room
    }   
}
