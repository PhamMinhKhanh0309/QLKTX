using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;

namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomRegistrationController : Controller
    {
        private readonly DataContext _context;
        public RoomRegistrationController(DataContext context)
        {
            _context = context;
        }
        // GET: Hiển thị danh sách các đăng ký chờ duyệt
        public IActionResult ApproveRoom()
        {
            // Lấy danh sách các đăng ký phòng chưa được duyệt (Status = 0)
            var pendingRegistrations = _context.RoomRegistrations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .Where(r => r.Status == 0)
                .ToList();

            return View(pendingRegistrations);
        }

        // POST: Duyệt đăng ký phòng cho sinh viên
        [HttpPost]
        public IActionResult ApproveRoom(int registrationId)
        {
            // Tìm bản đăng ký dựa trên registrationId
            var registration = _context.RoomRegistrations
                .Include(r => r.Student)
                .Include (r => r.Room)
                .FirstOrDefault(r => r.RegistrationID == registrationId);

            if (registration == null)
            {
                return Json(new { success = false, message = "Registration not found." });
            }

            // Cập nhật ID phòng trong bảng Student
            var student = _context.Students.FirstOrDefault(s => s.StudentID == registration.StudentID);
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == registration.RoomID);

            if (student != null)
            {
                // Gán RoomID cho sinh viên
                student.RoomID = registration.RoomID;
                //Thêm số thành viên trong phòng
                room.Quantity++;
                // Đánh dấu bản đăng ký là đã duyệt
                registration.Status = 1;

                // Lưu thay đổi vào database
                _context.SaveChanges();

                return Json(new { success = true, message = "Phòng đã được phê duyệt và giao cho sinh viên thành công." });
            }

            return Json(new { success = false, message = "Lỗi duyệt phòng không thành công" });
        }
    }
}
