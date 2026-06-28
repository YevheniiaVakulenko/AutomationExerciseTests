using OpenQA.Selenium;

namespace AutomationExerciseTests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By loginEmailInput = By.XPath("//input[@data-qa='login-email']");
        private readonly By loginPasswordInput = By.XPath("//input[@data-qa='login-password']");
        private readonly By signUpNameInput = By.XPath("//input[@data-qa='signup-name']");
        private readonly By signUpEmailInput = By.XPath("//input[@data-qa='signup-email']");
        private readonly By loginButton = By.XPath("//button[@data-qa='login-button']");
        private readonly By signUpButton = By.XPath("//button[@data-qa='signup-button']");
        private readonly By loginErrorMessage = By.XPath("//p[contains(text(),'incorrect')]");
        private readonly By signupErrorMessage = By.XPath("//p[contains(text(),'Email Address already exist')]");
        public LoginPage(IWebDriver driver) : base(driver) { }
        public void Login(string email, string password)
        {
            WaitForVisible(loginEmailInput).SendKeys(email);
            driver.FindElement(loginPasswordInput).SendKeys(password);
            driver.FindElement(loginButton).Click();
        }
        public RegisterPage SignUp(string name, string email)
        {
            WaitForVisible(signUpNameInput).SendKeys(name);
            WaitForVisible(signUpEmailInput).SendKeys(email);
            driver.FindElement(signUpButton).Click();
            return new RegisterPage(driver);
        }
        public bool IsErrorMessageDisplayed()
        {
            try
            {
                return WaitForVisible(loginErrorMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public bool IsSignupErrorDisplayed()
        {
            try
            {
                return WaitForVisible(signupErrorMessage).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
