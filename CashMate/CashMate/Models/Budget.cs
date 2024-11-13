using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Limit { get; set; }

        public decimal Spent { get; set; } = 0;
    }
}
