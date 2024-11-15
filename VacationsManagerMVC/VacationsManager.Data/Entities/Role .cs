using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            Users = new List<User>();
        }

        public string Name { get; set; } // CEO, Developer, Team Lead, Unassigned
        public virtual ICollection<User> Users { get; set; }


        public Role(string name)
            : base()
        {
            Name = name;
            Users = new List<User>();
        }
    }
}
