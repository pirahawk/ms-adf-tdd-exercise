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
                .AddJsonFile($"appsettings.local.json", true, true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();
            scenarioContext.Add(ScenarioContextValues.CONFIGURATION_BUILDER, configurationBuilder);
            ADFTestConfiguration? adfTestConfiguration = configuration.Get<ADFTestConfiguration>();

            objectContainer.RegisterInstanceAs<IConfiguration>(configuration);
            objectContainer.RegisterInstanceAs<ADFTestConfiguration>(adfTestConfiguration!);
        }
    }
}