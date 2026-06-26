using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By loginEmailInput = By.XPath("//input[@data-qa='login-email']");
        private readonly By loginPasswordInput = By.XPath("//input[@data-qa='login-password']");
        private readonly By loginButton = By.XPath("//button[@data-qa='login-button']");
        private readonly By loginErrorMessage = By.XPath("//p[contains(text(),'incorrect')]");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Login(string email, string password)
        {
            WaitForVisible(loginEmailInput).SendKeys(email);
            driver.FindElement(loginPasswordInput).SendKeys(password);
            driver.FindElement(loginButton).Click();
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
    }
}
