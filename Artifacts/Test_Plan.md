**Test Plan**

System Under Test: automationexercise.com

_Reference: Requirements Specification v2.0_

Author: Yevheniia Vakulenko | Version: 2.0 | Date: 29 June 2026

_Revision note: v2.0 updates risks, test case notes, and adds Section 12 (Findings Retrospective) based on confirmed behaviour discovered during test development._

# 1\. Introduction

This Test Plan defines the scope, approach, resources, and schedule for testing the UI and API layers of automationexercise.com, supporting a personal QA automation portfolio project. It follows the structure of an ISTQB-style test plan, scaled to a short, time-boxed individual project.

# 2\. Objectives

- Demonstrate structured, requirement-traced test design (manual case design before automation).
- Validate core account, browsing, and cart/checkout flows via automated UI tests using Selenium WebDriver + C# + NUnit, structured with the Page Object Model.
- Validate the documented REST API endpoints using RestSharp + NUnit, including positive, negative, and method-not-allowed cases.
- Produce a maintainable, version-controlled automation suite suitable as a public portfolio artifact, with a CI pipeline and test report.
- Correct and refine the test approach as real system behaviour is discovered, rather than treating initial assumptions as fixed (see Section 12).

# 3\. Scope

## 3.1 In Scope

- Account registration, login (valid/invalid), logout, account deletion (UI).
- Product search, including no-results state (UI).
- Add to cart, remove from cart, cart total verification (UI).
- Login required before checkout (UI).
- /api/productsList, /api/brandsList, /api/searchProduct, /api/verifyLogin, /api/createAccount, /api/deleteAccount (API).

## 3.2 Out of Scope

- Payment gateway / real transaction processing.
- Performance, load, and security testing.
- Cross-browser matrix beyond Chrome (Firefox treated as stretch goal only).
- Mobile responsive layout testing.

# 4\. Test Strategy

## 4.1 Levels and Types

