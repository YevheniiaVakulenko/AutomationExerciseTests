using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExerciseTests.Pages
{
    public class RegisterPage : BasePage
    {
        private readonly By mrRadio = By.Id("id_gender1");
        private readonly By mrsRadio = By.Id("id_gender2");
        private readonly By passwordInput = By.Id("password");
        private readonly By daysDropdown = By.Id("days");
        private readonly By monthsDropdown = By.Id("months");
        private readonly By yearsDropdown = By.Id("years");
        private readonly By firstNameInput = By.Id("first_name");
        private readonly By lastNameInput = By.Id("last_name");
        private readonly By addressInput = By.Id("address1");
        private readonly By countryDropdown = By.Id("country");
        private readonly By stateInput = By.Id("state");
        private readonly By cityInput = By.Id("city");
        private readonly By zipcodeInput = By.Id("zipcode");
        private readonly By mobileNumberInput = By.Id("mobile_number");
        private readonly By createAccountButton = By.XPath("//button[@data-qa='create-account']");
        private readonly By accountCreatedHeading = By.XPath("//h2[@data-qa='account-created']");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        public void FillAccountDetails(string password, string firstName, string lastName,
            string address, string state, string city, string zipcode, string mobileNumber,
            string title = "Mrs", string country = "India",
            string day = "10", string month = "January", string year = "1995")
        {
            WaitForVisible(passwordInput).SendKeys(password);

            if (title == "Mr")
                driver.FindElement(mrRadio).Click();
            else
                driver.FindElement(mrsRadio).Click();

            new SelectElement(driver.FindElement(daysDropdown)).SelectByValue(day);
            new SelectElement(driver.FindElement(monthsDropdown)).SelectByText(month);
            new SelectElement(driver.FindElement(yearsDropdown)).SelectByValue(year);

            driver.FindElement(firstNameInput).SendKeys(firstName);
            driver.FindElement(lastNameInput).SendKeys(lastName);
            driver.FindElement(addressInput).SendKeys(address);
            new SelectElement(driver.FindElement(countryDropdown)).SelectByText(country);
            driver.FindElement(stateInput).SendKeys(state);
            driver.FindElement(cityInput).SendKeys(city);
            driver.FindElement(zipcodeInput).SendKeys(zipcode);
            driver.FindElement(mobileNumberInput).SendKeys(mobileNumber);
        }

        public void SubmitAccountCreation()
        {
            WaitForEnabled(createAccountButton).Click();
        }

        public bool IsAccountCreatedConfirmationDisplayed()
        {
            return WaitForVisible(accountCreatedHeading).Displayed;
        }
    }
}
