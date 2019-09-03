using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.Entities
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Club> Clubs { get; set; }
    }
}
