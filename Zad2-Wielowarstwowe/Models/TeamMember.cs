using System;
using System.Collections.Generic;
using System.Text;

namespace Zad2_Wielowarstwowe.Models
{
    public enum Title
    {
        Developer,
        ScrumMaster
    }
    class TeamMember
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Title MemberTitle { get; set; }
        public int TeamId { get; set; }
        // for n+1 example purpose
        public Team Team { get; set; }
    }
}

