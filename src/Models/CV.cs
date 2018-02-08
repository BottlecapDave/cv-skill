using System.Collections.Generic;

namespace CVSkill.Models
{
    public class CV
    {
        public string Profile { get; set; }

        public IEnumerable<CVSkill> Skills { get; set; }

        public IEnumerable<CVJob> Employment { get; set; }
    }
}
