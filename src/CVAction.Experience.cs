using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using System;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
        private async Task<IBotResponse> GetExperienceAsync(IBot bot)
        {
            string keyword = null;
            if (bot.Query.HasToken(ParameterKeys.ExperienceType))
            {
                keyword = bot.Query.GetTokenValue(ParameterKeys.ExperienceType);

                bot.Log("ExperienceType: {0}", keyword);

                if (String.IsNullOrEmpty(keyword))
                {
                    return new BotResponse()
                    {
                        Speak = _resourceManager.GetResource(ResourceKeys.KeywordMissing)
                    };
                }

                bot.Log("Initialise service...");
                await _service.InitialiseAsync();

                bot.Log("Get Jobs...");
                var jobs = _service.GetJobs(keyword);

                if (jobs.Count < 1)
                {
                    return new BotResponse()
                    {
                        Speak = String.Format(_resourceManager.GetResource(ResourceKeys.NoJobsWithSkill),
                                              keyword)
                    };
                }
                else
                {
                    return new BotResponse()
                    {
                        Speak = String.Format("Jobs: {0}",
                                              jobs.Count)
                    };
                }
            }

            bot.Log("No Experience");

            return new BotResponse();
        }
    }
}
