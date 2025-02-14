using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class EntryViewModel
    {

        [Required(ErrorMessage = "Date is required")]
        public DateTime PurchaseDate { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        [StringLength(100, ErrorMessage = "`Description must be at least 6 characters long.", MinimumLength = 10)]
        public string? Description { get; set; }

        //public string FormattedPurchaseDate => PurchaseDate.ToString("dd/MM/yyyy"); // Форматирана дата
    }
}
