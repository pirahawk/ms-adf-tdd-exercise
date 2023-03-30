using Dapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Data.SqlClient;
using Ms.Tdd.Adf.Tests.Specs.Models;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class AzureSqlDatabaseTestSteps
    {
        private readonly SqlConnection sqlConnection;

        public AzureSqlDatabaseTestSteps(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        [Then(@"the SQL table MatchScores contains the following entries")]
        public void ThenTheSQLTableMatchScoresContainsTheFollowingEntries(Table table)
        {
            var expectedRows = table.Rows.Select(row => new MatchRow {
                Home = row[0],
                Away = row[1],
                ScoreHome = int.Parse(row[2]),
                ScoreAway = int.Parse(row[3]),
                Result = row[4],
            });

            var query = $"select [Id],[Home],[Away],[ScoreHome],[ScoreAway],[Result] from MatchScores";
            
            var actualResults = sqlConnection
                                .Query<MatchRow>(query)
                                .ToArray();
            using (var assertionScope = new AssertionScope())
            {
                
                foreach (MatchRow expectedRow in expectedRows)
                {
                    var reason = $"Could not find record - [Home]:{expectedRow.Home},[Away]:{expectedRow.Away},[ScoreHome]:{expectedRow.ScoreHome},[ScoreAway]:{expectedRow.ScoreAway},[Result]:{expectedRow.Result}";
                    actualResults.Any(row =>
                        row.Home!.Equals(expectedRow.Home)
                        && row.Away!.Equals(expectedRow.Away)
                        && row.ScoreHome.Equals(expectedRow.ScoreHome)
                        && row.ScoreAway.Equals(expectedRow.ScoreAway)
                        && row.Result!.Equals(expectedRow.Result))
                        .Should().BeTrue(reason);
                }
            }
        }
    }
}