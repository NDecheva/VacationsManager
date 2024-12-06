using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class ProjectEditVM : BaseVM
    {
        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Project name is required.")]
        [StringLength(1000, ErrorMessage = "Project name cannot exceed 1000 characters.")]
        public string Name { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

    }
}
