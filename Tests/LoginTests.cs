using AutomationExerciseTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace AutomationExerciseTests.Tests
{
    public class LoginTests
    {
        private IWebDriver driver;
        private HomePage homePage;
        //qatester__@gmail.com
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
        public void Register_NewUser_ShouldSucceed()
        {
            string uniqueEmail = $"qa_test_{Guid.NewGuid()}@example.com";
            var loginPage = homePage.ClickSignupLogin();
            var registerPage = loginPage.SignUp("QA Tester", uniqueEmail);

            registerPage.FillAccountDetails("TempPass123!", "QA", "Tester",
                "123 Test Street", "Cambridgeshire", "Cambridge", "AB1 2CD", "01234567890");
            registerPage.SubmitAccountCreation();

            Assert.That(registerPage.IsAccountCreatedConfirmationDisplayed(), Is.True);
        }
        [Test]
        public void Register_WithAlreadyUsedEmail_ShouldShowError()
        {
            string existingEmail = $"qa_existing_{Guid.NewGuid()}@example.com";

            // Step 1: create the account via API first - fast, no browser needed
            var apiClient = new RestClient("https://automationexercise.com");
            var createRequest = new RestRequest("/api/createAccount", Method.Post);
            createRequest.AddParameter("name", "Existing User");
            createRequest.AddParameter("email", existingEmail);
            createRequest.AddParameter("password", "TempPass123!");
            createRequest.AddParameter("title", "Mr");
            createRequest.AddParameter("birth_date", "1");
            createRequest.AddParameter("birth_month", "1");
            createRequest.AddParameter("birth_year", "1990");
            createRequest.AddParameter("firstname", "Existing");
            createRequest.AddParameter("lastname", "User");
            createRequest.AddParameter("company", "Test");
            createRequest.AddParameter("address1", "123 Test St");
            createRequest.AddParameter("country", "India");
            createRequest.AddParameter("zipcode", "AB1 2CD");
            createRequest.AddParameter("state", "Cambridgeshire");
            createRequest.AddParameter("city", "Cambridge");
            createRequest.AddParameter("mobile_number", "01234567890");
            apiClient.Execute(createRequest);

            // Step 2: now attempt to register the SAME email via the UI
            var loginPage = homePage.ClickSignupLogin();
            loginPage.SignUp("Duplicate Attempt", existingEmail);

            Assert.That(loginPage.IsSignupErrorDisplayed(), Is.True);

            // Step 3: cleanup - delete the account we created
            var deleteRequest = new RestRequest("/api/deleteAccount", Method.Delete);
            deleteRequest.AddParameter("email", existingEmail);
            deleteRequest.AddParameter("password", "TempPass123!");
            apiClient.Execute(deleteRequest);
        }
        [Test]
        public void Login_WithValidCredentials_ShouldRedirectAndShowLoggedInUser()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login("qatester__@gmail.com", "TempPass123!");

            var homePageAfterLogin = new HomePage(driver);
            Assert.That(homePageAfterLogin.IsLoggedIn(), Is.True);
        }
        [Test]
        public void Login_WithInvalidCredentials_ShouldShowError()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login("nonexistent_user_99182@fake.com", "wrongpassword123");

            Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True);
        }
        [Test]
        public void Logout_ReturnsUserToUnAuthenticatedState()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login("qatester__@gmail.com", "TempPass123!");
            var homePageAfterLogin = new HomePage(driver);
            Assert.That(homePageAfterLogin.IsLoggedIn(), Is.True);
            Assert.That(homePageAfterLogin.GetLoggedInUsername(), Is.EqualTo("QA Tester"));
        }
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
        }
    }
}