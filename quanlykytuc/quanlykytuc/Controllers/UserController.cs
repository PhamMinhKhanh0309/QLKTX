using quanlykytuc.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using quanlykytuc.Models;
using quanlykytuc.Utilities;
using System.Security.Claims;
using AutoMapper;
using quanlykytuc.Helpers;

namespace quanlykytuc.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Mapping thông tin từ model RegisterVM sang đối tượng User
                var user = _mapper.Map<User>(model);
                user.Passwords = Functions.MD5Password(user.Passwords);
                user.IsActive = true; // sẽ xử lý kích hoạt qua email
                user.Role = 2; // Giả sử 2 là role cho sinh viên

                // Lưu thông tin user vào cơ sở dữ liệu
                _context.Users.Add(user);
                _context.SaveChanges(); // Lưu để lấy ID của user mới tạo

                // Tạo bản ghi cho bảng Student liên kết với user
                var student = new Student
                {
                    UserID = user.UserID, // Liên kết với user vừa tạo
                    Name = model.YourName, // Giả sử RegisterVM có trường FullName
                    Phone = model.Phone,
                    Address = model.Address,
                    Email = model.Email,
                    RoomID = null // Ban đầu chưa gán phòng
                                  // Các thuộc tính khác của Student tùy vào nhu cầu
                };

                _context.Students.Add(student); // Thêm vào bảng Student
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                TempData["SuccessMessage"] = "Đăng ký thành công! Bạn có thể đăng nhập ngay bây giờ.";
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = _context.Users.SingleOrDefault(kh => kh.UserName == model.UserName);
                string ma = Functions.MD5Password(model.Passwords);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có tài khoản này");
                }
                else
                {
                    if (!khachHang.IsActive)
                    {
                        ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                    }
                    else
                    {
                        if (khachHang.Passwords != ma)
                        {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                        }
                        else
                        {
                            var claims = new List<Claim> {
                               /* new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.YourName),*/
                                new Claim(ClaimTypes.Name, khachHang.UserName),
                                new Claim("ID", khachHang.UserID.ToString()),

								//claim - role động
                                
								new Claim(ClaimTypes.Role, "1"),
                                new Claim(ClaimTypes.Role, "2")

                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            if(khachHang.Role == 2)
                            {
                                return RedirectToAction("HomeIndex", "Home");
                            }else
                            {
                                return RedirectToAction("Index", "Home", new { area = "Admin" }); // Chuyển đến trang chính của Admin
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
