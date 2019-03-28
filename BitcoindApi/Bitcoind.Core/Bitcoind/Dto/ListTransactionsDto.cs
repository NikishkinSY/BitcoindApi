using System.Collections.Generic;

namespace Bitcoind.Core.Bitcoind.Dto
{
    public class ListTransactionsDto
    {
        public List<BitcoinTransactionDto> Result { get; set; }
    }
}
