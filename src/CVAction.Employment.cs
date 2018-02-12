using Bottlecap.Components.Bots;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using CVSkill.Models;

namespace CVSkill
{
    public partial class CVAction
    {
        public async Task<IBotResponse> GetCurrentEmploymentAsync(IBot bot)
        {
            bot.Log("Initialise service...");
            await _service.InitialiseAsync();

            var employmentHistory = _service.GetEmploymentHistory();
            var role = employmentHistory.FirstOrDefault(x => x.End == null);

            var response = ConstructEmploymentResponse(bot, role, true);

            return new BotResponse()
            {
                Speak = response
            };
        }

        private string ConstructEmploymentResponse(IBot bot, CVJob role, bool isCurrent)
        {
            var response = new StringBuilder();
            response.AppendFormat(_resourceManager.GetResource(isCurrent
                                                               ? ResourceKeys.CurrentEmploymentStart
                                                               : ResourceKeys.PreviousEmploymentStart),
                                  role.Start,
                                  role.End,
                                  role.Employer,
                                  role.Role);

            response.Append(" ");

            if (role.Duties != null)
            {
                foreach (var duty in role.Duties)
                {
                    response.Append($"{duty.Duty}. ");
                }
            }

            return response.ToString();
        }
    }
}
