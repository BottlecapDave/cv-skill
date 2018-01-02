using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using Bottlecap.Resources;

namespace CVSkill
{
    public class CVAction : IBotAction
    {
        private readonly IResourceManager _resourceManager;

        public CVAction(IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public Task<IBotResponse> ProcessAsync(IBot bot)
        {
            return Task.FromResult<IBotResponse>(new BotResponse());
        }
    }
}
