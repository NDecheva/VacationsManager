using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Shared.Dtos
{
    public class TeamDto : BaseModel
    {
        public TeamDto()
        {
            Developers = new List<UserDto>();
        }

        public string Name { get; set; }
        public int? ProjectId { get; set; }
        public int TeamLeaderId { get; set; }
        public ProjectDto Project { get; set; }
        public UserDto TeamLeader { get; set; }
        public List<UserDto> Developers { get; set; }
    }
}
