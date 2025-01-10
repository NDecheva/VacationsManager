using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class TeamDetailsVM : BaseVM
    {
        [DisplayName("Team Name")]
        public string Name { get; set; }

        [DisplayName("Project Name")]
        public ProjectDetailsVM Project { get; set; }

        [DisplayName("Team Leader")]
        public UserDetailsVM TeamLeader { get; set; } 

        [DisplayName("Developers")]
        public virtual List<UserDetailsVM> Developers { get; set; }

        public TeamDetailsVM()
        {
            this.Developers = new List<UserDetailsVM>(); // Инициализираме списъка с потребители
        }
    }
}
