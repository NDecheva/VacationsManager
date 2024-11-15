using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class VacationRequest : BaseEntity
    {
        public VacationRequest()
        {
            CreationDate = DateTime.UtcNow;
            IsApproved = false;
            IsSickLeave = false;
            Attachment = string.Empty;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsApproved { get; set; }
        public User Requester { get; set; }
        public bool IsSickLeave { get; set; }
        public string Attachment { get; set; }


        public VacationRequest(DateTime startDate, DateTime endDate, User requester, bool isHalfDay, bool isSickLeave, string attachment = null)
            : base()
        {
            StartDate = startDate;
            EndDate = endDate;
            CreationDate = DateTime.UtcNow;
            Requester = requester;
            IsHalfDay = isHalfDay;
            IsSickLeave = isSickLeave;
            IsApproved = false;
            Attachment = attachment ?? string.Empty;
        }
    }
}
