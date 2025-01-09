using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VacationsManagerMVC.ViewModels
{
    public class UserEditVM : BaseVM
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "Role is required.")]
        public int RoleId { get; set; } 
        public IEnumerable<SelectListItem> AllRoles { get; set; }

        [DisplayName("Team")]
        public int? TeamId { get; set; } 
        public IEnumerable<SelectListItem> AllTeams { get; set; }
    }
}
