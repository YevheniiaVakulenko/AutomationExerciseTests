using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExerciseTests.Pages
{
    public class ProductsPage : BasePage
    {
        private readonly By searchInput = By.Id("search_product");
        private readonly By searchButton = By.Id("submit_search");
        private readonly By allProductCards = By.CssSelector(".product-image-wrapper");
        private readonly By searchedProductsHeading = By.XPath("//h2[contains(text(),'Searched Products')]");
        private readonly By continueShoppingButton = By.XPath("//button[contains(text(),'Continue Shopping')]");
        private readonly By addToCartModal = By.Id("cartModal");
        public ProductsPage(IWebDriver driver) : base(driver) { }

        public void SearchProduct(string keyword)
        {
            WaitForVisible(searchInput).SendKeys(keyword);
            driver.FindElement(searchButton).Click();
        }

        public bool HasSearchResults()
        {
            return driver.FindElements(allProductCards).Count > 0;
        }

        public int GetSearchResultsCount()
        {
            return driver.FindElements(allProductCards).Count;
        }

        public void AddFirstProductToCart()
        {
            var addToCartButtons = driver.FindElements(By.CssSelector(".add-to-cart"));
            WaitForEnabled(By.CssSelector(".add-to-cart"));
           ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", addToCartButtons[0]);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", addToCartButtons[0]);
        }

        public void AddProductToCartById(string productId)
        {
            var button = driver.FindElement(By.CssSelector($"a[data-product-id='{productId}']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);
            DismissAddToCartPopupIfPresent();
        }
        public bool IsAddToCartPopupDisplayed()
        {
            try
            {
                return WaitForVisible(addToCartModal).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public void DismissAddToCartPopupIfPresent()
        {
            try
            {
                var button = WaitForVisible(continueShoppingButton);
                button.Click();
            }
            catch (WebDriverTimeoutException)
            {
            }
        }
        public decimal GetProductPriceById(string productId)
        {
            string priceText = driver.FindElement(
                By.XPath($"//a[@data-product-id='{productId}']/ancestor::div[@class='productinfo text-center']//h2")
            ).Text;
            return ParsePrice(priceText);
        }
    }
}
