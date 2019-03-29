namespace Bitcoind.Core.Bitcoind.DTO
{
    public interface IResponse<T>
    {
        T Result { get; set; }
        Error Error { get; set; }
    }
}
