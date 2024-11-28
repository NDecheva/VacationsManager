using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Enums;

namespace VacationsManagerMVC.ViewModels
{
    public class RoleEditVM : BaseVM
    {
        [DisplayName("Role Name")]
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string Name { get; set; }

        [DisplayName("Role Type")]
        [Required(ErrorMessage = "Role type is required.")]
        public RoleType RoleType { get; set; }

        [DisplayName("Assigned Users")]
        public IEnumerable<int> SelectedUserIds { get; set; } = new List<int>();

        public IEnumerable<UserDetailsVM> AllUsers { get; set; }

        public IEnumerable<SelectListItem> RoleTypes { get; set; }
    }
}
