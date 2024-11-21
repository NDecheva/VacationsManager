using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Shared.Dtos
{
    public class NotificationDto : BaseModel
    {
        public int RecipientId { get; set; }
        public UserDto Recipient { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
    }
}
