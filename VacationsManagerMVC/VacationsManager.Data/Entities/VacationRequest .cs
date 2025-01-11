using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Enums;

namespace VacationsManager.Data.Entities
{
    public class VacationRequest : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsApproved { get; set; } = false;
        public int RequesterId { get; set; }
        public virtual User Requester { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public VacationType VacationType { get; set; }

    }
}
