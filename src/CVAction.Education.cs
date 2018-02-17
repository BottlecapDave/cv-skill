using System.Threading.Tasks;
using Bottlecap.Components.Bots;
using System;
using System.Text;

namespace CVSkill
{
    public partial class CVAction : IBotAction
    {
        private async Task<IBotResponse> GetEducationAsync(IBot bot)
        {
            bot.Log($"Starting {nameof(GetEducationAsync)}");

            await _service.InitialiseAsync();

            var education = _service.GetEducation();

            var response = new StringBuilder();

            foreach (var item in education)
            {
                response.AppendFormat($"{_resourceManager.GetResource(ResourceKeys.Education)} ",
                                      item.Establishment,
                                      item.Start,
                                      item.End,
                                      item.Course);
            }

            return new BotResponse()
            {
                Speak = response.ToString()
            };
        }
    }
}
