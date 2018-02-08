using Bottlecap.Components.Bots;
using System.Threading.Tasks;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> DescribeAsync(IBot bot)
        {
            bot.Log("Initialise service...");
            await _service.InitialiseAsync();

            var profile = _service.GetProfile();

            return new BotResponse()
            {
                Speak = profile
            };
        }
    }
}
