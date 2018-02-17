using Bottlecap.Components.Bots;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using CVSkill.Models;
using System.Collections.Generic;
using System;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> GetCurrentEmploymentAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetCurrentEmploymentAsync)}");
            await _service.InitialiseAsync();

            var employmentHistory = _service.GetEmploymentHistory();
            var role = employmentHistory.FirstOrDefault(x => x.End == null);

            var response = ConstructEmploymentResponse(bot, role);

            return new BotResponse()
            {
                Speak = response
            };
        }

        public async Task<IBotResponse> GetSpecificEmploymentHistoryAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetSpecificEmploymentHistoryAsync)}");

            string company = null;
            CVJob role = null;
            if (bot.Query.HasToken(TokenKeys.Company) &&
                String.IsNullOrEmpty(company = bot.Query.GetTokenValue(TokenKeys.Company)) == false)
            {
                var companyWithoutSpaces = company.Replace(" ", String.Empty);

                bot.Log("Initialise service...");
                await _service.InitialiseAsync();

                var employmentHistory = _service.GetEmploymentHistory();
                role = employmentHistory.FirstOrDefault(x => x.Employer.Replace(" ", String.Empty).StartsWith(companyWithoutSpaces, StringComparison.OrdinalIgnoreCase));
            }

            if (role == null)
            {
                return new BotResponse()
                {
                    Speak = String.Format(_resourceManager.GetResource(ResourceKeys.NoJobsWithEmployer),
                                          company)
                };
            }

            var response = ConstructEmploymentResponse(bot, role);

            return new BotResponse()
            {
                Speak = response
            };
        }

        public async Task<IBotResponse> GetNextEmploymentHistoryAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetNextEmploymentHistoryAsync)}");

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
            bot.Log($"Starting {nameof(GetEmploymentHistoryAsync)}");

            if (cachedJobs == null)
            {
                bot.Log("Retrieving cached jobs...");

                await _service.InitialiseAsync();

                var employmentHistory = _service.GetEmploymentHistory();
                cachedJobs = employmentHistory.Where(x => x.End != null);
            }

            var role = cachedJobs.FirstOrDefault();
            cachedJobs = cachedJobs.Skip(1);

            var response = ConstructEmploymentResponse(bot, role);

            bool isMoreEmploymentHistory = false;
            if (cachedJobs?.Any() == true)
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

        private string ConstructEmploymentResponse(IBot bot, CVJob role)
        {
            var response = new StringBuilder();
            response.AppendFormat(_resourceManager.GetResource(role.End == null
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
