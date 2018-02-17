using System.Collections.Generic;
using CVSkill.Services;

namespace CVSkill.Models
{
    public class CV
    {
        public string Profile { get; set; }

        public IEnumerable<CVSkill> Skills { get; set; }

        public IEnumerable<CVJob> Employment { get; set; }

        public IEnumerable<string> Interests { get; set; }

        public IEnumerable<CVAccomplishment> PersonalAccomplishments { get; set; }

        public IEnumerable<CVEducation> Education { get; set; }
    }
}
