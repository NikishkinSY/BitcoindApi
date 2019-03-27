using System;

namespace BitcoindApi.DAL.Entities
{
    public abstract class Transaction
    {
        public abstract Category Category { get; }
        public string Txid { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public int Confirmations { get; set; }
        public DateTime Date { get; set; }
    }
}
