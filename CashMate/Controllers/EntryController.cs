namespace CashMate.Controllers
{

    using CashMate.Models;
    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using Microsoft.AspNetCore.Http;
    using System.Web.WebPages;

    public class EntryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EntryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Purchases/Create
   
        [HttpGet]
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (userName != null)
            {
                ViewBag.UserId = userName;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Purchases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EntryViewModel model)
        {
            var userId = HttpContext.Session.GetString("UserId").AsInt();
            if (ModelState.IsValid)
            {
                var purchase = new Purchase();
                purchase.PurchaseDate = model.PurchaseDate;
                purchase.Debit = model.Debit;
                purchase.Credit = model.Credit;
                purchase.Description = model.Description;
                purchase.UserID = userId;


                _db.Purchases.Add(purchase);
                _db.SaveChanges();
                return RedirectToAction("Index"); // или където искате да пренасочите
            }
            return View(model);
            
        }

        private int Parse(string? userId)
        {
            throw new NotImplementedException();
        }
    }
}


