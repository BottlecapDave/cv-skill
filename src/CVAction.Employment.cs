using Bottlecap.Components.Bots;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using CVSkill.Models;
using System.Collections.Generic;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> GetCurrentEmploymentAsync(IBot bot)
        {
            bot.Log("Initialise service...");
            await _service.InitialiseAsync();

            var employmentHistory = _service.GetEmploymentHistory();
            var role = employmentHistory.FirstOrDefault(x => x.End == null);

            var response = ConstructEmploymentResponse(bot, role, true);

            return new BotResponse()
            {
                Speak = response
            };
        }

        public async Task<IBotResponse> GetNextEmploymentHistoryAsync(IBot bot)
        {
            var cachedJobs = await bot.GetContextAsync<IEnumerable<CVJob>>(ContextKeys.EmploymentHistory);
            if (cachedJobs != null)
            {
                return await GetEmploymentHistoryAsync(bot, cachedJobs);
            }

            return new BotResponse()
            {
                Speak = _resourceManager.GetResource(ResourceKeys.NoFurtherEmploymentHistory)
            };
        }

        public async Task<IBotResponse> GetEmploymentHistoryAsync(IBot bot, IEnumerable<CVJob> cachedJobs = null)
        {
            if (cachedJobs == null)
            {
                bot.Log("Initialise service...");
                await _service.InitialiseAsync();

                var employmentHistory = _service.GetEmploymentHistory();
                cachedJobs = employmentHistory.Where(x => x.End != null);
            }

            var role = cachedJobs.FirstOrDefault();
            cachedJobs = cachedJobs.Skip(1);

            var response = ConstructEmploymentResponse(bot, role, true);

            bool isMoreEmploymentHistory = false;
            if (cachedJobs.Any())
            {
                isMoreEmploymentHistory = true;
                response = $"{response}. {_resourceManager.GetResource(ResourceKeys.PreviousEmploymentContinue)}";
                await bot.AddContextAsync(ContextKeys.EmploymentHistory, cachedJobs);
            }

            return new BotResponse()
            {
                Speak = response,
                ExpectedUserResponse = isMoreEmploymentHistory ? UserResponse.Required : UserResponse.None
            };
        }

        private string ConstructEmploymentResponse(IBot bot, CVJob role, bool isCurrent)
        {
            var response = new StringBuilder();
            response.AppendFormat(_resourceManager.GetResource(isCurrent
                                                               ? ResourceKeys.CurrentEmploymentStart
                                                               : ResourceKeys.PreviousEmploymentStart),
                                  role.Start,
                                  role.End,
                                  role.Employer,
                                  role.Role);

            response.Append(" ");

            if (role.Duties != null)
            {
                foreach (var duty in role.Duties)
                {
                    response.Append($"{duty.Duty}. ");
                }
            }

            return response.ToString();
        }
    }
}
