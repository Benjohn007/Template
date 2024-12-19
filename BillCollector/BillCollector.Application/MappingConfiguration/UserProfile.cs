using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillCollector.Core.Entities;

namespace BillCollector.Application.MappingConfiguration
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            AllowNullCollections = true;
            AddGlobalIgnore("Item");

            // Default mapping when property names are same
            //CreateMap<User, UserDto>().ReverseMap();

            // Sample mapping when property names are different
            //CreateMap<User, UserDto>()
            //    .ForMember(dest =>
            //    dest.ApiUserName,
            //    opt => opt.MapFrom(src => src.Name))
            //    .ForMember(dest =>
            //    dest.ApiUserStatus,
            //    opt => opt.MapFrom(src => src.Status));
        }
    }
}
