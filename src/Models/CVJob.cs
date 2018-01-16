using System;
using System.Collections.Generic;

namespace CVSkill.Models
{
    public class CVJob
    {
        public string Role { get; set; }

        public string Employer { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public IEnumerable<CVJobDuty> Duties { get; set; }

        public CVJob Clone()
        {
            return new CVJob()
            {
                Role = Role,
                Employer = Employer,
                Start = Start,
                End = End,
                Duties = Duties
            };
        }
    }
}