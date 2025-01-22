namespace CashMate.Models.Data;

using System.ComponentModel.DataAnnotations;

public class Purchase
{
    public int Id { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public string? Description { get; set; }

    public int UserID { get; set; }
    public User User { get; set; }
}