namespace AutomationExerciseTests.Tests.UI
{
    public class ProductTests : BaseUITest
    {
        // TC-06
        [Test]
        public void SearchProduct_WithValidKeyword_ShouldReturnResults()
        {
            var productsPage = homePage.ClickProductsLink();
            productsPage.SearchProduct("dress");

            Assert.That(productsPage.HasSearchResults(), Is.True);
        }

        // TC-07
        [Test]
        public void SearchProduct_WithNoMatches_ShouldShowEmptyState()
        {
            var productsPage = homePage.ClickProductsLink();
            productsPage.SearchProduct("zzzznonexistentproduct9999");

            Assert.That(productsPage.HasSearchResults(), Is.False);
        }

        // TC-08
        [Test]
        public void AddSingleProductToCart_ShouldAppearInCart()
        {
            var productsPage = homePage.ClickProductsLink();
            productsPage.AddFirstProductToCart();
            Assert.That(productsPage.IsAddToCartPopupDisplayed(), Is.True);
        }
    }
}
