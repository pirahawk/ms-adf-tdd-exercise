using BoDi;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class ConfigurationBuilderSetupHook
    {
        [BeforeScenario(Order = 0)]
        public void LoadConfigurationBeforeScenario(ScenarioContext scenarioContext, IObjectContainer objectContainer)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();

            scenarioContext.Add(ScenarioContextValues.CONFIGURATION_BUILDER, configurationBuilder);
            objectContainer.RegisterInstanceAs<IConfiguration>(configurationBuilder.Build());
        }
    }
}