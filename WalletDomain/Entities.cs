namespace WalletDomain
{
    public class Wallet
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Direction Direction { get; set; }
        public string Account { get; set; }

    }


    public class WalletDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public Direction Direction { get; set; }
        public string? Account { get; set; }

    }


    public enum Direction { Debit,Credit}
}