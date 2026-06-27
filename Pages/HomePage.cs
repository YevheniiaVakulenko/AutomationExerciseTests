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
        private readonly By productsNavLink = By.XPath("//a[contains(text(),'Products')]");
        private readonly By cartNavLink = By.XPath("//a[contains(text(),'Cart')]");
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
        public ProductsPage ClickProductsLink()
        {
            WaitForEnabled(productsNavLink).Click();
            return new ProductsPage(driver);
        }
        public CartPage ClickCartLink()
        {
            WaitForEnabled(cartNavLink).Click();
            return new CartPage(driver);
        }
    }
}
