using AutomationExerciseTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AutomationExerciseTests.Tests
{
    public class LoginTests
    {
        private IWebDriver driver;
        private HomePage homePage;

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://automationexercise.com");
            homePage = new HomePage(driver);
            homePage.AcceptCookiesIfPresent();
        }

        [Test]
        public void HomePage_ShouldLoad_WithCorrectTitle()
        {
            Assert.That(homePage.GetTitle(), Is.EqualTo("Automation Exercise"));
        }

        [Test]
        public void Login_WithInvalidCredentials_ShouldShowError()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login("nonexistent_user_99182@fake.com", "wrongpassword123");

            Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True);
        }
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
        }
    }
}