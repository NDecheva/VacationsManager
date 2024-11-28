using System.ComponentModel;
using VacationsManager.Shared.Enums;

namespace VacationsManagerMVC.ViewModels
{
    public class RoleDetailsVM : BaseVM
    {
        [DisplayName("Role Name")]
        public string Name { get; set; }

        [DisplayName("Role Type")]
        public string RoleType { get; set; } 

        public IEnumerable<UserDetailsVM> Users { get; set; }
    }
}
