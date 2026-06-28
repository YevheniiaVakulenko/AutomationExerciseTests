using OpenQA.Selenium;
using System.Linq;

namespace AutomationExerciseTests.Pages
{
    public class CartPage : BasePage
    {
        private readonly By cartRows = By.CssSelector("#cart_info_table tbody tr");
        private readonly By proceedToCheckoutButton = By.XPath("//a[contains(text(),'Proceed To Checkout')]");
        private readonly By checkoutModal = By.Id("checkoutModal");
        public CartPage(IWebDriver driver) : base(driver) { }

        public int GetItemCount()
        {
            return driver.FindElements(cartRows).Count;
        }

        public decimal GetLineTotal(int rowIndex)
        {
            var rows = driver.FindElements(cartRows);
            string priceText = rows[rowIndex].FindElement(By.CssSelector(".cart_total_price")).Text;
            return ParsePrice(priceText);
        }

        public decimal GetGrandTotal()
        {
            var rows = driver.FindElements(cartRows);
            decimal total = 0;
            foreach (var row in rows)
            {
                string priceText = row.FindElement(By.CssSelector(".cart_total_price")).Text;
                total += ParsePrice(priceText);
            }
            return total;
        }

        public void RemoveItem(int rowIndex)
        {
            int countBefore = driver.FindElements(cartRows).Count;
            var rows = driver.FindElements(cartRows);
            rows[rowIndex].FindElement(By.CssSelector(".cart_quantity_delete")).Click();
            wait.Until(d => d.FindElements(cartRows).Count < countBefore);
        }

        public void ClickProceedToCheckout()
        {
            WaitForEnabled(proceedToCheckoutButton).Click();
        }
        public bool IsLoginPopupDisplayed()
        {
            try
            {
                return WaitForVisible(checkoutModal).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
