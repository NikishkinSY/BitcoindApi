namespace BitcoindApi.DAL.Entities
{
    public class OutcomeTransaction: Transaction
    {
        public override Category Category => Category.Receive;
    }
}
