using Server.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class SocialNetworkCSContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Club> Clubs { get; set; }
    }
}
