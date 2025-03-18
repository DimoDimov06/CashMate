using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class EntryViewModel
    {

        public int? Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime PurchaseDate { get; set; }

        public string PurchaseDateFormatted
        {
            get { return PurchaseDate.ToString("yyyy-MM-dd"); }
            set { PurchaseDate = DateTime.Parse(value); }
        }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public string? Description { get; set; }

    }
}
