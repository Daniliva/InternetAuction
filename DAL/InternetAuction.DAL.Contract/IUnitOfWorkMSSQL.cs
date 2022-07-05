using InternetAuction.DAL.Entities.MSSQL;
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

        IRepositoryMsSqlWithImage<Lot, int> LotRepository { get; }
        IRepositoryMsSql<Autction, int> AutctionRepository { get; }
        IRepositoryMsSql<AutctionStatus, int> AutctionStatusRepository { get; }
        IRepositoryMsSql<Bidding, int> BiddingRepository { get; }
        IRepositoryMsSql<ImageId, int> ImageIdRepository { get; }
        IRepositoryMsSql<LotCategory, int> LotCategoryRepository { get; }
        IRepositoryMsSql<RoleUser, string> RoleUserRepository { get; }
        IRepositoryMsSql<Role, string> RoleRepository { get; }
        IRepositoryMsSqlWithImage<User, string> UserRepository { get; }

        Task SaveAsync();
    }
}