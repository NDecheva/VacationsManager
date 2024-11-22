using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationsManager.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            VacationRequests = new List<VacationRequest>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<VacationRequest> VacationRequests { get; set; }

    }
}

