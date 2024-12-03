using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacationsManager.Data.Entities;

namespace VacationsManagerMVC.ViewModels
{
    public class TeamEditVM : BaseVM
    {
        [DisplayName("Team Name")]
        [Required(ErrorMessage = "Team name is required.")]
        [StringLength(100, ErrorMessage = "Team name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [DisplayName("Project")]
        [Required(ErrorMessage = "Project is required.")]
        public int ProjectId { get; set; }  

        public IEnumerable<SelectListItem> Projects { get; set; } 

        [DisplayName("Team Leader")]
        [Required(ErrorMessage = "Team leader is required.")]
        public int TeamLeaderId { get; set; } 

        public IEnumerable<SelectListItem> AllTeamLeaders { get; set; }

        [DisplayName("Developers")]
        public IEnumerable<int> SelectedDeveloperIds { get; set; }  

        public IEnumerable<SelectListItem> AllDevelopers { get; set; }  

        public TeamEditVM()
        {
            Projects = new List<SelectListItem>();
            AllTeamLeaders = new List<SelectListItem>();
            AllDevelopers = new List<SelectListItem>();
        }
    }
}
