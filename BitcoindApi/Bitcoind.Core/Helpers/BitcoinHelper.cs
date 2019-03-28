namespace Bitcoind.Core.Helpers
{
    public static class BitcoinHelper
    {
        // https://ru.bitcoinwiki.org/wiki/%D0%90%D0%B4%D1%80%D0%B5%D1%81
        public static bool CheckAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            if (address.Length < 26)
                return false;

            if (address.Length > 35)
                return false;

            return true;
        }
    }
}
