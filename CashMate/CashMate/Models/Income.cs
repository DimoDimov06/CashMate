using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class Income
    {
        public int Id { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
