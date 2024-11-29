using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class TeamDetailsVM : BaseVM
    {
        public int TeamId { get; set; }

        [DisplayName("Team Name")]
        public string Name { get; set; }

        [DisplayName("Project Name")]
        public ProjectDetailsVM ProjectName { get; set; }

        [DisplayName("Team Leader")]
        public string TeamLeader { get; set; }

        [DisplayName("Developers")]
        public virtual List<string> Developers { get; set; } 

        public TeamDetailsVM()
        {
            this.Developers = new List<string>();
        }
    }
}
