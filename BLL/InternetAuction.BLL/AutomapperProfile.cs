using AutoMapper;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities.MSSQL;
using System;
using System.Linq;

namespace InternetAuction.BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<LotCategory, LotCategoryModel>()
           .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Lots.Select(rd => rd.Id)))
           .ReverseMap();

            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>();

            CreateMap<RoleUser, RoleUserModel>();
            CreateMap<RoleUserModel, RoleUser>();

            CreateMap<Lot, LotModel>();
            CreateMap<LotModel, Lot>();

            CreateMap<LotCategory, LotCategoryModel>();
            CreateMap<LotCategoryModel, LotCategory>();

            CreateMap<Autction, AutctionModel>();
            CreateMap<AutctionModel, Autction>();

            CreateMap<AutctionStatus, AutctionStatusModel>();
            CreateMap<AutctionStatusModel, AutctionStatus>();

            CreateMap<Bidding, BiddingModel>();
            CreateMap<BiddingModel, Bidding>();
        }
    }
}