using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using System;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
        private async Task<IBotResponse> GetAccomplishmentsAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetAccomplishmentsAsync)}");
            await _service.InitialiseAsync();

            var accomplishments = _service.GetAccomplishments();

            return new BotResponse()
            {
                Speak = ToString(accomplishments)
            };
        }
    }
}
