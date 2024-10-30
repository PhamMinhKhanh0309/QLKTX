using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using quanlykytuc.Models;

namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var mnList = _context.Users.OrderBy(m => m.UserID).ToList();
            return View(mnList);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User mn)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(mn);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(mn);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var mn = _context.Users.Find(id);
            if (mn == null)
            {
                return NotFound();
            }

            return View(mn);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User mn)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(mn);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(mn);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var mn = _context.Users.Find(id);
            if (mn == null)
            {
                return NotFound();
            }

            return View(mn);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dele = _context.Users.Find(id);
            if (dele == null)
            {
                return NotFound();
            }

            _context.Users.Remove(dele);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
