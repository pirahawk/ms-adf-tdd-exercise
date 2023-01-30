using FluentAssertions;
using TechTalk.SpecFlow;

namespace Ms.Tdd.Adf.Tests.Specs.Steps
{
    [Binding]
    public class TestSteps
    {
        [Given(@"I am writing a Test")]
        public void GivenIAmWritingATest()
        {
            var test = 1;
            test.Should().Be(1);
        }
    }
}