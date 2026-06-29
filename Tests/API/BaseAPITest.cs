using AutomationExerciseTests.Helpers;
using RestSharp;

namespace AutomationExerciseTests.Tests.API
{
    public abstract class BaseAPITest
    {
        protected RestClient client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ReportManager.InitializeReport();
        }

        [SetUp]
        public virtual void Setup()
        {
            ReportManager.StartTest(TestContext.CurrentContext.Test.Name);
            client = new RestClient("https://automationexercise.com");
        }

        [TearDown]
        public virtual void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                ReportManager.LogFail($"Test failed: {TestContext.CurrentContext.Result.Message}");
            }
            else
            {
                ReportManager.LogPass("Test passed");
            }
            client?.Dispose();
        }
        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            ReportManager.FlushReport();
        }
    }
}
