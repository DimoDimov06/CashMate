namespace CashMate.Controllers
{
    using CashMate.Models;
    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.JsonWebTokens;
    using Microsoft.IdentityModel.Tokens;
    using MimeKit;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.WebPages;
    using Microsoft.AspNetCore.Identity;

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
       
        public AccountController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // Регистрация
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                bool emailExists = _db.Users.Any(u => u.Email == model.Email);
                if (emailExists)
                {
                    // Имейлът вече съществува в базата данни
                    TempData["ErrorMessage"] = "There is a registered user with this email address.!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                        // Хеширане на паролата

                    var passwordHash = HashPassword(model.Password);
                    var code = GenerateRandomCode();
                    var user = new User
                    {
                        Email = model.Email,
                        PasswordHash = passwordHash,
                        UserName = model.UserName,
                        IsEmailConfirmed = false,
                        Code = int.Parse(code ?? "0")
                    };
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    // Генериране на код и изпращане на имейл
                    SendEmail(user.Email, code);

                    // Записване на кода в сесия или база данни
                    HttpContext.Session.SetString("UserId", user.Id.ToString());

                    return RedirectToAction("ConfirmEmail");

                }
            }
            return View(model);
        }

        // Логин
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public Task<IActionResult> Login(LoginViewMode)
        public Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _db.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                if (!user.IsEmailConfirmed)
                {
                    var code = GenerateRandomCode();
                    user.Code = int.Parse(code ?? "0");
                    _db.SaveChanges();
                    // Генериране на код и изпращане на имейл
                    SendEmail(user.Email, code);

                    // Записване на кода в сесия или база данни
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    return Task.FromResult<IActionResult>(RedirectToAction("ConfirmEmail"));
                }
                else
                {


                    // Записване на ID на потребителя

                    var claims = new[]
                    {
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                         _config["Jwt:Issuer"],
                         _config["Jwt:Audience"],
                         claims,
                         expires: DateTime.UtcNow.AddMinutes(30),
                         signingCredentials: singIn);
                    var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                    HttpContext.Session.SetString("Token", tokenValue);
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserName", user.UserName.ToString());
                    return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
                }
            }
            else
            {
                ModelState.AddModelError("", "Incorect Email or Password!");
                return Task.FromResult<IActionResult>(BadRequest(ModelState)); // Връщане на BadRequest с моделната грешка
            }
        }
     
        // Хеширане на паролата
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Проверка на паролата
        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
        // Генериране на случайен 5-цифрен код
        private string GenerateRandomCode()
        {
            Random random = new Random();
            return random.Next(10000, 99999).ToString();
        }
        // Изпращане на имейл
        private void SendEmail(string email, string code)
        {
            var fromAddress = new MailboxAddress("CashMate", "test@service.bg");
            var toAddress = new MailboxAddress("", email);
            const string fromPassword = "46sMQDi0QYd1SaC@"; // Задайте паролата на вашия имейл
            const string subject = "Email Confirmation Code";
            string body = $"Вашият код за потвърждение е: {code}";

            var message = new MimeMessage();
            message.From.Add(fromAddress);
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("mail.livezone.org", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(fromAddress.Address, fromPassword);

                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }

        }
        [HttpGet]
        public ActionResult ConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmEmail(string code)
        {
            var userId = HttpContext.Session.GetString("UserId").AsInt();
            var user = _db.Users.Find(userId);
            if (user != null)
            {
                var sessionCode = user.Code.ToString();

                if (sessionCode == code)

                {
                    user.IsEmailConfirmed = true;
                    _db.SaveChanges();
                    HttpContext.Session.Remove("UserId");
                   
                    // Добавяне на съобщение в TempData
                    TempData["SuccessMessage"] = "Your email has been successfully verified.!";
                    return RedirectToAction("Index", "Home");
                }
            }

            // Добавяне на съобщение за грешка в TempData
            TempData["ErrorMessage"] = "Invalid verification code!";
            return RedirectToAction("Index", "Home");
        }
    }
}
