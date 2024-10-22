namespace quanlykytuc.Models
{
	public class RoomCostViewModel
	{
		public string RoomName { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public decimal RoomCost { get; set; }
		public decimal ElectricityCost { get; set; }
		public decimal WaterCost { get; set; }
		public decimal OtherCost { get; set; }
		public decimal TotalCost { get; set; }
	}
}
