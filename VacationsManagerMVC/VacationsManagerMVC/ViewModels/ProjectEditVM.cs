using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class ProjectEditVM : BaseVM
    {
        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Project name is required.")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Is Completed")]
        public bool IsCompleted { get; set; }

        // Списък с екипите в проекта
        public List<TeamDetailsVM> Teams { get; set; }
    }
}
