namespace CashMate.Controllers
{

    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.JsonWebTokens;
    using Microsoft.IdentityModel.Tokens;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    
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
        public ActionResult Register(string email, string password, string usarName)
        {
            if (ModelState.IsValid)
            {
                // Хеширане на паролата
                var passwordHash = HashPassword(password);
                var user = new User { Email = email, PasswordHash = passwordHash, UserName = usarName, IsEmailConfirmed = false};
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }

        // Логин
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public Task<IActionResult> Login(string email, string password)
        public Task<IActionResult> Login(string email, string password)
        {
            var user = _db.Users.SingleOrDefault(u => u.Email == email);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                // Записване на ID на потребителя

                var claims = new[]
                {
                     new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,
                        _config["Jwt:Subject"]),
                     new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,
                         Guid.NewGuid().ToString()),
                     new Claim("UserId", user.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: singIn
                    );
                var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                HttpContext.Items["Token"] = tokenValue;
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                return Task.FromResult<IActionResult>(RedirectToAction("Index", "Home"));
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
    }
}
