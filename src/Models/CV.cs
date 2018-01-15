using System;
using System.Collections.Generic;
using System.Text;

namespace CVSkill.Models
{
    public class CV
    {
        public IEnumerable<CVSkill> Skills { get; set; }

        public IEnumerable<CVJob> Employment { get; set; }
    }
}
