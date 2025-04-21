using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CashMate.Models
{
    public class EntryViewModel
    {

        public int? Id { get; set; }
        [DisplayName("Дата....")]
        [Required(ErrorMessage = "Date is required")]
        public DateTime PurchaseDate { get; set; }

        public string PurchaseDateFormatted
        {
            get { return PurchaseDate.ToString("yyyy-MM-dd"); }
            set { PurchaseDate = DateTime.Parse(value); }
        }
        
        public decimal? Debit { get; set; }

        
        public string DebitFormatted
        {
            get => Debit != null ? Debit.Value.ToString("F2", CultureInfo.InvariantCulture) : "";
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    Debit = parsedValue;
                }
                else
                {
                    Debit = null; // Ако парсването не успее, задаваме null
                }
            }
        }

        public decimal? Credit { get; set; }

        public string CreditFormatted
        {
            get => Credit != null ? Credit.Value.ToString("F2", CultureInfo.InvariantCulture) : "";
            set
            {
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedValue))
                {
                    Credit = parsedValue;
                }
                else
                {
                    Credit = null; // Ако парсването не успее, задаваме null
                }
            }
        }

        public string? Description { get; set; }

    }
}
