using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class Notification : BaseEntity
    {

        public int RecipientId { get; set; }
        public virtual User Recipient { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;

    }
}
