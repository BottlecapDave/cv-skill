﻿using CVSkill.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CVSkill.Services
{
    public interface ICVService
    {
        Task InitialiseAsync();

        IReadOnlyList<CVJob> GetJobs(string keyword);
        bool IsSkillPresent(string keyword);
        string GetProfile();
        string GetSkillName(string keyword);
        CVJob GetEmploymentHistory(string company);
        IEnumerable<CVEducation> GetEducation();
        IEnumerable<string> GetAccomplishments();
        IEnumerable<CVJob> GetEmploymentHistory();
        IEnumerable<string> GetInterests();
    }
}