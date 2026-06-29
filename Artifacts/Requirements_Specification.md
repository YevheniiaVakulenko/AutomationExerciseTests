**Software Requirements Specification**

System Under Test: automationexercise.com

_Document type: Reverse-engineered specification, prepared for QA test-design purposes_

Author: Yevheniia Vakulenko | Version: 2.0 | Date: 29 June 2026

_Revision note: v2.0 incorporates confirmed findings from automation development and execution (see Section 8)._

_This document was not produced by the system owner. It was reverse-engineered by observing the live, publicly available website and its documented API list, for the sole purpose of designing a structured QA automation portfolio project. It does not claim to represent the original product requirements._

# 1\. Purpose and Scope

This document defines the functional requirements of automationexercise.com as understood through direct exploration of the live application and its published API list, for the purpose of designing a representative test plan and an automated test suite (UI via Selenium WebDriver in C#, API via RestSharp/NUnit).

Scope covers: account management, product browsing and search, cart and checkout flow, and the subset of REST API endpoints documented by the site for automation practice. Payment processing, third-party integrations, and performance/load characteristics are out of scope for this project.

# 2\. System Overview

Automation Exercise is a publicly available e-commerce demonstration site, purpose-built by its maintainers for QA automation practice. It exposes both a browsable storefront (HTML/UI) and a set of REST endpoints under /api/, making it suitable for combined UI and API test automation exercises.

# 3\. Functional Requirements - UI

## 3.1 Account Management

- REQ-UI-01: The system shall allow a new user to register via a Signup/Login page by providing name and email, followed by a detailed account form (password, DOB, name, address, country, etc.).
- REQ-UI-02: The system shall reject signup attempts using an email address already registered, and shall display an appropriate message.
- REQ-UI-03: The system shall allow a registered user to log in with a valid email and password.
- REQ-UI-04: The system shall reject login attempts with incorrect credentials and display an error message without revealing whether the email or password was the invalid part.
- REQ-UI-05: The system shall allow a logged-in user to log out, returning them to an unauthenticated state.
- REQ-UI-06: The system shall allow an existing account to be deleted from within the account flow.

## 3.2 Product Browsing and Search

- REQ-UI-07: The system shall display a paginated/listed catalogue of products on the Products page.
- REQ-UI-08: The system shall allow a user to search for products by keyword and display matching results.
- REQ-UI-09 (REVISED - see Section 8.1): The system shall display zero product results when a search returns no matches. No explicit empty-state message is rendered; absence of an empty-state message is the confirmed, actual behavior.
- REQ-UI-08a (NEW - see Section 8.4): Search matching is not limited to the product name field. Products may be returned as a match based on category metadata even when the search term does not appear in the product's displayed name.
- REQ-UI-10: The system shall allow filtering or browsing of products by category and by brand.
- REQ-UI-11: The system shall display full product details (name, price, availability, category, description) on a dedicated product page.

## 3.3 Cart and Checkout

- REQ-UI-12: The system shall allow a user (guest or logged-in) to add one or more products to a cart, including a selectable quantity.
- REQ-UI-12a (NEW - see Section 8.5): Upon adding a product to the cart, the system displays a confirmation modal ("Continue Shopping" / view cart options) which must be dismissed before further page interaction.
- REQ-UI-13: The system shall allow a user to view cart contents, with correct line totals and a correct cart-level total.
- REQ-UI-14: The system shall allow a user to remove an item from the cart, with totals recalculated accordingly. Removal is asynchronous; the row is removed from the DOM without a full page reload.
- REQ-UI-15: The system shall require login or registration before allowing checkout to be completed.
- REQ-UI-16: The system shall allow a logged-in user to proceed from cart to a checkout/order confirmation step.

# 4\. Functional Requirements - API

The following endpoints are documented by the system under automationexercise.com/api_list and form the basis of the API automation scope. Request/response details below reflect confirmed, observed behaviour, not assumed behaviour based on documentation alone.

| **Endpoint**              | **Method(s)** | **Purpose**                               | **Notes**                                                                                                                                                                                                           |
| ------------------------- | ------------- | ----------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| /api/productsList         | GET           | Returns the full list of products         | POST on this endpoint returns responseCode 405 in the JSON body. See Section 8.2 - HTTP transport status is always 200.                                                                                             |
| /api/brandsList           | GET           | Returns the full list of brands           | PUT on this endpoint is expected to behave the same way as productsList under POST - not independently re-verified.                                                                                                 |
| /api/searchProduct        | POST          | Searches products by keyword              | Requires search_product parameter (form-encoded, not JSON body - see Section 8.3). Matches against category metadata as well as name - see Section 8.4.                                                             |
| /api/verifyLogin          | POST          | Verifies a user's credentials             | Confirmed distinct messages for: success ("User exists"), missing parameters ("Bad request"), user not found ("User not found"). All returned with HTTP 200 regardless of outcome.                                  |
| /api/createAccount        | POST          | Creates a new user account                | Requires name, email, password, and full address/profile fields, sent as form-encoded parameters.                                                                                                                   |
| /api/deleteAccount        | DELETE        | Deletes an existing account               | Requires registered email + password sent specifically as multipart/form-data. Standard form-urlencoded parameters and query-string parameters were both confirmed NOT to work for this endpoint - see Section 8.3. |
| /api/updateAccount        | PUT           | Updates an existing account's details     | Requires same field set as createAccount. Encoding requirements not independently re-verified; recommend testing as multipart/form-data given deleteAccount's confirmed behaviour.                                  |
| /api/getUserDetailByEmail | GET           | Returns account details for a given email | Requires a registered email as parameter.                                                                                                                                                                           |

# 5\. Non-Functional Considerations (Practice Scope)

- Response time: API responses should return within an acceptable threshold (e.g. < 2s) for the test environment; treated as informational, not a hard SLA, since this is a third-party demo site.
- Reliability: as a public demo site, intermittent downtime or data resets are expected and are explicitly out of scope as defects.
- Browser compatibility: UI requirements are validated against Chrome only for this project, with Firefox as a stretch goal.
- Third-party content interference (NEW - see Section 8.5): the live site serves third-party advertisements that can intermittently overlap interactive elements. This is an environmental risk, not a defect in the application under test, but it directly affects UI test reliability and must be accounted for in automation design.

# 6\. Assumptions and Constraints

- The site's actual underlying business requirements are unknown; this document describes observed behaviour only.
- No access exists to a staging/test environment - all testing occurs against the live public site, so destructive operations (e.g. deleteAccount) are used carefully and with disposable, GUID-based test data.
- No formal acceptance criteria were issued by a product owner; acceptance criteria in the accompanying Test Plan are inferred from observed behaviour and standard e-commerce conventions, refined further by confirmed findings during test automation development.

# 7\. Traceability

Each requirement ID above (REQ-UI-xx, API endpoints) is referenced directly in the accompanying Test Plan's test case IDs, to demonstrate requirement-to-test traceability - a practice expected in professional QA documentation.

# 8\. Confirmed Findings From Test Automation Development

The following findings were discovered through hands-on test automation development and execution, not through documentation review alone. Each is distinguished from the originally inferred requirements above, consistent with good practice: requirements should be corrected when testing reveals the actual system behaviour differs from what was assumed.

## 8.1 No explicit empty-state message on search

**CONFIRMED BY TESTING:** Searching for a non-existent product term returns zero product results, but the page does not render any explicit "no results found" message or equivalent empty-state text. The original assumption in REQ-UI-09 (v1.0) - that a clear empty-state message would be displayed - does not hold. Verification must be performed by asserting on the absence of product elements, not the presence of a message.

## 8.2 API returns HTTP 200 regardless of actual outcome

**CONFIRMED BY TESTING:** Every API endpoint tested returns HTTP status 200 (OK) at the transport level, even for requests that fail business-rule validation (e.g. POST to /api/productsList, which is documented as unsupported). The actual outcome is communicated exclusively through a responseCode field inside the JSON response body (e.g. responseCode: 405 with message "This request method is not supported"). Test assertions against HTTP status codes alone are insufficient for this API; the response body must be inspected for the true result.

## 8.3 Inconsistent parameter encoding requirements across endpoints

**CONFIRMED BY TESTING:** Not all account-management endpoints accept parameters the same way. /api/createAccount and /api/searchProduct accept standard form-encoded (application/x-www-form-urlencoded) parameters. /api/deleteAccount specifically requires multipart/form-data encoding - form-encoded parameters and query-string parameters were both confirmed, through direct testing, to fail with "email parameter is missing" even when the email value was demonstrably present in the request. This was confirmed by comparing a working Postman request (sent as multipart/form-data) against the failing equivalent. /api/updateAccount has not been independently tested but should not be assumed to follow createAccount's encoding without verification, given this inconsistency.

## 8.4 Search matches on category metadata, not name alone

**CONFIRMED BY TESTING:** A search for "dress" returned at least one product whose displayed name did not contain the term "dress", but whose category metadata did. This confirms the search function matches against category data in addition to product name. This is treated as confirmed, intended site behaviour rather than a defect, but it means any test asserting "all search results contain the search term in their name" is testing an incorrect assumption about the search's actual matching logic. Assertions should account for matches against name OR category.

## 8.5 Third-party advertisement interference with UI interactions

**CONFIRMED BY TESTING:** Add-to-cart interactions on the Products page intermittently fail with ElementClickInterceptedException due to a third-party advertisement iframe (served via Google Ads) overlapping the target element at the click coordinates. This is environmental interference from ad content unrelated to the application under test, not a defect in the site itself. It was mitigated in automation by executing the click via JavaScript directly on the target element rather than relying on Selenium's native, coordinate-based click simulation.