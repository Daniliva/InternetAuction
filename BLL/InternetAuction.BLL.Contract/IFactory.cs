namespace InternetAuction.BLL.Contract
{
    /// <summary>
    /// The factory.
    /// </summary>
    public interface IFactory
    {
        T Get<T>();
    }
}