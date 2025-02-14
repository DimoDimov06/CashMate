using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CashMate.Models.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string UserName { get; set; }

        public int Code { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

    }
}
