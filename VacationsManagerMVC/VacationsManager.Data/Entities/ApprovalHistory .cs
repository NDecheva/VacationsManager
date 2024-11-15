using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class ApprovalHistory : BaseEntity
    {
        public ApprovalHistory()
        {
            ApprovalDate = DateTime.UtcNow;
            IsApproved = false;
            Comments = string.Empty;
        }

        public VacationRequest VacationRequest { get; set; }
        public User ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }

        public ApprovalHistory(VacationRequest vacationRequest, User approvedBy, bool isApproved, string comments = "")
            : base()
        {
            VacationRequest = vacationRequest;
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.UtcNow;
            IsApproved = isApproved;
            Comments = comments ?? string.Empty;
        }
    }
}
