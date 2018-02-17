using Bottlecap.Web;
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

            var skills = GetSkills(_cv.Skills, keyword);

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
                    else if (employer.Skills?.Any(x => String.Equals(x, skill.Id, StringComparison.OrdinalIgnoreCase)) == true)
                    {
                        var job = employer.Clone();
                        job.Duties = null;
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
            return GetSkills(_cv.Skills, keyword).Any();
        }

        public string GetSkillName(string keyword)
        {
            var skill = GetSkills(_cv.Skills, keyword);

            return skill.FirstOrDefault()?.Skill;
        }

        private IEnumerable<Models.CVSkill> GetSkills(IEnumerable<Models.CVSkill> skills, string keyword)
        {
            var keywordWithNoSpaces = keyword.Replace(" ", String.Empty);

            var skillsToReturn = skills?.Where(x => x.Keywords?.Any(skillkeyword => String.Equals(skillkeyword, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)) == true);
            skillsToReturn = skillsToReturn.Union(skills?.Where(x => String.Equals(x.Skill, keywordWithNoSpaces, StringComparison.OrdinalIgnoreCase)));

            return skillsToReturn;
        }

        public IEnumerable<string> GetInterests()
        {
            return _cv.Interests;
        }

        public IEnumerable<CVJob> GetEmploymentHistory()
        {
            return _cv.Employment;
        }

        public CVJob GetEmploymentHistory(string company)
        {
            var companyWithoutSpaces = company.Replace(" ", String.Empty);

            return _cv.Employment.FirstOrDefault(x => String.Equals(x.Employer.Replace(" ", String.Empty), companyWithoutSpaces, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> GetAccomplishments()
        {
            return _cv.PersonalAccomplishments.Select(x => x.Accomplishment);
        }

        public IEnumerable<CVEducation> GetEducation()
        {
            return _cv.Education;
        }
    }
}
