using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
