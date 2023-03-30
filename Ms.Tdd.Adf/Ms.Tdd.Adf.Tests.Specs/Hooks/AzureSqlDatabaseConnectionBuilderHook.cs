using BoDi;
using Microsoft;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Hooks
{
    [Binding]
    public class AzureSqlDatabaseConnectionBuilderHook
    {
        [BeforeScenario(Order = 1)]
        public static void BuildDatabaseConnection(
            ScenarioContext scenarioContext,
            IObjectContainer objectContainer,
            IConfiguration configuration)
        {
            Assumes.NotNull(configuration);

            AzureSqlDatabaseConfiguration? azureSqlDatabaseConfiguration = configuration.GetSection("AzureSqlDb").Get<AzureSqlDatabaseConfiguration>();
            Assumes.NotNull(azureSqlDatabaseConfiguration);
            Assumes.NotNullOrEmpty(azureSqlDatabaseConfiguration!.ConnectionString);

            var sqlConnection = new SqlConnection(azureSqlDatabaseConfiguration!.ConnectionString);

            objectContainer.RegisterInstanceAs(azureSqlDatabaseConfiguration);
            objectContainer.RegisterInstanceAs(sqlConnection);
        }
    }
}