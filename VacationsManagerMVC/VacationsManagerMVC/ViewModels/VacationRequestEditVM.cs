using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using VacationsManager.Shared.Enums;

namespace VacationsManagerMVC.ViewModels
{
    public class VacationRequestEditVM : BaseVM
    {
        [DisplayName("Start Date")]
        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [DisplayName("Half Day Request")]
        public bool IsHalfDay { get; set; }

        [DisplayName("Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Requester")]
        public UserDetailsVM Requester { get; set; }
        public IEnumerable<SelectListItem> Requesters { get; set; }

        [DisplayName("Attachment")]
        public string Attachment { get; set; }

        [DisplayName("Vacation Type")]
        [Required(ErrorMessage = "Vacation type is required.")]
        public VacationType SelectedVacationType { get; set; }
        public IEnumerable<SelectListItem> VacationTypes { get; set; }

        public VacationRequestEditVM()
        {
            Requesters = new List<SelectListItem>();
            VacationTypes = new List<SelectListItem>();
        }
    }
}
