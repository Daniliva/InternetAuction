using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using InternetAuction.DAL.MongoDB;
using InternetAuction.DAL.MSSQL;
using InternetAuction.DAL.MSSQL.Repositories.Data;
using InternetAuction.DAL.MSSQL.Repositories.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetAuction.DAL.Repositories
{
	public class UnitOfWorkMSSQL : IUnitOfWorkMSSQL
	{
		private MsSqlContext msSqlContext;

		public IRepositoryMsSqlWithImage<Lot, int> LotRepository { get; private set; }

		public IRepositoryMsSql<Autction, int> AutctionRepository { get; private set; }

		public IRepositoryMsSql<AutctionStatus, int> AutctionStatusRepository { get; private set; }

		public IRepositoryMsSql<Bidding, int> BiddingRepository { get; private set; }

		public IRepositoryMsSql<ImageId, int> ImageIdRepository { get; private set; }

		public IRepositoryMsSql<LotCategory, int> LotCategoryRepository { get; private set; }

		public IRepositoryMsSql<RoleUser, string> RoleUserRepository { get; private set; }

		public IRepositoryMsSql<Role, string> RoleRepository { get; private set; }

		public IRepositoryMsSqlWithImage<User, string> UserRepository { get; private set; }
		//  public ApplicationRoleManager ApplicationRoleManager { get; private set; }
		//   public ApplicationUserManager ApplicationUserManager { get; private set; }

		public UnitOfWorkMSSQL(string[] connections)
		{
			msSqlContext = new MsSqlContext(connections[0]);
			ImageContext mongoDB = new ImageContext(connections[1]);
			LotRepository = new LotRepository(msSqlContext, mongoDB);
			UserRepository = new UserRepository(msSqlContext, mongoDB);
			AutctionRepository = new AutctionRepository(msSqlContext);

			AutctionStatusRepository = new AutctionStatusRepository(msSqlContext);
			BiddingRepository = new BiddingRepository(msSqlContext);
			ImageIdRepository = new ImageIdRepository(msSqlContext);
			LotCategoryRepository = new LotCategoryRepository(msSqlContext);
			//   RoleUserRepository = new RoleUserRepository(msSqlContext);

			RoleRepository = new RoleRepository(msSqlContext);

			/* IOptions<IdentityOptions> optionsAccessor = new Options();
             IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
             IEnumerable<IUserValidator<User>> userValidators = new List<IUserValidator<User>>();
             IEnumerable<IPasswordValidator<User>> passwordValidators = new List<IPasswordValidator<User>>();
             ILookupNormalizer keyNormalizer = new LookupNormalizer();
             IdentityErrorDescriber errors = new IdentityErrorDescriber();
             IServiceProvider services = new ServiceProvider();

             IEnumerable<IRoleValidator<Role>> roleValidators = new List<IRoleValidator<Role>>();

             ILogger<ApplicationUserManager> logger = new Logger();

             LoggerRole loggerRole = new LoggerRole();
             ApplicationUserManager = new ApplicationUserManager(new UserStore<User>(msSqlContext), optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger);
             ApplicationRoleManager = new ApplicationRoleManager(new RoleStore<Role>(msSqlContext), roleValidators, keyNormalizer, errors, loggerRole);
            */
			RoleUserRepository = new RoleUserRepository(msSqlContext);
		}

		public async Task SaveAsync()
		{
			await msSqlContext.SaveChangesAsync();
		}
	}

	public class Options : IOptions<IdentityOptions>
	{
		IdentityOptions IOptions<IdentityOptions>.Value => new IdentityOptions();
	}

	public class LookupNormalizer : ILookupNormalizer
	{
		public string NormalizeEmail(string email)
		{
			return Normalize(email);
		}

		public string NormalizeName(string name)
		{
			return Normalize(name);
		}

		private string Normalize(string str)
		{
			var strWithoutSpaces = str.Replace(" ", "").Trim();
			var strWithout = strWithoutSpaces.Replace("\n", "").Replace("\t", "");
			return strWithout;
		}
	}

	/*
    public class ServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public class Logger : ILogger<ApplicationUserManager>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }

    public class LoggerRole : ILogger<RoleManager<Role>>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }*/
}