using Microsoft.AspNetCore.Mvc;
using quanlykytuc.Models;

namespace quanlykytuc.Controllers
{
    public class RoomCostController : Controller
    {
		private readonly DataContext _context;
		public RoomCostController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index(int? month, int? year)
        {
            // Lấy StudentID từ User Claims
            var studentIdClaim = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out int studentId))
            {
                return Unauthorized("Student ID is invalid.");
            }

            // Lấy thông tin phòng của sinh viên dựa vào StudentID
            var room = _context.Rooms
                .Where(r => r.Students.Any(s => s.UserID == studentId))
                .FirstOrDefault();

            if (room == null)
            {
                return NotFound("Room for the student not found.");
            }

            // Nếu người dùng không chọn tháng hoặc năm, sử dụng tháng và năm hiện tại
            var currentMonth = month ?? DateTime.Now.Month;
            var currentYear = year ?? DateTime.Now.Year;

            // Lấy thông tin chi phí phòng từ RoomCostPrice dựa vào RoomID, Month và Year
            var roomCost = _context.roomCostPrices
                .Where(rc => rc.RoomID == room.RoomID && rc.Year == currentYear && rc.Month == currentMonth)
                .FirstOrDefault();

            // Nếu không tìm thấy chi phí cho tháng và năm hiện tại, tìm chi phí gần nhất
            if (roomCost == null)
            {
                roomCost = new RoomCostPrice
                {
                    RoomCost = 0,
                    ElectricityCost = 0,
                    WaterCost = 0,
                    OtherCost = 0,
                    TotalCost = 0
                };
            }

            // Tạo ViewModel để gửi dữ liệu sang View
            var model = new RoomCostViewModel
            {
                RoomName = room.RoomName,
                Month = currentMonth,
                Year = currentYear,
                RoomCost = roomCost.RoomCost,
                ElectricityCost = roomCost.ElectricityCost,
                WaterCost = roomCost.WaterCost,
                OtherCost = roomCost.OtherCost,
                TotalCost = roomCost.TotalCost
            };

            return View(model); // Trả về View với model chứa thông tin chi phí
        }
    }
}
