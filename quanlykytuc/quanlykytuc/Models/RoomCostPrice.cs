using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quanlykytuc.Models
{
	[Table("RoomCost")]
	public class RoomCostPrice
	{
		[Key]
		public int RoomCostID { get; set; }
		public int RoomID { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public decimal RoomCost { get; set; }
		public decimal ElectricityCost { get; set; }
		public decimal WaterCost { get; set; }
		public decimal OtherCost { get; set; }
		public decimal TotalCost { get; set; }
        public virtual Room? Room { get; set; } // Navigation property to Room
    }
}
