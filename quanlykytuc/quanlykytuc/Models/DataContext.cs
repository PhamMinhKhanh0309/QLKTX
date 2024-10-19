using Microsoft.EntityFrameworkCore;
using quanlykytuc.Areas.Admin.Models;

namespace quanlykytuc.Models
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
		public DbSet<Menu> Menus { get; set; }
        public DbSet<AdminMenu> AdminMenus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Room> Rooms { get; set; }
		public DbSet<Notification> Notifications {  get; set; } 
		public DbSet<RoomCostPrice> roomCostPrices { get; set; }

    }

}
