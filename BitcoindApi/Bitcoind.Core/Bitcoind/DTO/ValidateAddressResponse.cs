namespace Bitcoind.Core.Bitcoind.DTO
{
    public class ValidateAddressResponse
    {
        public ValidateAddressResult Result { get; set; }
    }

    public class ValidateAddressResult
    {
        public bool Isvalid { get; set; }
    }
}
