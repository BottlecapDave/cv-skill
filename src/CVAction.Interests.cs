using Bottlecap.Components.Bots;
using System.Threading.Tasks;
using System.Text;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> InterestsAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(InterestsAsync)}");
            await _service.InitialiseAsync();

            var interests = _service.GetInterests();

            return new BotResponse()
            {
                Speak = ToString(interests)
            };
        }
    }
}
