using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class TestSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly IConfiguration configuration;

        public TestSteps(ScenarioContext scenarioContext, IConfiguration configuration)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [Given(@"I am writing a Test")]
        public void GivenIAmWritingATest()
        {
            var a = AzureDataFactoryConfiguration;
            var test = 1;
            test.Should().Be(1);
        }

        private AzureDataFactoryConfiguration? AzureDataFactoryConfiguration => configuration.GetSection("AzureDataFactory").Get<AzureDataFactoryConfiguration>();
    }
}