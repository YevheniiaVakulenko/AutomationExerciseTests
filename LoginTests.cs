using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AutomationExerciseTests
{
    public class LoginTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void HomePage_ShouldLoad_WithCorrectTitle()
        {
            driver.Navigate().GoToUrl("https://automationexercise.com");

            string title = driver.Title;

            Assert.That(title, Is.EqualTo("Automation Exercise"));
        }

        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
        }
    }
}