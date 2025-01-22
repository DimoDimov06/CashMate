namespace CashMate.Controllers
{

    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;


    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PurchasesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Entering()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetPurchases(DateTime startDate, DateTime endDate)
        {
            var purchases = _db.Purchases
                .Where(p => p.PurchaseDate >= startDate && p.PurchaseDate <= endDate)
                .ToList();

            return PartialView("_PurchasesList", purchases);
        }
    }
}
