using AutomationExerciseTests.Pages;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using WebDriverManager;

namespace AutomationExerciseTests.Tests
{
    public class CartTests
    {
        private IWebDriver driver;
        private HomePage homePage;

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://automationexercise.com");
            homePage = new HomePage(driver);
            homePage.AcceptCookiesIfPresent();
        }
        // TC-09
        [Test]
        public void AddMultipleProducts_CartTotal_ShouldBeCorrect()
        {
            var productsPage = homePage.ClickProductsLink();
            
            productsPage.AddProductToCartById("1");
            decimal price1 = productsPage.GetProductPriceById("1");

            productsPage.AddProductToCartById("2");
            decimal price2 = productsPage.GetProductPriceById("2");

            var cartPage = homePage.ClickCartLink();

            decimal expectedTotal = price1 + price2;
            Assert.That(cartPage.GetGrandTotal(), Is.EqualTo(expectedTotal));
        }

        // TC-10
        [Test]
        public void RemoveItemFromCart_ShouldUpdateItemCount()
        {
            var productsPage = homePage.ClickProductsLink();
            productsPage.AddProductToCartById("1");
            productsPage.AddProductToCartById("2");

            var cartPage = homePage.ClickCartLink();
            int countBefore = cartPage.GetItemCount();

            cartPage.RemoveItem(0);

            Assert.That(cartPage.GetItemCount(), Is.EqualTo(countBefore - 1));
        }

        // TC-11
        [Test]
        public void Checkout_WithoutLogin_ShouldDisplayLoginPopup()
        {
            var productsPage = homePage.ClickProductsLink();
            productsPage.AddFirstProductToCart();
            productsPage.DismissAddToCartPopupIfPresent();
            var cartPage = homePage.ClickCartLink();

            cartPage.ClickProceedToCheckout();

            Assert.That(cartPage.IsLoginPopupDisplayed(), Is.True);
        }
        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
        }
    }
}
