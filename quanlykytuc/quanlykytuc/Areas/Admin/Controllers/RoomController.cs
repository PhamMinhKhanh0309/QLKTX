using Microsoft.AspNetCore.Mvc;
using quanlykytuc.Models;

namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private DataContext _context;
        public RoomController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
			int pageSize = 5;
			var RoomQuery = _context.Rooms.AsQueryable();
			int totalRoom = RoomQuery.Count();
			var room = _context.Rooms.OrderBy(r => r.RoomID)
				.Skip((page - 1) * pageSize)
				.Take(5)
				.ToList();

			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = (int)Math.Ceiling(totalRoom / (double)pageSize);
			return View(room);
        }
        public IActionResult Create()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room he)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Add(he);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(he);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var he = _context.Rooms.Find(id);
            if (he == null)
            {
                return NotFound();
            }
            return View(he);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            var he = _context.Rooms.Find(id);
            if (he == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(he);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var he = _context.Rooms.Find(id);
            if (he == null)
            {
                return NotFound();
            }

            return View(he);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Room he)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Update(he);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(he);
        }
    }
}
