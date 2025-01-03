﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quanlykytuc.Models;
using System.Net.Mail;
using System.Net;

using MimeKit;
namespace quanlykytuc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomRegistrationController : Controller
    {
        private readonly DataContext _context;
        public RoomRegistrationController(DataContext context)
        {
            _context = context;
        }
        // GET: Hiển thị danh sách các đăng ký chờ duyệt
        public IActionResult ApproveRoom()
        {
            // Lấy danh sách các đăng ký phòng chưa được duyệt (Status = 0)
            var pendingRegistrations = _context.RoomRegistrations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .Where(r => r.Status == 0)
                .ToList();

            return View(pendingRegistrations);
        }

        // POST: Duyệt đăng ký phòng cho sinh viên
        [HttpPost]
        public IActionResult ApproveRoom(int registrationId)
        {
            // Tìm bản đăng ký dựa trên registrationId
            var registration = _context.RoomRegistrations
                .Include(r => r.Student)
                .Include (r => r.Room)
                .FirstOrDefault(r => r.RegistrationID == registrationId);

            if (registration == null)
            {
                return Json(new { success = false, message = "Registration not found." });
            }

            // Cập nhật ID phòng trong bảng Student
            var student = _context.Students.FirstOrDefault(s => s.StudentID == registration.StudentID);
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == registration.RoomID);

            if (student != null)
            {
                // Gán RoomID cho sinh viên
                student.RoomID = registration.RoomID;
                //Thêm số thành viên trong phòng
                room.Quantity++;
                // Đánh dấu bản đăng ký là đã duyệt
                registration.Status = 1;
                using (MailMessage mail = new MailMessage())
                {
                    // Gửi mail từ hệ thống
                    mail.From = new MailAddress("pmk20030309@gmail.com");

                    // Tiêu đề của email
                    mail.Subject = "Thông báo đăng ký phòng ký túc xá thành công";

                    // Gửi đến email của sinh viên
                    mail.To.Add(registration.Email);  // Đảm bảo rằng bạn có thuộc tính `Email` của sinh viên

                    // Cấu hình nội dung mail là HTML
                    mail.IsBodyHtml = true;

                    // Tạo nội dung HTML cho email
                    var bodyBuilder = new BodyBuilder();

                    // Nội dung HTML của email
                    bodyBuilder.HtmlBody = $@"
<html>
<head>
    <style>
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #dddddd; text-align: left; padding: 8px; }}
        th {{ background-color: #f2f2f2; }}
        h2 {{ color: #333333; }}
    </style>
</head>
<body>
    <h2>Thông báo đăng ký phòng ký túc xá</h2>
    <p>Xin chào <strong>{student.Name}</strong>,</p>
    <p>Bạn đã đăng ký thành công phòng ký túc xá. Dưới đây là thông tin chi tiết về đăng ký của bạn:</p>
    
    <table>
        <tr><th>Tên phòng</th><td>{room.RoomName}</td></tr>
        <tr><th>Số điện thoại</th><td>{registration.Phone}</td></tr>
        <tr><th>Ngày đăng ký</th><td>{registration.RegistrationDate.ToString("dd/MM/yyyy")}</td></tr>
        <tr><th>Ngày bắt đầu</th><td>{registration.StartDate.ToString("dd/MM/yyyy")}</td></tr>
        <tr><th>Ngày kết thúc</th><td>{registration.EndDate.ToString("dd/MM/yyyy")}</td></tr>
       
    </table>
    
    <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email hoặc số điện thoại hỗ trợ.</p>
    
    <p>Trân trọng,<br/>Ban quản lý ký túc xá</p>
</body>
</html>";

                    // Thiết lập nội dung cho mail
                    mail.Body = bodyBuilder.HtmlBody;

                    // Cấu hình SMTP và gửi mail
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("pmk20030309@gmail.com", "sxhp fhdo wspm kmcf");
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;

                        // Gửi email
                        smtpClient.Send(mail);
                    }

                    // Thông báo gửi mail thành công
                    ViewBag.Message = "Email đã được gửi thành công.";

                    // Xóa ModelState để làm mới form
                    ModelState.Clear();
                }

                // Lưu thay đổi vào database
                _context.SaveChanges();

                return Json(new { success = true, message = "Phòng đã được phê duyệt và giao cho sinh viên thành công." });
            }

            return Json(new { success = false, message = "Lỗi duyệt phòng không thành công" });
        }
		public IActionResult Approved()
		{
			// Lấy danh sách các đăng ký phòng đã được duyệt (Status = 1)
			var pendingRegistrations = _context.RoomRegistrations
				.Include(r => r.Student)
				.Include(r => r.Room)
				.Where(r => r.Status == 1)
				.ToList();
			return View(pendingRegistrations);
		}
        public IActionResult Liquidation(int registrationId)
        {
            // Tìm bản đăng ký dựa trên registrationId
            var registration = _context.RoomRegistrations
                .Include(r => r.Student)
                .Include(r => r.Room)
                .FirstOrDefault(r => r.RegistrationID == registrationId);

            if (registration == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin đăng ký." });
            }

            // Cập nhật thông tin sinh viên và phòng
            var student = _context.Students.FirstOrDefault(s => s.StudentID == registration.StudentID);
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == registration.RoomID);

            if (student != null && room != null)
            {
                // Xóa ID phòng khỏi sinh viên để thanh lý hợp đồng
                student.RoomID = null;

                // Giảm số lượng thành viên trong phòng
                if (room.Quantity > 0)
                    room.Quantity--;

                // Cập nhật trạng thái của đăng ký thành đã thanh lý
                registration.Status = 2; // 2 biểu thị trạng thái thanh lý

                // Gửi email thông báo thanh lý hợp đồng cho sinh viên
                using (MailMessage mail = new MailMessage())
                {
                    // Địa chỉ email người gửi
                    mail.From = new MailAddress("pmk20030309@gmail.com");

                    // Tiêu đề của email
                    mail.Subject = "Thông báo thanh lý hợp đồng ký túc xá";

                    // Gửi đến email của sinh viên
                    mail.To.Add(registration.Email);

                    // Cấu hình nội dung mail là HTML
                    mail.IsBodyHtml = true;

                    // Tạo nội dung HTML của email
                    var bodyBuilder = new BodyBuilder();

                    // Nội dung HTML của email
                    bodyBuilder.HtmlBody = $@"
<html>
<head>
    <style>
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #dddddd; text-align: left; padding: 8px; }}
        th {{ background-color: #f2f2f2; }}
        h2 {{ color: #333333; }}
    </style>
</head>
<body>
    <h2>Thông báo thanh lý hợp đồng ký túc xá</h2>
    <p>Xin chào <strong>{student.Name}</strong>,</p>
    <p>Hợp đồng thuê phòng ký túc xá của bạn đã được thanh lý. Dưới đây là thông tin chi tiết:</p>
    
    <table>
        <tr><th>Tên phòng</th><td>{room.RoomName}</td></tr>
        <tr><th>Số điện thoại</th><td>{registration.Phone}</td></tr>
        <tr><th>Ngày đăng ký</th><td>{registration.RegistrationDate.ToString("dd/MM/yyyy")}</td></tr>
        <tr><th>Ngày bắt đầu</th><td>{registration.StartDate.ToString("dd/MM/yyyy")}</td></tr>
        <tr><th>Ngày kết thúc</th><td>{registration.EndDate.ToString("dd/MM/yyyy")}</td></tr>
    </table>
    
    <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email hoặc số điện thoại hỗ trợ.</p>
    
    <p>Trân trọng,<br/>Ban quản lý ký túc xá</p>
</body>
</html>";

                    // Thiết lập nội dung cho mail
                    mail.Body = bodyBuilder.HtmlBody;

                    // Cấu hình SMTP và gửi mail
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("pmk20030309@gmail.com", "sxhp fhdo wspm kmcf");
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;

                        // Gửi email
                        smtpClient.Send(mail);
                    }

                    // Thông báo gửi mail thành công
                    ViewBag.Message = "Email thanh lý hợp đồng đã được gửi thành công.";
                }

                // Lưu thay đổi vào database
                _context.SaveChanges();

                return Json(new { success = true, message = "Hợp đồng đã được thanh lý thành công và email đã được gửi cho sinh viên." });
            }

            return Json(new { success = false, message = "Lỗi thanh lý hợp đồng." });
        }


    }
}
