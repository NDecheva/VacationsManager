using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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

        [DisplayName("Role")]
        public RoleDetailsVM Role { get; set; }

        public IEnumerable<SelectListItem> AllRoles { get; set; }

        [DisplayName("Team")]
        public TeamDetailsVM Team { get; set; }

        public IEnumerable<SelectListItem> AllTeams { get; set; }

        [DisplayName("Vacation Requests")]
        public virtual List<VacationRequestDetailsVM> VacationRequests { get; set; }

        public UserEditVM()
        {
            AllRoles = new List<SelectListItem>();
            AllTeams = new List<SelectListItem>();
            VacationRequests = new List<VacationRequestDetailsVM>();
        }
    }
}
