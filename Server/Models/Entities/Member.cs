using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Sport> Sports { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
    }
}
