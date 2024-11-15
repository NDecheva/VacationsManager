using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Teams = new List<Team>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Team> Teams { get; set; }


        public Project(string name, string description)
            : base()
        {
            Name = name;
            Description = description;
            Teams = new List<Team>();
        }
    }
}
