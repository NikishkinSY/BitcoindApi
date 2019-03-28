using System;

namespace Bitcoind.Core.Dto
{
    public class TransactionDto
    {
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public int Confirmations { get; set; }
        public DateTime Date { get; set; }
    }
}
