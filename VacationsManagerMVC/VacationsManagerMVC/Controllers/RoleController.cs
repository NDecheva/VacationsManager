using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;

namespace VacationsManagerMVC.Controllers
{

    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO")]
    public class RoleController : BaseCrudController<RoleDto, IRoleRepository, IRoleService, RoleEditVM, RoleDetailsVM>
    {
        protected readonly IRoleService _roleService;

        public RoleController(IMapper mapper, IRoleService roleService)
            : base(roleService, mapper)
        {
            this._roleService = roleService;
        }

        protected override async Task<RoleEditVM> PrePopulateVMAsync(RoleEditVM editVM)
        {
            editVM.RoleTypeOptions = Enum.GetValues(typeof(RoleType))
                .Cast<RoleType>()
                .Select(roleType => new SelectListItem
                {
                    Value = ((int)roleType).ToString(),
                    Text = roleType.ToString()
                })
                .ToList();

            return editVM;
        }
    }
}
