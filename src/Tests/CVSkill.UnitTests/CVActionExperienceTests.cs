using Bottlecap;
using Bottlecap.Components.Bots;
using Bottlecap.Dispatching;
using Bottlecap.Resources;
using CVSkill.Services;
using Moq;
using NUnit.Framework;
using System;

namespace CVSkill.UnitTests
{
    [TestFixture]
    public class CVActionExperienceTests
    {
        [SetUp]
        public void Setup()
        {
            DependencyService.Current.Register<IDispatcher, BaseDispatcher>(new BaseDispatcher());
            DependencyService.Current.Register<ICVService, CVService>(new CVService());

            Bottlecap.Json.JsonFactory.Current = new Bottlecap.Json.Newtonsoft.JsonFactory();
        }

        [Test]
        public void Process_Experience_When_ExperienceDoesNotMatchSkill_Then_NoJobFound()
        {
            // Arrange
            var wrapper = new ActionWrapper();
            wrapper.MockedBotQuery.SetupGet(x => x.Name).Returns(IntentKeys.Experience);
            wrapper.MockedBotQuery.Setup(x => x.HasToken(ParameterKeys.ExperienceType)).Returns(true);
            wrapper.MockedBotQuery.Setup(x => x.GetTokenValue(ParameterKeys.ExperienceType)).Returns("react");

            // Act
            var result = wrapper.Action.ProcessAsync(wrapper.MockedBot.Object).Result;

            // Assert
            Assert.IsTrue(result.Speak.Contains(ResourceKeys.NoExperienceWithSkill),
                          "Expected no experience with skill to be found. Response was {0}",
                          result.Speak);
        }

        [Test]
        public void Process_Experience_When_ExperienceFound_NoJobFound_Then_ExperienceFound()
        {
            // Arrange
            var wrapper = new ActionWrapper();
            wrapper.MockedBotQuery.SetupGet(x => x.Name).Returns(IntentKeys.Experience);
            wrapper.MockedBotQuery.Setup(x => x.HasToken(ParameterKeys.ExperienceType)).Returns(true);
            wrapper.MockedBotQuery.Setup(x => x.GetTokenValue(ParameterKeys.ExperienceType)).Returns("solid");

            // Act
            var result = wrapper.Action.ProcessAsync(wrapper.MockedBot.Object).Result;

            // Assert
            Assert.IsTrue(result.Speak.Contains(ResourceKeys.ExperienceNoDutiesFoundStart), 
                          "Expected no jobs with skill to be found. Response was {0}",
                          result.Speak);
        }

        [Test]
        public void Process_Experiences_When_ExperienceMatchesSkill_Then_JobFound()
        {
            // Arrange
            var wrapper = new ActionWrapper();
            wrapper.MockedBotQuery.SetupGet(x => x.Name).Returns(IntentKeys.Experience);
            wrapper.MockedBotQuery.Setup(x => x.HasToken(ParameterKeys.ExperienceType)).Returns(true);
            wrapper.MockedBotQuery.Setup(x => x.GetTokenValue(ParameterKeys.ExperienceType)).Returns("xamarin");

            // Act
            var result = wrapper.Action.ProcessAsync(wrapper.MockedBot.Object).Result;

            // Assert
            Assert.IsTrue(result.Speak.Contains(ResourceKeys.ExperienceFoundStart),
                          "Expected experience found. Response was {0}",
                          result.Speak);
        }

        [Test]
        public void Process_Experience_When_ExperienceMatchesSkillKeyword_Then_JobFound()
        {
            // Arrange
            var wrapper = new ActionWrapper();
            wrapper.MockedBotQuery.SetupGet(x => x.Name).Returns(IntentKeys.Experience);
            wrapper.MockedBotQuery.Setup(x => x.HasToken(ParameterKeys.ExperienceType)).Returns(true);
            wrapper.MockedBotQuery.Setup(x => x.GetTokenValue(ParameterKeys.ExperienceType)).Returns("c sharp");

            // Act
            var result = wrapper.Action.ProcessAsync(wrapper.MockedBot.Object).Result;

            // Assert
            Assert.IsTrue(result.Speak.Contains(ResourceKeys.ExperienceFoundStart),
                          "Expected experience found. Response was {0}",
                          result.Speak);
        }

        public class ActionWrapper
        {
            public Mock<IBot> MockedBot { get; private set; }

            public Mock<IBotQuery> MockedBotQuery { get; private set; }

            public Mock<IResourceManager> MockedResourceManager { get; private set; }

            public CVService CVService { get; private set; }

            public CVAction Action { get; private set; }

            public ActionWrapper()
            {
                MockedBot = new Mock<IBot>();
                MockedBotQuery = new Mock<IBotQuery>();

                MockedResourceManager = new Mock<IResourceManager>();
                CVService = new CVService();
                Action = new CVAction(MockedResourceManager.Object, CVService);

                MockedBot.SetupGet(x => x.Query).Returns(MockedBotQuery.Object);
                MockedResourceManager.Setup(x => x.GetResource(It.IsAny<string>())).Returns<string>(x => x);
            }
        }
    }
}
