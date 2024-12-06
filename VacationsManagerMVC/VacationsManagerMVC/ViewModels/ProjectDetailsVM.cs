using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class ProjectDetailsVM : BaseVM
    {
        public ProjectDetailsVM()
        {
            this.Teams = new List<TeamDetailsVM>();
        }

        [DisplayName("Project Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Is Completed")]
        public bool IsCompleted { get; set; }

        [DisplayName("Teams Involved")]
        public virtual List<TeamDetailsVM> Teams { get; set; } = new List<TeamDetailsVM>();
    }
}