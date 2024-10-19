using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace quanlykytuc.Models
{
	[Table("Menu")]
	public class Menu
	{
		[Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; }
		public string? MenuName { get; set; }
		public bool? IsActive { get; set; }
		public string? ControllerName { get; set; }
		public string? ActionName { get; set; }
		public int Levels { get; set; }
		public int ParentID { get; set; }
		public string? Link { get; set; }
		public int MenuOrder { get; set; }
		public int Position { get; set; }
        public string? ItemTarget { get; set; }
        public string? Icon { get; set; }
        public string? IdName { get; set; }
    }
}
