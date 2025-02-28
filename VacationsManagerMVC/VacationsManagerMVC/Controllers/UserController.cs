﻿using AutoMapper;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManagerMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using VacationsManager.Shared.Security;

namespace VacationsManagerMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "CEO")]
    public class UserController : BaseCrudController<UserDto, IUserRepository, IUserService, UserEditVM, UserDetailsVM>
    {
        protected readonly IRoleService _roleService;
        protected readonly ITeamService _teamService;

        public UserController(IUserService userService, IMapper mapper, IRoleService roleService, ITeamService teamService)
            : base(userService, mapper)
        {
            this._roleService = roleService;
            this._teamService = teamService;
        }

        protected override async Task<UserEditVM> PrePopulateVMAsync(UserEditVM editVM)
        {

            editVM.AllRoles = (await _roleService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));

            editVM.AllTeams = (await _teamService.GetAllAsync())
                .Select(x => new SelectListItem($"{x.Name}", x.Id.ToString()));

            return editVM;
        }

        [HttpPost]
        public override async Task<IActionResult> Create(UserEditVM editVM)
        {
            if (!string.IsNullOrEmpty(editVM.Password))
            {
                editVM.Password = PasswordHasher.HashPassword(editVM.Password);
            }

            return await base.Create(editVM);
        }

    }
}
