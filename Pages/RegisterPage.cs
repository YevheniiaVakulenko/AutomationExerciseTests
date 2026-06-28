using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AutomationExerciseTests.Helpers;

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
        private readonly By companyInput = By.Id("company");
        private readonly By addressInput = By.Id("address1");
        private readonly By countryDropdown = By.Id("country");
        private readonly By stateInput = By.Id("state");
        private readonly By cityInput = By.Id("city");
        private readonly By zipcodeInput = By.Id("zipcode");
        private readonly By mobileNumberInput = By.Id("mobile_number");
        private readonly By createAccountButton = By.XPath("//button[@data-qa='create-account']");
        private readonly By accountCreatedHeading = By.XPath("//h2[@data-qa='account-created']");

        public RegisterPage(IWebDriver driver) : base(driver) { }

        public void FillAccountDetails(UserTestData user)
        {
            WaitForVisible(passwordInput).SendKeys(user.Password);

            if (user.Title == "Mr")
                driver.FindElement(mrRadio).Click();
            else
                driver.FindElement(mrsRadio).Click();

            new SelectElement(driver.FindElement(daysDropdown)).SelectByValue(user.Birth_day);
            new SelectElement(driver.FindElement(monthsDropdown)).SelectByValue(user.Birth_month);
            new SelectElement(driver.FindElement(yearsDropdown)).SelectByValue(user.Birth_year);

            driver.FindElement(firstNameInput).SendKeys(user.FirstName);
            driver.FindElement(lastNameInput).SendKeys(user.LastName);
            driver.FindElement(companyInput).SendKeys(user.Company);
            driver.FindElement(addressInput).SendKeys(user.Address);
            new SelectElement(driver.FindElement(countryDropdown)).SelectByText(user.Country);
            driver.FindElement(stateInput).SendKeys(user.State);
            driver.FindElement(cityInput).SendKeys(user.City);
            driver.FindElement(zipcodeInput).SendKeys(user.Zipcode);
            driver.FindElement(mobileNumberInput).SendKeys(user.MobileNumber);
        }

        public void SubmitAccountCreation()
        {
            var createAccountBtn = driver.FindElement(createAccountButton);
            WaitForEnabled(createAccountButton);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", createAccountBtn);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", createAccountBtn);
        }

        public bool IsAccountCreatedConfirmationDisplayed()
        {
            return WaitForVisible(accountCreatedHeading).Displayed;
        }
    }
}
