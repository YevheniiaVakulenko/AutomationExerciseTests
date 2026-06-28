using AutomationExerciseTests.Helpers;
using AutomationExerciseTests.Pages;

namespace AutomationExerciseTests.Tests
{
    public class LoginTests : BaseUITest
    {
        private UserTestData user;
        [SetUp]
        public override void Setup()
        {
            base.Setup();
            user = UserTestData.GenerateRandomUser();
            ApiHelper.CreateAccountViaApi(user);
        }

        [Test]
        public void HomePage_ShouldLoad_WithCorrectTitle()
        {
            Assert.That(homePage.GetTitle(), Is.EqualTo("Automation Exercise"));
        }
        // TC-01
        [Test]
        public void Register_NewUser_ShouldSucceed()
        {
            user = UserTestData.GenerateRandomUser();
            var loginPage = homePage.ClickSignupLogin();
            var registerPage = loginPage.SignUp(user.Name, user.Email);

            registerPage.FillAccountDetails(user);
            registerPage.SubmitAccountCreation();

            Assert.That(registerPage.IsAccountCreatedConfirmationDisplayed(), Is.True);

        }
        // TC-02
        [Test]
        public void Register_WithAlreadyUsedEmail_ShouldShowError()
        {
            
            var loginPage = homePage.ClickSignupLogin();
            loginPage.SignUp("Duplicate Attempt", user.Email);

            Assert.That(loginPage.IsSignupErrorDisplayed(), Is.True);
        }

        // TC-03
        [Test]
        public void Login_WithValidCredentials_ShouldRedirectAndShowLoggedInUser()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login(user.Email, user.Password);

            var homePageAfterLogin = new HomePage(driver);
            Assert.That(homePageAfterLogin.IsLoggedIn(), Is.True);
        }
        // TC-04
        [Test]
        public void Login_WithInvalidCredentials_ShouldShowError()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login("nonexistent_user_99182@fake.com", "wrongpassword123");

            Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True);
        }
        // TC-05
        [Test]
        public void Logout_ReturnsUserToUnAuthenticatedState()
        {
            var loginPage = homePage.ClickSignupLogin();
            loginPage.Login(user.Email, user.Password);

            var homePageAfterLogin = new HomePage(driver);
            Assert.That(homePageAfterLogin.IsLoggedIn(), Is.True);
            Assert.That(homePageAfterLogin.GetLoggedInUsername(), Is.EqualTo("QA Tester"));
        }
        [TearDown]
        public override void TearDown()
        {
            ApiHelper.DeleteAccountViaApi(user);
            base.TearDown();
        }
    }
}