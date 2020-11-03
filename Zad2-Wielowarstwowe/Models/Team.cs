using System;
using System.Collections.Generic;
using System.Text;

namespace Zad2_Wielowarstwowe.Models
{
    class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection <TeamMember> TeamMembers { get; set; }
    }
}
