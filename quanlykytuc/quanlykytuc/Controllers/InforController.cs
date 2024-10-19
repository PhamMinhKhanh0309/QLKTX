using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;

namespace quanlykytuc.Controllers
{
    public class InforController : Controller
    {
        private readonly DataContext _context;
        public InforController(DataContext context)
        {
            _context = context;
        }
      
        public async Task<IActionResult> Student(int studentID)
        {
            var Id = User.FindFirst("ID")?.Value;
            var students = await _context.Students
                .FirstOrDefaultAsync(o => o.UserID == int.Parse(Id));

            if (students == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
        .FirstOrDefaultAsync(r => r.RoomID == students.RoomID);

            // Tạo ViewModel và truyền dữ liệu vào
            var viewModel = new StudentRoomViewModel
            {
                Student = students,
                Room = room
            };

            // Trả về ViewModel cho view
            return View(viewModel);
        }
       
    }
}
