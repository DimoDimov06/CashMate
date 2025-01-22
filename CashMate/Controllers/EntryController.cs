namespace CashMate.Controllers
{

    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.IdentityModel.Tokens;

    [Authorize]
    public class EntryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EntryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Purchases/Create

       public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Purchases/Create
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PurchaseDate,Debit,Credit,Description,UserId")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _db.Add(purchase);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // или където искате да пренасочите
            }
            return View(purchase);
        }
    }
}


