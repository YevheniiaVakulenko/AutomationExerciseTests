using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AutomationExerciseTests.Pages;
using WebDriverManager.Helpers;
using AutomationExerciseTests.Helpers;

namespace AutomationExerciseTests.Tests
{
    public abstract class BaseUITest
    {
        protected IWebDriver driver;
        protected HomePage homePage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ReportManager.InitializeReport();
        }

        [SetUp]
        public virtual void Setup()
        {
            ReportManager.StartTest(TestContext.CurrentContext.Test.Name);
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            var options = new ChromeOptions();
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://automationexercise.com");
            homePage = new HomePage(driver);
            homePage.AcceptCookiesIfPresent();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string path = $"FailureScreenshots/{TestContext.CurrentContext.Test.Name}.png";
                Directory.CreateDirectory("FailureScreenshots");
                screenshot.SaveAsFile(path);
                ReportManager.LogFail($"Test failed. Screenshot: {path}");
            }
            else
            {
                ReportManager.LogPass("Test passed");
            }
            driver?.Dispose();
        }
        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            ReportManager.FlushReport();
        }
    }
}
