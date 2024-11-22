using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Enums;

namespace VacationsManager.Shared.Dtos
{
    public class VacationRequestDto : BaseModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsApproved { get; set; }
        public UserDto Requester { get; set; }
        public string Attachment { get; set; }
        public VacationType VacationType { get; set; }
        
    }
}
