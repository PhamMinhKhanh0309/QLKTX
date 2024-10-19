using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using quanlykytuc.Models;

namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var NotificationQuery = _context.Notifications.AsQueryable();
            int totalNotification = NotificationQuery.Count();
			var mnList = _context.Notifications.OrderByDescending(m => m.Created_At)
				.Skip((page - 1) * pageSize)
				.Take(5)
				.ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalNotification / (double)pageSize);
            return View(mnList);
        }
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Notification nt)
        {
            if (ModelState.IsValid)
            {
                _context.Notifications.Add(nt);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nt);
        }
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var nt = _context.Notifications.Find(id);

			if (nt == null)
			{
				return NotFound();
			}

			var mnList = (from m in _context.Notifications
						  select new SelectListItem()
						  {
							  Text = m.Title,
							  Value = m.NoticeID.ToString(),
						  }).ToList();
			mnList.Insert(0, new SelectListItem()
			{
				Text = "----Select----",
				Value = string.Empty
			});
			ViewBag.mnList = mnList;
			return View(nt);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Notification nt)
		{

			if (ModelState.IsValid)
			{
				_context.Notifications.Update(nt);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(nt);
		}
	}
}
