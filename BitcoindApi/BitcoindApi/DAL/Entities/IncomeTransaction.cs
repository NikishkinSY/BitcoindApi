namespace BitcoindApi.DAL.Entities
{
    public class IncomeTransaction: Transaction
    {
        public override Category Category => Category.Receive;
    }
}
