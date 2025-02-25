using AssignmentPS42054.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AssignmentPS42054.Models;
using AssignmentPS42054.DAL;
using AssignmentPS42054.Class;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace AssignmentPS42054.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration configuration;
        private readonly AccountDAL _accountDAL;
        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
            _accountDAL = new AccountDAL(configuration);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                AccountDAL userDAL = new AccountDAL(configuration);


                var result = userDAL.Register(user.Name, user.Pass, user.Email, user.Mobile);
                if (result)
                {
                    ViewBag.Message = "Đăng ký thành công!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = "Tên đăng nhập đã tồn tại!";
                }
            }

            return View(user);
        }
        public IActionResult Login()
        {
            AccountDAL userDAL = new AccountDAL(configuration);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, int Remember)
        {
            AccountDAL userDAL = new AccountDAL(configuration);
            Users user1 = userDAL.Login(userName, password);

            if (user1 != null)
            {

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user1.Name),
            new Claim(ClaimTypes.Role, user1.roleID == 1 ? "Admin" : "User") 
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Remember == 1, 
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
                };


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                int idUser = userDAL.GetIDbyUser(userName);
                HttpContext.Session.SetString("UserID", idUser.ToString());

                if (Remember == 1)
                {

                    CookieOptions options = new CookieOptions()
                    {
                        Domain = "localhost",
                        Path = "/",
                        Expires = DateTime.Now.AddDays(1),
                        Secure = false,
                        HttpOnly = true,
                        IsEssential = true,
                    };

                    Response.Cookies.Append("UserName", userName, options);
                }
                else
                {

                    HttpContext.Session.SetString("UserName", user1.Name);
                    
                }


                if (user1.roleID == 1)
                {
                    ViewBag.Info = "Đăng nhập thành công với tư cách admin";
                    return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                }
                else
                {
                    ViewBag.Info = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Info = "Sai Username hoặc password";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            CookieOptions options = new CookieOptions
            {
                Domain = "localhost",
                Path = "/"
            };
            Response.Cookies.Delete("UserName", options);
            HttpContext.Session.Remove("UserName");

            // Điều hướng người dùng về trang Login hoặc trang Home sau khi đăng xuất
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Edit(int id)
        {
            string userIdString = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdString))
            {
                // Nếu session không có userID, có thể điều hướng về trang đăng nhập hoặc trả về lỗi
                return RedirectToAction("Login", "Account");
            }
            int idUser = int.Parse(userIdString);
            var user = _accountDAL.GetUserById(idUser);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin người dùng
                _accountDAL.UpdateUser(user);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                // Nếu model không hợp lệ, trả về form chỉnh sửa cùng với thông tin đã nhập
                return View(user);
            }
        }
        public IActionResult AccessDenied()
        {
            return View(); // Tạo một view AccessDenied.cshtml để hiển thị thông báo
        }

    }

}
