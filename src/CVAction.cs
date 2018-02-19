using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using Bottlecap.Resources;
using System.Reflection;
using CVSkill.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Bottlecap.Components.Bots.Content;

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
                IBotResponse response = null;
                switch (bot.Query?.Name)
                {
                    case IntentKeys.Experience:
                        response = await GetExperienceAsync(bot);
                        break;
                    case IntentKeys.Describe:
                        response = await DescribeAsync(bot);
                        break;
                    case IntentKeys.Contact:
                        response = await ContactAsync(bot);
                        break;
                    case IntentKeys.Interests:
                        response = await InterestsAsync(bot);
                        break;
                    case IntentKeys.EmploymentCurrent:
                        response = await GetCurrentEmploymentAsync(bot);
                        break;
                    case IntentKeys.EmploymentHistory:
                        response = await GetEmploymentHistoryAsync(bot);
                        break;
                    case IntentKeys.EmploymentSpecific:
                        response = await GetSpecificEmploymentHistoryAsync(bot);
                        break;
                    case IntentKeys.Accomplishments:
                        response = await GetAccomplishmentsAsync(bot);
                        break;
                    case IntentKeys.YesAlexa:
                    case IntentKeys.NextAlexa:
                        response = await GetNextEmploymentHistoryAsync(bot);
                        break;
                    case IntentKeys.Education:
                        response = await GetEducationAsync(bot);
                        break;
                    case IntentKeys.StopAlexa:
                    case IntentKeys.CancelAlexa:
                    case IntentKeys.NoAlexa:
                        return new BotResponse();
                }

                if (response != null)
                {
                    response.Content = ContentBuilderManager.Create(ContentBuilderKeys.Default, null);

                    response.Speak = UpdateSsmlHints(response.Speak);
                    response.IsSSML = true;

                    return response;
                }
            }

            return new BotResponse()
            {
                Speak = _resourceManager.GetResource(ResourceKeys.Welcome),
                ExpectedUserResponse = UserResponse.Required
            };
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

        /// <summary>
        /// Strip away certain markdown languages as this can trip up voice assistants when reading the content.
        /// </summary>
        /// <param name="content">The content to strip the markdown from.</param>
        /// <returns>The </returns>
        private string StripMarkdown(string content)
        {
            var matches = _MarkdownRegex.Matches(content);
            foreach (Match match in matches)
            {
                content = content.Replace(match.Value, match.Groups[MarkdownRegexName].Value);
            }

            return content;
        }

        /// <remarks>
        /// Because voice assistants have difficulty pronouncing some things in technical CVs such as company names and technologies,
        /// we need to find references to these and provide SSML hints. It's a little bit of a hack, but this data shouldn't be in the original
        /// source as it describes the presentation of the data.
        /// </remarks>
        private string UpdateSsmlHints(string response)
        {
            response = response.Replace("BSc", "<sub alias=\"Bachelor of Science\">BSc</sub>");
            response = response.Replace("Moq", "<sub alias=\"Mock\">Moq</sub>");
            response = response.Replace("NUnit", "<sub alias=\"N Unit\">NUnit</sub>");
            response = response.Replace("XCode", "<sub alias=\"X Code\">XCode</sub>");
            response = response.Replace("REPL", "<say-as interpret-as=\"spell-out\">REPL</say-as>");
            response = response.Replace("CI", "<say-as interpret-as=\"spell-out\">CI</say-as>");
            response = response.Replace("AdDuplex", "<sub alias=\"Ad Duplex\">AdDuplex</sub>");

            return response;
        }
    }
}
