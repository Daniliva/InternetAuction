using AutoMapper;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.BLL.Service;
using InternetAuction.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternetAuction.BLL
{
    public class ServiceFactory : IFactory
    {
        private readonly Dictionary<Type, object> _managerCollection;

        public ServiceFactory(IUnitOfWorkMSSQL unitOfWork)
        {
            AutomapperProfile autoMapper = new AutomapperProfile();
            _managerCollection = new Dictionary<Type, object>();
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();

            _managerCollection.Add(typeof(ICrud<AutctionModel, int>), new AuctionService(unitOfWork, mapper));

            _managerCollection.Add(typeof(ICrud<AutctionStatusModel, int>), new AuctionStatusService(unitOfWork, mapper));

            _managerCollection.Add(typeof(ICrud<BiddingModel, int>), new BiddingService(unitOfWork, mapper));
            _managerCollection.Add(typeof(ICrud<LotCategoryModel, int>), new LotCategoryService(unitOfWork, mapper));
            _managerCollection.Add(typeof(ICrud<LotModel, int>), new LotService(unitOfWork, mapper));
            _managerCollection.Add(typeof(ICrud<RoleModel, string>), new RoleService(unitOfWork, mapper));
            _managerCollection.Add(typeof(ICrud<RoleUserModel, string>), new RoleUserService(unitOfWork, mapper));

            _managerCollection.Add(typeof(IExpansionGetEmail<UserModel, string>), new UserService(unitOfWork, mapper));
        }

        public T Get<T>()
        {
            var type = typeof(T);
            return (T)_managerCollection[type];
        }
    }

    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());    // mapping between Business and DB layer objects
                                                            //mapping between Web and Business layer objects
            });

            return config;
        }
    }
}