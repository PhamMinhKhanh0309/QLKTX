using Microsoft.AspNetCore.Mvc;
using quanlykytuc.Models;

namespace quanlykytuc.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class RoomCostController : Controller
	{
		private readonly DataContext _context;
		public RoomCostController(DataContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
