using System.Collections.Generic;

namespace Bitcoind.Core.Bitcoind.DTO
{
    public class BitcoinSingleTransactionDto
    {
        public int Confirmations { get; set; }
        public IEnumerable<Details> Details { get; set; }
        public string Txid { get; set; }
        public int Time { get; set; }
    }

    public class Details
    {
        public string Address { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }
}
