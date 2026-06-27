using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.BiDi.BrowsingContext;

namespace AutomationExerciseTests.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;
        private readonly By cookieAcceptButton = By.CssSelector("button[aria-label='Consent']");
        protected BasePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement WaitForVisible(By locator)
        {
            return wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed ? element : null;
            });
        }

        protected IWebElement WaitForEnabled(By locator)
        {
            return wait.Until(driver =>
            {
                var element = driver.FindElement(locator);
                return element.Displayed && element.Enabled ? element : null;
            });
        }

        public void AcceptCookiesIfPresent()
        {
            try
            {
                WaitForEnabled(cookieAcceptButton).Click();
            }
            catch (WebDriverTimeoutException)
            {
            }
        }
        protected decimal ParsePrice(string rawText)
        {
            string cleaned = rawText.Replace("Rs.", "").Trim();
            return decimal.Parse(cleaned);
        }
        
    }
}
