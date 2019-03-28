namespace Bitcoind.Core.Bitcoind.Dto
{
    public class TransactionDto
    {
        //public string Account { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        //public int Vout { get; set; }
        //public decimal Fee { get; set; }
        public int Confirmations { get; set; }
        //public string Blockhash { get; set; }
        //public int Blockindex { get; set; }
        //public long Blocktime { get; set; }
        public string Txid { get; set; }
        public int Time { get; set; }
        //public long TimeReceived { get; set; }
    }
}
