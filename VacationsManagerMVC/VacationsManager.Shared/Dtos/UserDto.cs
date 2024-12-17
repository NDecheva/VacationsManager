using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Shared.Dtos
{
    public class UserDto : BaseModel
    {
        public UserDto()
        {
            VacationRequests = new List<VacationRequestDto>();
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId {  get; set; }
        public int TeamId {  get; set; }
        public RoleDto Role { get; set; }
        public TeamDto Team { get; set; }
        public List<VacationRequestDto> VacationRequests { get; set; }
    }
}
