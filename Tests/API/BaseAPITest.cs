using RestSharp;

namespace AutomationExerciseTests.Tests.API
{
    public abstract class BaseAPITest
    {
        protected RestClient client;

        [SetUp]
        public virtual void Setup()
        {
            client = new RestClient("https://automationexercise.com");
        }

        [TearDown]
        public virtual void Teardown()
        {
            client?.Dispose();
        }
    }
}
