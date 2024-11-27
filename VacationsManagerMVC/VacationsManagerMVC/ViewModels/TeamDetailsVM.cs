using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class TeamDetailsVM
    {
        public int TeamId { get; set; }

        [DisplayName("Team Name")]
        public string Name { get; set; }

        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

        [DisplayName("Team Leader")]
        public string TeamLeader { get; set; }

        [DisplayName("Developers")]
        public List<string> Developers { get; set; } 

        public TeamDetailsVM()
        {
            Developers = new List<string>();
        }
    }
}
