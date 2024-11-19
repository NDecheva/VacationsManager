using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Shared.Enums;

namespace VacationsManager.Data.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            Users = new List<User>();
        }

        public string Name { get; set; } 
        public virtual ICollection<User> Users { get; set; }
        public RoleType RoleType { get; set; }


        public Role(string name)
            : base()
        {
            Name = name;
            Users = new List<User>();
        }
    }
}
