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
    }
}
