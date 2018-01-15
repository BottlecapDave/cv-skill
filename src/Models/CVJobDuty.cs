using System.Collections.Generic;

namespace CVSkill.Models
{
    public class CVJobDuty
    {
        public string Duty { get; set; }

        public IEnumerable<string> Skills { get; set; }
    }
}