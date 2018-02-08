using CVSkill.Models;
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
    }
}