using System;

namespace Bitcoind.Core.Bitcoind
{
    public class BitcoindException: Exception
    {
        public BitcoindException()
        { }

        public BitcoindException(string message)
            :base(message)
        {
        }
    }
}
