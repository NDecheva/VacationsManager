using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class Team : BaseEntity
    {
        public Team()
        {
            Developers = new List<User>();
        }

        public string Name { get; set; }
        public virtual Project Project { get; set; }
        public virtual User TeamLeader { get; set; }
        public virtual ICollection<User> Developers { get; set; }


        public Team(string name, Project project, User teamLeader)
            : base()
        {
            Name = name;
            Project = project;
            TeamLeader = teamLeader;
            Developers = new List<User>();
        }
    }
}
