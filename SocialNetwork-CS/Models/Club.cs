using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork_CS.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
