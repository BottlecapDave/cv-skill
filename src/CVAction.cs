using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using Bottlecap.Resources;
using System.Reflection;
using CVSkill.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
        private const string MarkdownRegexName = "name";
        private static readonly Regex _MarkdownRegex = new Regex("\\[(?<name>[^\\]]+)\\]\\([^\\)]+\\)");
        private static bool _IsResourcesLoaded;

        private readonly IResourceManager _resourceManager;

        private readonly ICVService _service;

        public CVAction(IResourceManager resourceManager, ICVService service)
        {
            _resourceManager = resourceManager;
            _service = service;

            LoadResources();
        }

        public async Task<IBotResponse> ProcessAsync(IBot bot)
        {
            bot.Log("Intent Name: {0}", bot.Query?.Name);

            if (String.IsNullOrEmpty(bot.Query?.Name) == false)
            {
                switch (bot.Query?.Name)
                {
                    case IntentKeys.Experience:
                        return await GetExperienceAsync(bot);
                    case IntentKeys.Describe:
                        return await DescribeAsync(bot);
                    case IntentKeys.Contact:
                        return await ContactAsync(bot);
                    case IntentKeys.Interests:
                        return await InterestsAsync(bot);
                    case IntentKeys.EmploymentCurrent:
                        return await GetCurrentEmploymentAsync(bot);
                    case IntentKeys.EmploymentHistory:
                        return await GetEmploymentHistoryAsync(bot);
                    case IntentKeys.EmploymentSpecific:
                        return await GetSpecificEmploymentHistoryAsync(bot);
                    case IntentKeys.Accomplishments:
                        return await GetAccomplishmentsAsync(bot);
                    case IntentKeys.YesAlexa:
                    case IntentKeys.NextAlexa:
                        return await GetNextEmploymentHistoryAsync(bot);
                    case IntentKeys.Education:
                        return await GetEducationAsync(bot);
                    case IntentKeys.StopAlexa:
                    case IntentKeys.CancelAlexa:
                    case IntentKeys.NoAlexa:
                        return new BotResponse();
                }
            }

            return new BotResponse();
        }

        private void LoadResources()
        {
            if (_IsResourcesLoaded == false)
            {
                var assembly = Assembly.Load(new AssemblyName("CVSkill"));
                if (assembly != null)
                {
                    using (var stream = assembly.GetManifestResourceStream("CVSkill.Resources.json"))
                    {
                        if (stream != null)
                        {
                            _resourceManager.Initialise(stream);
                            _IsResourcesLoaded = true;
                        }
                    }
                }
            }
        }

        private string ToString(IEnumerable<string> items)
        {
            var stringBuilder = new StringBuilder();
            foreach (var interest in items)
            {
                var updatedInterest = StripMarkdown(interest);
                stringBuilder.Append($" {updatedInterest}");
            }

            return stringBuilder.ToString();
        }

        private string StripMarkdown(string interest)
        {
            var matches = _MarkdownRegex.Matches(interest);
            foreach (Match match in matches)
            {
                interest = interest.Replace(match.Value, match.Groups[MarkdownRegexName].Value);
            }

            return interest;
        }
    }
}
