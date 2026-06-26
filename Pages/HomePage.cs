using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Pages
{
    public class HomePage : BasePage
    {
        private readonly By signupLoginLink = By.XPath("//a[contains(text(),'Signup / Login')]");
        private readonly By loggedInAsText = By.XPath("//a[contains(text(),'Logged in as')]");
        public HomePage(IWebDriver driver) : base(driver) { }

        public string GetTitle()
        {
            return driver.Title;
        }

        public LoginPage ClickSignupLogin()
        {
            WaitForEnabled(signupLoginLink).Click();
            return new LoginPage(driver);
        }
        public bool IsLoggedIn()
        {
            try
            {
                return WaitForVisible(loggedInAsText).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string GetLoggedInUsername()
        {
            string fullText = WaitForVisible(loggedInAsText).Text;
            return fullText.Replace("Logged in as ", "").Trim();
        }
    }
}
