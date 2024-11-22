using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Shared.Dtos
{
    public class ProjectDto : BaseModel
    {
        public ProjectDto()
        {
            Teams = new List<TeamDto>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<TeamDto> Teams { get; set; }
    }
}
