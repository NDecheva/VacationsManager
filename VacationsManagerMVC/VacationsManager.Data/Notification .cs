using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data
{
    public class Notification : BaseEntity
    {

        public Notification()
        {
            DateSent = DateTime.UtcNow; 
            IsRead = false; 
            Message = string.Empty; 
        }

        public User Recipient { get; set; }
        public string Message { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsRead { get; set; }

        public Notification(User recipient, string message)
            : base()
        {
            Recipient = recipient;
            Message = message ?? string.Empty; 
            DateSent = DateTime.UtcNow;
            IsRead = false; 
        }

    }
}
