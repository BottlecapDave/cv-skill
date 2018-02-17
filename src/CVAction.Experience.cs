using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using System;
using System.Text;
using System.Linq;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
        private async Task<IBotResponse> GetExperienceAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetExperienceAsync)}");

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
                    if (_service.IsSkillPresent(keyword))
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
                            Speak = String.Format(_resourceManager.GetResource(ResourceKeys.NoExperienceWithSkill),
                                                  keyword)
                        };
                    }
                }
                else
                {
                    bot.Log("{0} Jobs found", jobs.Count);

                    var responseBuilder = new StringBuilder();

                    if (jobs.Any(x => x.Duties?.Any() == true))
                    {
                        responseBuilder.AppendFormat(_resourceManager.GetResource(ResourceKeys.ExperienceFoundStart),
                                                     keyword);

                        for (int i = 0; i < jobs.Count; i++)
                        {
                            var job = jobs[i];

                            if (job.Duties != null)
                            {
                                responseBuilder.Append(" ");
                                responseBuilder.AppendFormat(_resourceManager.GetResource(job.End == null
                                                                                          ? ResourceKeys.CurrentJobExperienceStart
                                                                                          : ResourceKeys.PreviousJobExperienceStart),
                                                             job.Employer);
                                responseBuilder.Append(" ");


                                foreach (var duty in job.Duties)
                                {
                                    responseBuilder.AppendFormat("{0} ", duty.Duty);
                                }
                            }
                        }
                    }
                    else
                    {
                        responseBuilder.AppendFormat(_resourceManager.GetResource(ResourceKeys.ExperienceNoDutiesFoundStart),
                                                     keyword);
                        responseBuilder.Append(" ");

                        for (int i = 0; i < jobs.Count; i++)
                        {
                            if (i < (jobs.Count - 1))
                            {
                                responseBuilder.AppendFormat("{0}, ", jobs[i].Employer);
                            }
                            else
                            {
                                responseBuilder.AppendFormat("and {0}.", jobs[i].Employer);
                            }
                        }
                    }

                    bot.Log("Jobs Response: {0}", responseBuilder.ToString());

                    return new BotResponse()
                    {
                        Speak = responseBuilder.ToString()
                    };
                }
            }

            bot.Log("No Experience");

            return new BotResponse();
        }
    }
}
