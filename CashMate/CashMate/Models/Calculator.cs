using System.ComponentModel.DataAnnotations;

namespace CashMate.Models
{
    public class Calculator
    {
        [Required]
        public double Number1 { get; set; }

        [Required]
        public double Number2 { get; set; }

        [Required]
        public string Operation { get; set; }

        public double? Result { get; set; }
    }
}
    