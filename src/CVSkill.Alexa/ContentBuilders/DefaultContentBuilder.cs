using Bottlecap.Components.Bots.Content;

namespace CVSkill.Alexa.ContentBuilders
{
    public class DefaultContentBuilder : IContentBuilder
    {
        public object Create(object input)
        {
            return "Sometimes you might get weird responses when asking questions. This card appears so you can check what Alexa heard. If you still got a weird response, please contact me and let me know at me@davidskendall.co.uk.";
        }
    }
}
