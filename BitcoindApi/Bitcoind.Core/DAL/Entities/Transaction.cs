using System;

namespace Bitcoind.Core.DAL.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Wallet { get; set; }
        public Category Category { get; set; }
        public string Txid { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public int Confirmations { get; set; }
        public DateTime Date { get; set; }
        public bool IsShown { get; set; }

        public override bool Equals(object other)
        {
            var transaction = other as Transaction;
            return transaction != null 
                && Id == transaction.Id 
                && Txid.Equals(transaction.Txid, StringComparison.InvariantCultureIgnoreCase)
                && Category == transaction.Category;
        }

        public override int GetHashCode()
        {
            return $"{Id}_{Txid}_{(int)Category}".GetHashCode();
        }
    }
}
