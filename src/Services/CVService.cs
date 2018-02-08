﻿using Bottlecap.Web;
using CVSkill.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CVSkill.Services
{
    public class CVService : ICVService
    {
        private CV _cv;

        public IReadOnlyList<CVJob> GetJobs(string keyword)
        {
            var jobs = new List<CVJob>();
            var keywordWithNoSpaces = keyword.Replace(" ", String.Empty);

            var skills = _cv.Skills?.Where(x => x.Keywords?.Any(skillkeyword => String.Equals(skillkeyword, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)) == true);
            skills = skills.Union(_cv.Skills?.Where(x => String.Equals(x.Skill, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)));

            foreach (var skill in skills)
            {
                foreach (var employer in _cv.Employment)
                {
                    var duties = employer.Duties.Where(x => x.Skills?.Any(dutySkill => String.Equals(dutySkill, skill.Id, StringComparison.OrdinalIgnoreCase)) == true);
                    if (duties.Count() > 0)
                    {
                        var job = employer.Clone();
                        job.Duties = duties;
                        jobs.Add(job);
                    }
                }
            }

            return jobs;
        }

        public string GetProfile()
        {
            return _cv.Profile;
        }

        public async Task InitialiseAsync()
        {
            var request = new CVWebRequest();
            if (await WebRequestManager.MakeRequestAsync(request))
            {
                _cv = request.Response;
            }
        }

        public bool IsSkillPresent(string keyword)
        {
            var keywordWithNoSpaces = keyword.Replace(" ", String.Empty);

            var skills = _cv.Skills?.Where(x => x.Keywords?.Any(skillkeyword => String.Equals(skillkeyword, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)) == true);
            skills = skills.Union(_cv.Skills?.Where(x => String.Equals(x.Skill, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)));

            return skills.Any();
        }
    }
}
