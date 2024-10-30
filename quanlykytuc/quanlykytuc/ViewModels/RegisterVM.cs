using System.ComponentModel.DataAnnotations;

namespace quanlykytuc.ViewModels
{
    public class RegisterVM
    {
        [Key]
        public int UserID { get; set; }
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string UserName { get; set; }
        public string Passwords { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string YourName { get; set; }
        [Display(Name = "Điện thoại")]
        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Chưa đúng định dạng di động Việt Nam")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        public string Address { get; set; }

        public int Role { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; }
    }
}
