using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data
{
    public class AuditLog : BaseEntity
    {
        public AuditLog()
        {
            ActionDate = DateTime.UtcNow;
            Details = string.Empty; 
        }

        public string Action { get; set; } //"Create User" или "Approve Vacation"
        public User PerformedBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string Details { get; set; }

       
        public AuditLog(string action, User performedBy, string details = "")
            : base()
        {
            Action = action;
            PerformedBy = performedBy;
            ActionDate = DateTime.UtcNow;
            Details = details ?? string.Empty; 
    }
}
