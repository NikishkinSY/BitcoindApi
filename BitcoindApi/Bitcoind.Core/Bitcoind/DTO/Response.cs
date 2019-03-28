namespace Bitcoind.Core.Bitcoind
{
    public class Response<T>
    {
        public T Result { get; set; }
        public Error Error { get; set; }
    }
}
