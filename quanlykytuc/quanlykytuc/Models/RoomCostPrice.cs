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
		public int RoomCost { get; set; }
		public int ElectricityCost { get; set; }
		public int WaterCost { get; set; }
		public int OtherCost { get; set; }
		public int TotalCost { get; set; }
	}
}
