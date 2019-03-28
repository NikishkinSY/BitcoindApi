using System;

namespace Bitcoind.Core.DAL.Entities
{
    public class Transaction
    {
        public string Wallet { get; set; }
        public Category Category { get; }
        public string Txid { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public int Confirmations { get; set; }
        public DateTime Date { get; set; }

        public override bool Equals(object other)
        {
            var transaction = other as Transaction;
            return transaction != null && Txid.Equals(transaction.Txid, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Txid.GetHashCode();
        }
    }
}
