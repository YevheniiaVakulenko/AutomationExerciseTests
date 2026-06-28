using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AutomationExerciseTests.Pages;
using WebDriverManager.Helpers;

namespace AutomationExerciseTests.Tests
{
    public abstract class BaseUITest
    {
        protected IWebDriver driver;
        protected HomePage homePage;

        [SetUp]
        public virtual void Setup()
        {
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
            driver?.Dispose();
        }
    }
}
