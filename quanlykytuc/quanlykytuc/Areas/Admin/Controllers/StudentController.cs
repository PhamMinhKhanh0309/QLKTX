using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;
using System.Drawing.Printing;

namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentController : Controller
    {
        private readonly DataContext _context;
        public StudentController(DataContext context)
        {
            _context = context;
        }
        public IActionResult StudentWithRoom(int page = 1)
        {
            // Lọc sinh viên đã có phòng
            int pageSize = 5;
            var StudentQuery = _context.Students.AsQueryable();
            int totalStudent = StudentQuery.Count();
            var studentsWithRoom = _context.Students
                .Where(s => s.RoomID != null && s.RoomID != 0)
                .Skip((page - 1) * pageSize)
                .Take(5)
                .Include(s => s.Room)
                .OrderBy(s => s.StudentID)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalStudent / (double)pageSize);
            return View(studentsWithRoom);
        }
        public IActionResult StudentWithoutRoom(int page = 1)
        {
            int pageSize = 5;
            var StudentQuery = _context.Students.AsQueryable();
            int totalStudent = StudentQuery.Count();
            // Lọc sinh viên chưa có phòng
            var studentsWithoutRoom = _context.Students
                .Where(s => s.RoomID == null || s.RoomID == 0)
                .Skip((page - 1) * pageSize)
                .Take(5)
                .OrderBy(s => s.StudentID)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalStudent / (double)pageSize);
            return View(studentsWithoutRoom);
        }
        public IActionResult Create()
        {
            var RoomList = (from m in _context.Rooms
                          select new SelectListItem()
                          {
                              Text = m.RoomName,
                              Value = m.RoomID.ToString(),
                          }).ToList();

            RoomList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });
            ViewBag.RoomList = RoomList;
            var UserList = (from m in _context.Users
                            where !_context.Students.Any(s => s.UserID == m.UserID) // Lọc những User không có trong bảng Student
                            select new SelectListItem()
                            {
                                Text = m.UserName,
                                Value = m.UserID.ToString(),
                            }).ToList();

            // Thêm tùy chọn "----Select----" vào đầu danh sách
            UserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            ViewBag.UserList = UserList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student st)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(st);
                _context.SaveChanges();
                return RedirectToAction(nameof(StudentWithoutRoom));
            }
            return View(st);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var he = _context.Students.Find(id);
            if (he == null)
            {
                return NotFound();
            }
            var RoomList = (from m in _context.Rooms
                            select new SelectListItem()
                            {
                                Text = m.RoomName,
                                Value = m.RoomID.ToString(),
                            }).ToList();

            RoomList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });
            ViewBag.RoomList = RoomList;
            var UserList = (from m in _context.Users
                            where !_context.Students.Any(s => s.UserID == m.UserID) // Lọc những User không có trong bảng Student
                            select new SelectListItem()
                            {
                                Text = m.UserName,
                                Value = m.UserID.ToString(),
                            }).ToList();

            // Thêm tùy chọn "----Select----" vào đầu danh sách
            UserList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = "0"
            });

            ViewBag.UserList = UserList;
            return View(he);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student st)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Update(st);
                _context.SaveChanges();
                return RedirectToAction(nameof(StudentWithoutRoom));
            }
            return View(st);
        }
    }
}
