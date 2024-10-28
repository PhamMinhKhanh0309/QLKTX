using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
		public IActionResult Index(int page = 1)
		{
            // Lọc sinh viên đã có phòng
            int pageSize = 5;
            var CostQuery = _context.roomCostPrices.AsQueryable();
            int totalCost = CostQuery.Count();
            var CostRoom = _context.roomCostPrices
                .Skip((page - 1) * pageSize)
                .Take(5)
                .Include(s => s.Room)
                .OrderBy(s => s.RoomCost)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCost / (double)pageSize);
            return View(CostRoom);
        }
        // thêm mới nha
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomCostPrice rc)
        {
            if (ModelState.IsValid)
            {
                // Tính tổng chi phí
                rc.TotalCost = rc.RoomCost + rc.ElectricityCost + rc.WaterCost + rc.OtherCost;

                _context.roomCostPrices.Add(rc);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(rc);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var he = _context.roomCostPrices.Find(id);
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
            return View(he);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoomCostPrice rc)
        {
            if (ModelState.IsValid)
            {
                // Tính tổng chi phí
                rc.TotalCost = rc.RoomCost + rc.ElectricityCost + rc.WaterCost + rc.OtherCost;
                _context.roomCostPrices.Update(rc);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(rc);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Tìm chi phí theo id
            var cost = _context.roomCostPrices.FirstOrDefault(e => e.RoomCostID == id);

            if (cost == null)
            {
                return Json(new { success = false, message = "Chi phí không tồn tại" });
            }

            // Xóa chi phí
            _context.roomCostPrices.Remove(cost);
            _context.SaveChanges();

            // Trả về JSON xác nhận đã xóa thành công
            return Json(new { success = true, message = "Xóa chi phí thành công" });
        }
    }
}
