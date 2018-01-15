using System;
using System.Collections.Generic;
using System.Text;

namespace CVSkill.Models
{
    public class CVSkill
    {
        public string Skill { get; set; }

        public string Id { get; set; }

        public IEnumerable<string> Keywords { get; set; }
    }
}
