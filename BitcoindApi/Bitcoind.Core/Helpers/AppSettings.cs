namespace Bitcoind.Core.Helpers
{
    public class AppSettings
    {
        public string BitcoindServer { get; set; }
        public string BitcoindUser { get; set; }
        public string BitcoindPassword { get; set; }
        public string BitcoindRpcJsonVersion { get; set; }
        public int UpdateWalletsDelayInSeconds { get; set; }
        public int UpdateTransactionsDelayInSeconds { get; set; }
        public int MaxGetTransactions { get; set; }
        public int ShowIncomeTransactionsWithConfirmationLessThan { get; set; }
        public int UpdateTransactionsWithConfirmationLessThan { get; set; }
    }
}
