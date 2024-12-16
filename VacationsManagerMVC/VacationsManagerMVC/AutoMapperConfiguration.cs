using AutoMapper;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration() {
            CreateMap<Notification,NotificationDto>().ReverseMap();
            CreateMap<NotificationDto, NotificationEditVM>().ReverseMap();
            CreateMap<NotificationDto, NotificationDetailsVM>().ReverseMap();

            CreateMap<Project,ProjectDto>().ReverseMap();
            CreateMap<ProjectDto, ProjectEditVM>().ReverseMap();
            CreateMap<ProjectDto, ProjectDetailsVM>().ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<RoleDto, RoleEditVM>().ReverseMap();
            CreateMap<RoleDto, RoleDetailsVM>().ReverseMap();

            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<TeamDto, TeamEditVM>().ReverseMap();
            CreateMap<TeamDto, TeamDetailsVM>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserDto, UserEditVM>().ReverseMap();
            CreateMap<UserDto, UserDetailsVM>().ReverseMap();

            CreateMap<VacationRequest, VacationRequestDto>().ReverseMap();
            CreateMap<VacationRequestDto, VacationRequestEditVM>().ReverseMap();
            CreateMap<VacationRequestDto, VacationRequestDetailsVM>().ReverseMap();
        }
    }   
}
