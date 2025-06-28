using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
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

        public IEnumerable<SelectListItem> RoleTypeOptions { get; set; }

        public RoleEditVM()
        {
            RoleTypeOptions = new List<SelectListItem>();
        }

    }
}
