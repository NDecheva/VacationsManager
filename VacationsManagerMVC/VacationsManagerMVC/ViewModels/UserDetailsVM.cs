using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class UserDetailsVM : BaseVM
    {
        public string Username { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Role")]
        public RoleDetailsVM Role { get; set; }

        [DisplayName("Team")]
        public TeamDetailsVM Team { get; set; }

        [DisplayName("Vacation Requests")]
        public virtual List<VacationRequestDetailsVM> VacationRequests { get; set; }

        public UserDetailsVM()
        {
            VacationRequests = new List<VacationRequestDetailsVM>();
        }
    }
}
