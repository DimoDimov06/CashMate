namespace CashMate.Controllers
{
    using CashMate.Models;
    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Web.WebPages;


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
            
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var purchases = _db.Purchases
                .Where(p => p.PurchaseDate >= startDate && p.PurchaseDate <= endDate)
                .ToList();

            return PartialView("_PurchasesList", purchases);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var purchase = _db.Purchases.Find(id);
            if (purchase == null)
            {
                return NotFound();
            }

            var model = new EntryViewModel
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                Debit = purchase.Debit,
                Credit = purchase.Credit,
                Description = purchase.Description
            };

            return View("Edit", model); // Използваме същата форма за въвеждане за редактиране
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EntryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var purchase = _db.Purchases.Find(model.Id);
                if (purchase == null)
                {
                    return NotFound();
                }

                purchase.PurchaseDate = model.PurchaseDate;
                purchase.Debit = model.Debit;
                purchase.Credit = model.Credit;
                purchase.Description = model.Description;

                _db.SaveChanges();
                return RedirectToAction("Entering", "Home"); // Пренасочване към списъка с покупки
            }
            else
            {
                // Проверка за валидни стойности в Debit и Credit
                if (!decimal.TryParse(model.Debit.ToString(), out _))
                {
                    ModelState.AddModelError("Debit", "Debit трябва да бъде валидно число.");
                }
                if (!decimal.TryParse(model.Credit.ToString(), out _))
                {
                    ModelState.AddModelError("Credit", "Credit трябва да бъде валидно число.");
                }
                return View("Index", model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, DateTime startDate, DateTime endDate)
        {
            var purchase = _db.Purchases.Find(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _db.Purchases.Remove(purchase);
            _db.SaveChanges();

            return RedirectToAction("Entering", "Home");
        }
    }
}