- Functional UI testing - automated, via Selenium WebDriver (C#), structured with Page Object Model.
- API testing - automated, via RestSharp/HttpClient (C#), covering schema, status code, and negative-path validation. Status validation is performed against the JSON response body's responseCode field, not solely the HTTP status code - see Section 12.2.
- Regression - full suite re-run on each code change, via CI on every push and pull request.

## 4.2 Test Design Technique

Test cases are derived using equivalence partitioning (valid/invalid login, valid/invalid search terms) and boundary/negative testing (missing required API parameters, disallowed HTTP methods), consistent with REQ-UI and API requirements in the Requirements Specification.

# 5\. Test Environment

| **Item**                      | **Detail**                                                               |
| ----------------------------- | ------------------------------------------------------------------------ |
| Application under test        | <https://automationexercise.com> (public, live)                          |
| Browser                       | Google Chrome (latest stable), headless in CI                            |
| Automation language/framework | C# / .NET 8, NUnit, Selenium WebDriver, RestSharp                        |
| Driver management             | WebDriverManager.Net (explicit MatchingBrowser version-resolve strategy) |
| Reporting                     | ExtentReports (HTML), with failure screenshots                           |
| CI                            | GitHub Actions, triggered on push and pull request to main               |
| Defect tracking (simulated)   | GitHub Issues                                                            |

# 6\. Entry and Exit Criteria

## 6.1 Entry Criteria

- Requirements Specification reviewed and baselined (v2.0).
- Test environment (toolchain, dependencies) installed and verified.
- Site is reachable and core pages load without error.

## 6.2 Exit Criteria

- All planned test cases executed at least once.
- No outstanding High-severity defects in core flows (signup, login, cart) without a documented workaround.
- Automated suite runs cleanly from a fresh clone and in CI, and produces a report.

# 7\. Test Case Summary

Full step-by-step test cases are maintained in the project's test classes directly, organised under Tests/UI and Tests/API. The table below summarises coverage by requirement ID for traceability, with notes reflecting confirmed behaviour where it affects how the test case should actually be implemented.

| **ID** | **Requirement**                    | **Title**                                                        | **Pri.** | **Notes**                                                                                                      |
| ------ | ---------------------------------- | ---------------------------------------------------------------- | -------- | -------------------------------------------------------------------------------------------------------------- |
| TC-01  | REQ-UI-01                          | Register new user with valid details                             | High     |                                                                                                                |
| TC-02  | REQ-UI-02                          | Register with already-used email is rejected                     | High     | Precondition account created via API for speed/isolation                                                       |
| TC-03  | REQ-UI-03                          | Login with valid credentials                                     | High     | Precondition account created via API                                                                           |
| TC-04  | REQ-UI-04                          | Login with invalid credentials is rejected                       | High     |                                                                                                                |
| TC-05  | REQ-UI-05                          | Logout returns user to unauthenticated state                     | Med      | Requires logged-in precondition via API + UI login                                                             |
| TC-06  | REQ-UI-08                          | Search for an existing product keyword                           | High     |                                                                                                                |
| TC-07  | REQ-UI-09                          | Search with no matching results shows empty state                | Med      | Revised: assert on zero rendered product cards, not a message (Section 12.1)                                   |
| TC-08  | REQ-UI-12                          | Add single product to cart                                       | High     | Must dismiss add-to-cart confirmation modal; may require JS click due to ad interception (Section 12.5)        |
| TC-09  | REQ-UI-13                          | Cart total is correct for multiple items                         | High     | Expected total derived from product prices read independently on Products page, not from the cart's own totals |
| TC-10  | REQ-UI-14                          | Remove item from cart updates total                              | Med      | Requires explicit wait for row-count change; removal is asynchronous                                           |
| TC-11  | REQ-UI-15                          | Checkout without login redirects to login/signup                 | High     |                                                                                                                |
| TC-12  | API: productsList                  | GET productsList returns 200 and product list                    | High     |                                                                                                                |
| TC-13  | API: productsList                  | POST productsList returns responseCode 405                       | Low      | Revised: assert on JSON body responseCode, not HTTP status (Section 12.2)                                      |
| TC-14  | API: brandsList                    | GET brandsList returns 200 and brand list                        | Med      |                                                                                                                |
| TC-15  | API: searchProduct                 | POST searchProduct with valid term returns matches               | High     | Assertion must allow match on category as well as name (Section 12.4)                                          |
| TC-16  | API: searchProduct                 | POST searchProduct missing parameter returns error message       | High     |                                                                                                                |
| TC-17  | API: verifyLogin                   | POST verifyLogin with valid credentials returns success          | High     | Precondition account created via ApiHelper                                                                     |
| TC-18  | API: verifyLogin                   | POST verifyLogin with missing parameters returns error           | High     |                                                                                                                |
| TC-19  | API: verifyLogin                   | POST verifyLogin with non-existent user returns 'User not found' | Med      |                                                                                                                |
| TC-20  | API: createAccount / deleteAccount | Create then delete a disposable test account                     | High     | deleteAccount requires multipart/form-data; form/query params confirmed insufficient (Section 12.3)            |

_Test data for API account-creation/deletion cases uses disposable, uniquely-generated emails per run, since this executes against the live public site - never a shared or personal account._

# 8\. Risks and Mitigations

| **Risk**                                                                                                       | **Likelihood** | **Impact** | **Mitigation**                                                                                                |
| -------------------------------------------------------------------------------------------------------------- | -------------- | ---------- | ------------------------------------------------------------------------------------------------------------- |
| Site is a shared public demo; data or markup may change without notice                                         | Medium         | Medium     | Keep locators resilient (prefer stable attributes over text/position); re-baseline if site changes            |
| No staging environment for destructive operations                                                              | Low            | High       | Use disposable test accounts only; never reuse personal credentials                                           |
| Rate limiting / flakiness on shared public infrastructure                                                      | Medium         | Low        | Add explicit waits and limited retry logic in CI; avoid hammering endpoints in tight loops                    |
| Third-party advertisement content intercepts UI clicks (CONFIRMED, Section 12.5)                               | Medium         | Medium     | JavaScript-executed click on the target element, bypassing coordinate-based interception                      |
| API success/failure communicated inconsistently across HTTP status vs. response body (CONFIRMED, Section 12.2) | High           | High       | All API assertions check the JSON body's responseCode/message rather than relying on HTTP status alone        |
| Inconsistent parameter encoding requirements between endpoints (CONFIRMED, Section 12.3)                       | Medium         | High       | Encoding requirements verified per-endpoint via direct request inspection before relying on assumed behaviour |

# 9\. Schedule

| **Phase** | **Focus**                                                                       | **Deliverable**                                        |
| --------- | ------------------------------------------------------------------------------- | ------------------------------------------------------ |
| Phase 1   | Environment setup; exploratory testing; raw Selenium scripts against core pages | Working WebDriver scaffold, locator notes              |
| Phase 2   | Refactor into Page Object Model; complete UI test cases TC-01 to TC-11          | UI suite green in NUnit                                |
| Phase 3   | API automation for TC-12 to TC-20 via RestSharp                                 | API suite green in NUnit                               |
| Phase 4   | Reporting (ExtentReports), GitHub Actions CI, README, documentation set         | Public repo with CI badge                              |
| Phase 5   | Findings retrospective and documentation revision                               | Requirements Spec v2.0, Test Plan v2.0 (this document) |

# 10\. Roles and Responsibilities

Single-contributor project: Yevheniia Vakulenko acts as test analyst, automation engineer, and reviewer for all items in this plan.

# 11\. Deliverables

- Requirements Specification (this document's companion), v2.0.
- This Test Plan, v2.0.
- Automated UI suite (Selenium WebDriver, C#, POM, NUnit).
- Automated API suite (RestSharp, C#, NUnit).
- HTML test execution report (ExtentReports), with failure screenshots.
- GitHub repository with CI workflow and README.

# 12\. Findings Retrospective

The following findings were confirmed during hands-on test development and execution, and directly changed how specific test cases needed to be implemented. They are documented here, separately from the original plan, to make the distinction between initial assumption and confirmed behaviour explicit - a standard and expected part of real test planning, not a sign the original plan was flawed.

## 12.1 No empty-state message on search (affects TC-07)

**CONFIRMED BY TESTING:** A search returning no matches renders zero product cards with no accompanying "no results" message. TC-07 was implemented to assert on the absence of product elements rather than the presence of any message text.

## 12.2 HTTP status code does not reflect actual API outcome (affects TC-13 and all API test cases)

**CONFIRMED BY TESTING:** All API responses return HTTP 200 regardless of business-logic outcome. The real result is in the response body's responseCode field. Every API test case in this suite asserts against the response body, not solely the HTTP status code.

## 12.3 deleteAccount requires multipart/form-data (affects TC-20)

**CONFIRMED BY TESTING:** Standard form-encoded parameters and query-string parameters both produced "email parameter is missing" errors on /api/deleteAccount, despite the email being present in the request. Multipart/form-data encoding, confirmed against a working Postman request, resolved this. /api/updateAccount has not been independently verified and should not be assumed to share createAccount's simpler encoding without testing.

## 12.4 Search matches category as well as product name (affects TC-15)

**CONFIRMED BY TESTING:** A search for "dress" returned a result whose category metadata matched the term even though its product name did not. TC-15's assertion was revised to accept a match on name OR category, reflecting the search's actual, confirmed matching behaviour rather than the originally assumed name-only matching.

## 12.5 Third-party ad iframes intercept add-to-cart clicks (affects TC-08, TC-09, TC-10)

**CONFIRMED BY TESTING:** Add-to-cart buttons on the Products page intermittently sit beneath a Google Ads iframe at the moment of click, causing ElementClickInterceptedException. This is unrelated to the application under test. Mitigated using a JavaScript-executed click on the target element, which bypasses the coordinate-based interception check Selenium's native click performs.