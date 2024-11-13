using Microsoft.AspNetCore.Mvc;
using CashMate.Models;

namespace CashMate.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(Calculator model)
        {
            if (ModelState.IsValid)
            {
                switch (model.Operation)
                {
                    case "Add":
                        model.Result = model.Number1 + model.Number2;
                        break;
                    case "Subtract":
                        model.Result = model.Number1 - model.Number2;
                        break;
                    case "Multiply":
                        model.Result = model.Number1 * model.Number2;
                        break;
                    case "Divide":
                        model.Result = model.Number2 != 0 ? model.Number1 / model.Number2 : double.NaN;
                        break;
                }
            }

            return View("Index", model);
        }
    }
}
