using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class TeamEditVM : BaseVM
    {

        [DisplayName("Team Name")]
        [Required(ErrorMessage = "Team name is required.")]
        [StringLength(100, ErrorMessage = "Team name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Project is required.")]
        public ProjectDetailsVM ProjectName { get; set; }

        [DisplayName("Team Leader")]
        [Required(ErrorMessage = "Team leader is required.")]
        public string TeamLeader { get; set; }

        [DisplayName("Developers")]
        public List<UserDetailsVM> SelectedDevelopers { get; set; }

        public IEnumerable<UserDetailsVM> AllDevelopers { get; set; }

        public IEnumerable<SelectListItem> Projects { get; set; }

        public TeamEditVM()
        {
            SelectedDevelopers = new List<UserDetailsVM>();
            AllDevelopers = new List<UserDetailsVM>();
            Projects = new List<SelectListItem>();
        }
    }
}
