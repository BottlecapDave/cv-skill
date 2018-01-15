using System.Collections.Generic;

namespace CVSkill.Models
{
    public class CVJob
    {
        public string Role { get; set; }

        public string Employer { get; set; }

        public IEnumerable<CVJobDuty> Duties { get; set; }
    }
}