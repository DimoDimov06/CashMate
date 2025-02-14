namespace CashMate.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml.Linq;
    using CashMate.Models;
    using CashMate.Models.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Web.WebPages;

    public class ReadAccountXMLController : Controller
    {
        private readonly ApplicationDbContext _db;


        public ReadAccountXMLController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult UpLoad()
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                int userId = HttpContext.Session.GetString("UserId").AsInt();
                var xmlDataList = new List<AccountMovementsResult>();
                var xmlContent = await new StreamReader(file.OpenReadStream()).ReadToEndAsync();

                using (var stringReader = new StringReader(xmlContent))
                {

                    var serializer = new XmlSerializer(typeof(AccountMovementsResult), new XmlRootAttribute("AccountMovementsResult"));
                    // Логирайте xmlContent или го покажете в конзолата
                    try
                    {
                        var accountMovementsResult = (AccountMovementsResult)serializer.Deserialize(stringReader);
                        xmlDataList.Add(accountMovementsResult); // Add the deserialized object to the list
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Логирайте или обработете грешката
                        TempData["ErrorMessage"] = "XML десериализацията е неуспешна: " + ex.Message;
                        return View("Index", "Home");
                    }
                }

                // Запис в базата данни
                foreach (var accountMovement in xmlDataList.SelectMany(x => x.AccountMovements))
                {
                    var purchase = new Purchase
                    {
                        PurchaseDate = accountMovement.Date,
                        Credit = accountMovement.MovementType == "CR" ? accountMovement.Amount : 0,
                        Debit = accountMovement.MovementType == "DR" ? accountMovement.Amount : 0,
                        Description = accountMovement.Reason,
                        UserID = userId
                    };

                    _db.Purchases.Add(purchase);
                    }
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Your XML file is success load!";
                return RedirectToAction("Index", "Home");
            }
            TempData["ErrorMessage"] = "file processing failed!";
            return View("Index", "Home");
        }

        
    }
}
