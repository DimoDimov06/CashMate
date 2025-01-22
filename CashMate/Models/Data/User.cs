namespace CashMate.Models.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsEmailConfirmed { get; set; }=false;

        public ICollection<Purchase> Purchases { get; set; }

    }
}
