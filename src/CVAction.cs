using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using Bottlecap.Resources;
using System.Reflection;
using CVSkill.Services;
using System;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
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
                    //case IntentKeys.EmploymentHistory:
                    //    return await GetCurrentEmploymentAsync(bot);
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
    }
}
