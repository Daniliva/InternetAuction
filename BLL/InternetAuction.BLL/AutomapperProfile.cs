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
            CreateMap<Role, RoleModel>();
            CreateMap<RoleUser, RoleUserModel>();

        }
    }
}