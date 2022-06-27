using System.Threading.Tasks;

namespace InternetAuction.DAL.Contract
{
    public interface IUnitOfWorkMSSQL
    {
        /*    ICustomerRepository CustomerRepository { get; }

            IPersonRepository PersonRepository { get; }

            IProductRepository ProductRepository { get; }

            IProductCategoryRepository ProductCategoryRepository { get; }

            IReceiptRepository ReceiptRepository { get; }

            IReceiptDetailRepository ReceiptDetailRepository { get; }*/

        Task SaveAsync();
    }
}