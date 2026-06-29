# Automation Exercise Test Suite

A C# test automation framework covering both **UI** and **API** testing for [automationexercise.com](https://automationexercise.com), built as a hands-on portfolio project demonstrating Selenium WebDriver, the Page Object Model, REST API automation, and CI/CD with GitHub Actions.

## Overview

This project tests an e-commerce demo site end-to-end: account registration and login, product search, cart management, checkout, and the site's documented REST API.

## Tech Stack

| Layer | Tools |
|---|---|
| Language / Runtime | C#, .NET 8 |
| UI Automation | Selenium WebDriver, WebDriverManager.Net |
| API Automation | RestSharp |
| Test Framework | NUnit |
| Reporting | ExtentReports (HTML) |
| CI/CD | GitHub Actions |

## Project Structure

```
AutomationExerciseTests/
├── Helpers/
│   ├── ApiHelper.cs          # Shared API-driven account setup/teardown
│   ├── ProductsResponse.cs   # API response model
│   ├── ReportManager.cs      # ExtentReports wiring
│   └── UserTestData.cs       # GUID-based disposable test user factory
├── Pages/                    # Page Object Model classes
│   ├── BasePage.cs
│   ├── CartPage.cs
│   ├── HomePage.cs
│   ├── LoginPage.cs
│   ├── ProductsPage.cs
│   └── RegisterPage.cs
├── Tests/
│   ├── API/
│   │   ├── AccountAPITests.cs
│   │   ├── BaseAPITest.cs
│   │   ├── ProductsAPITests.cs
│   │   └── SearchAPITests.cs
│   └── UI/
│       ├── BaseUITests.cs
│       ├── CartTests.cs
│       ├── LoginTests.cs
│       └── ProductTests.cs
└── .github/workflows/
    └── tests.yml              # CI pipeline definition
```

## Running the Tests

**Prerequisites:** .NET 8 SDK, Google Chrome installed.

```bash
git clone https://github.com/YevheniiaVakulenko/AutomationExerciseTests.git
cd AutomationExerciseTests
dotnet restore
dotnet test
```

An HTML report is generated at `bin/Debug/net8.0/TestReport.html` after each run.

## Continuous Integration

Every push and pull request targeting `main` triggers the test suite via GitHub Actions — see [`.github/workflows/tests.yml`](.github/workflows/tests.yml). Test results and failure screenshots are uploaded as run artifacts.

## Known Limitations

- Occasional `ElementClickInterceptedException` failures can occur due to third-party ad content on the live site; add-to-cart interactions fall back to a JavaScript-executed click to mitigate this.

## Author

**Yevheniia Vakulenko**
[LinkedIn](https://www.linkedin.com/in/yevheniia-vakulenko-6a732733a)
