
using Amazon.Lambda.Core;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Bottlecap;
using Bottlecap.Dispatching;
using Bottlecap.Resources;
using Bottlecap.Components.Bots.Alexa;
using CVSkill.Services;
using Bottlecap.Components.Bots.Content;
using CVSkill.Alexa.ContentBuilders;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CVSkill.Alexa
{
    public class Function
    {

        static Function()
        {
            DependencyService.Current.Register<IDispatcher, BaseDispatcher>(new BaseDispatcher());
            DependencyService.Current.Register<IResourceManager, ResourceManager>(new ResourceManager());
            DependencyService.Current.Register<ICVService, CVService>(new CVService());

            Bottlecap.Json.JsonFactory.Current = new Bottlecap.Json.Newtonsoft.JsonFactory();

            ContentBuilderManager.Register(ContentBuilderKeys.Default, new DefaultContentBuilder());
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var alexa = new AlexaBot(input, context, addressOnlySupportsPostalCode: false);

            var action = new CVAction(DependencyService.Current.Get<IResourceManager>(), DependencyService.Current.Get<ICVService>());

            return alexa.ToSkillResponse(action.ProcessAsync(alexa).Result);
        }
    }
}
