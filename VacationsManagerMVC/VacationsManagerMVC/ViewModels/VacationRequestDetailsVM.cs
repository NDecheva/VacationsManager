using System.ComponentModel;
using VacationsManager.Shared.Enums;

namespace VacationsManagerMVC.ViewModels
{
    public class VacationRequestDetailsVM : BaseVM
    {
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Half Day Request")]
        public bool IsHalfDay { get; set; }

        [DisplayName("Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Requester")]
        public UserDetailsVM Requester { get; set; }

        [DisplayName("Attachment")]
        public string? Attachment { get; set; }

        [DisplayName("Vacation Type")]
        public VacationType VacationType { get; set; }
    }
}
