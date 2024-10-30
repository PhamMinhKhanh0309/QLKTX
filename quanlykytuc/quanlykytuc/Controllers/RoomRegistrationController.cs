using Microsoft.AspNetCore.Mvc;
using quanlykytuc.Models;

namespace quanlykytuc.Controllers
{
    public class RoomRegistrationController : Controller
    {
        private readonly DataContext _context;
        public RoomRegistrationController(DataContext context)
        {
            _context = context;
        }
        // GET: Hiển thị form đăng ký phòng
        [HttpGet]
        public IActionResult RegisterRoom()
        {
            // Lấy StudentID từ User Claims
            var studentIdClaim = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out int studentId))
            {
                return Unauthorized("Student ID is invalid.");
            }
            
            // Kiểm tra xem sinh viên đã đăng ký phòng hay chưa
            var existingRegistration = _context.RoomRegistrations
                .FirstOrDefault(r => r.Student.UserID == studentId && r.Status == 0); // 0 là trạng thái đăng ký đang hoạt động
            if (existingRegistration != null)
            {
                // Nếu đã đăng ký phòng, hiển thị thông tin về phòng hiện tại
                var currentRoom = _context.Rooms.FirstOrDefault(r => r.RoomID == existingRegistration.RoomID);

                ViewBag.RoomName = currentRoom?.RoomName;
                ViewBag.Message = "Bạn đã đăng ký phòng.";
                return View("existingRegistration"); // Chuyển sang view hiển thị thông báo
            }
            // Kiểm tra xem sinh viên đã đăng ký phòng hay chưa
            var AlreadyRegistered = _context.RoomRegistrations
                .FirstOrDefault(r => r.Student.UserID == studentId && r.Status == 1); // 1 là trạng thái đã có phòng
            if (AlreadyRegistered != null)
            {
                // Nếu đã đăng ký phòng, hiển thị thông tin về phòng hiện tại
                var currentRoom = _context.Rooms.FirstOrDefault(r => r.RoomID == AlreadyRegistered.RoomID);

                ViewBag.RoomName = currentRoom?.RoomName;
                ViewBag.Message = "Bạn đã có phòng.";
                return View("AlreadyRegistered"); // Chuyển sang view hiển thị thông báo
            }
            // Tạo đối tượng model mới để gửi sang View
            var model = new RoomRegistration
            {
                RegistrationDate = DateTime.Now
            };

            // Lấy danh sách phòng và truyền vào ViewBag
            ViewBag.Rooms = _context.Rooms
            .Where(r => _context.RoomRegistrations.Count(reg => reg.RoomID == r.RoomID && reg.Status == 1) < r.Max)
            .ToList();

            return View(model);
        }

        // POST: Nhận và xử lý dữ liệu từ form đăng ký phòng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterRoom(RoomRegistration model)
        {
            // Lấy StudentID từ User Claims
            var studentIdClaim = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(studentIdClaim) || !int.TryParse(studentIdClaim, out int studentId))
            {
                return Unauthorized("Student ID is invalid.");
            }

            // Kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                // Kiểm tra xem sinh viên đã đăng ký phòng hay chưa
                var existingRegistration = _context.RoomRegistrations
                    .FirstOrDefault(r => r.StudentID == studentId && r.Status == 0); // 0 là trạng thái đang đăng ký

                if (existingRegistration != null)
                {
                    ModelState.AddModelError("", "Bạn đã đăng ký phòng rồi.");
                    ViewBag.Rooms = _context.Rooms.ToList(); // Đảm bảo danh sách phòng vẫn được hiển thị nếu có lỗi
                    return View(model);
                }

                // Kiểm tra xem phòng có còn trống không
                var room = _context.Rooms.FirstOrDefault(r => r.RoomID == model.RoomID);

                //Lấy StudentID dựa trên  Id của user
                var student = _context.Students
                .Where(s => s.UserID == studentId).FirstOrDefault();
                

                // Kiểm tra số lượng sinh viên hiện tại trong phòng
                var currentRegistrations = _context.RoomRegistrations
                    .Count(r => r.RoomID == model.RoomID && r.Status == 1); // 1 là trạng thái đăng ký đang hoạt động

                if (currentRegistrations >= room.Max)
                {
                    ModelState.AddModelError("", "Phòng đã đủ người ở.");
                    ViewBag.Rooms = _context.Rooms.ToList();
                    return View(model);
                }

                // Nếu phòng còn trống, thêm đăng ký mới vào cơ sở dữ liệu
                var newRegistration = new RoomRegistration
                {
                    StudentID = student.StudentID,
                    RoomID = model.RoomID,
                    RegistrationDate = DateTime.Now,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = 0, // 0 là trạng thái đang đăng ký
                    Email = model.Email,
                    Phone = model.Phone
                    
                };

                _context.RoomRegistrations.Add(newRegistration);
                _context.SaveChanges();

                // Chuyển hướng về trang xác nhận
                return RedirectToAction("RegistrationConfirmation");
            }

            // Nếu có lỗi, trả về view cùng với lỗi
            ViewBag.Rooms = _context.Rooms.ToList(); // Đảm bảo danh sách phòng vẫn được hiển thị nếu có lỗi
            return View(model);
        }

        // Trang xác nhận đăng ký
        public IActionResult RegistrationConfirmation()
        {
            return View();
        }
    }
}
