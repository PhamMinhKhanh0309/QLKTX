using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;

namespace quanlykytuc.Controllers
{
    public class RoomController : Controller
    {
        private readonly DataContext _context;
        public RoomController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy StudentID từ User Claims
            var studentIdClaim = User.FindFirst("ID")?.Value;
            if (studentIdClaim == null)
            {
                return BadRequest("User is not logged in or ID is not available.");
            }

            // Parse StudentID từ chuỗi sang số nguyên
            int studentID;
            if (!int.TryParse(studentIdClaim, out studentID))
            {
                return BadRequest("Invalid student ID.");
            }

            // Lấy thông tin của sinh viên dựa vào StudentID
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserID == studentID);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Lấy RoomID từ thông tin sinh viên
            var roomID = student.RoomID;
            if (roomID == null)
            {
                return BadRequest("Student is not assigned to any room.");
            }

            // Lấy thông tin của phòng dựa vào RoomID
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomID == roomID);
            if (room == null)
            {
                return NotFound("Room not found.");
            }

            // Lấy danh sách sinh viên ở trong cùng phòng với sinh viên hiện tại
            var studentsInRoom = await _context.Students
                .Where(s => s.RoomID == roomID)
                .ToListAsync();

            // Tạo ViewModel và truyền dữ liệu vào
            var viewModel = new RoomDetailsViewModel
            {
                Room = room,
                Students = studentsInRoom
            };

            // Trả về ViewModel cho view
            return View(viewModel);
        }
    }
}

