using Bottlecap.Components.Bots;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CVSkill
{
    public partial class CVAction
    {
        public Task<IBotResponse> ContactAsync(IBot bot)
        {
            return Task.FromResult<IBotResponse>(new BotResponse()
            {
                Speak = _resourceManager.GetResource(ResourceKeys.Contact)
            });
        }
    }
}
