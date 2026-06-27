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
    public class ProductTests
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

        [TearDown]
        public void Teardown()
        {
            driver?.Dispose();
        }
    }
}
