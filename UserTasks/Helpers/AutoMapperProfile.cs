using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserTasks.Core.Authorization;
using UserTasks.Core.Entities;
using UserTasks.ViewModels;

namespace UserTasks.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>().ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<UserViewModel, User>().ForMember(d => d.Roles, map => map.Ignore());

            CreateMap<Role, RoleViewModel>()
                .ForMember(d => d.Permissions, map => map.MapFrom(s => s.Claims))
                .ForMember(d => d.UsersCount, map => map.ResolveUsing(s => s.Users?.Count ?? 0))
                .ReverseMap();
            CreateMap<RoleViewModel, Role>();

            CreateMap<IdentityRoleClaim<string>, ClaimViewModel>()
                .ForMember(d => d.Type, map => map.MapFrom(s => s.ClaimType))
                .ForMember(d => d.Value, map => map.MapFrom(s => s.ClaimValue))
                .ReverseMap();

            CreateMap<ApplicationPermission, PermissionViewModel>().ReverseMap();

            CreateMap<IdentityRoleClaim<string>, PermissionViewModel>()
                .ConvertUsing(s => Mapper.Map<PermissionViewModel>(ApplicationPermissions.GetPermissionByValue(s.ClaimValue)));

            CreateMap<TaskItem, TaskItemViewModel>().ReverseMap();
        }
    }
}
