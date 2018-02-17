using Bottlecap.Components.Bots;
using System.Threading.Tasks;

namespace CVSkill
{
    public partial class CVAction
    {
        public Task<IBotResponse> ContactAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(ContactAsync)}");

            return Task.FromResult<IBotResponse>(new BotResponse()
            {
                Speak = _resourceManager.GetResource(ResourceKeys.Contact)
            });
        }
    }
}
