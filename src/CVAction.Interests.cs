using Bottlecap.Components.Bots;
using System.Threading.Tasks;
using System.Text;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> InterestsAsync(IBot bot)
        {
            bot.Log("Initialise service...");
            await _service.InitialiseAsync();

            var interests = _service.GetInterests();

            var interestsBuilder = new StringBuilder();
            foreach (var interest in interests)
            {
                interestsBuilder.Append($" {interest}");
            }

            return new BotResponse()
            {
                Speak = interestsBuilder.ToString()
            };
        }
    }
}
