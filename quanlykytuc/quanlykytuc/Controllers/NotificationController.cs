using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;

namespace quanlykytuc.Controllers
{
    public class NotificationController : Controller
    {
        private readonly DataContext _context;
        public NotificationController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {

            int pageSize = 3;
            var postsQuery = _context.Notifications.AsQueryable();
            int totalPosts = postsQuery.Count();
            var mnList = postsQuery
                        .OrderByDescending(a => a.Created_At)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
        .               ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
            return View(mnList);

        }
    }
}
