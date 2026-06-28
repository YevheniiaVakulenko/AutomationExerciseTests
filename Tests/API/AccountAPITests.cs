using AutomationExerciseTests.Helpers;
using RestSharp;
using System.Net;

namespace AutomationExerciseTests.Tests.API
{
    public class AccountAPITests : BaseAPITest
    {
        // TC-17
        [Test]
        public void VerifyLogin_WithValidCredentials_ShouldReturnSuccess()
        {
            var user = UserTestData.GenerateRandomUser();
            ApiHelper.CreateAccountViaApi(user);

            var request = new RestRequest("/api/verifyLogin", Method.Post);
            request.AddParameter("email", user.Email);
            request.AddParameter("password", user.Password);

            var response = client.Execute(request);

            Assert.That(response.Content, Does.Contain("User exists"));

            ApiHelper.DeleteAccountViaApi(user);
        }

        // TC-18
        [Test]
        public void VerifyLogin_MissingParameters_ShouldReturnError()
        {
            var request = new RestRequest("/api/verifyLogin", Method.Post);

            var response = client.Execute(request);

            Assert.That(response.Content, Does.Contain("Bad request"));
        }

        // TC-19
        [Test]
        public void VerifyLogin_NonExistentUser_ShouldReturnUserNotFound()
        {
            var request = new RestRequest("/api/verifyLogin", Method.Post);
            request.AddParameter("email", $"qa_nonexistent_{System.Guid.NewGuid()}@example.com");
            request.AddParameter("password", "whatever123");

            var response = client.Execute(request);

            Assert.That(response.Content, Does.Contain("User not found"));
        }

        // TC-20
        [Test]
        public void CreateAccount_ThenDeleteAccount_ShouldSucceedBoth()
        {
            var user = UserTestData.GenerateRandomUser();

            var createResponse = ApiHelper.CreateAccountViaApi(user);
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(createResponse.Content, Does.Contain("User created!"));

            var deleteResponse = ApiHelper.DeleteAccountViaApi(user);
            Assert.That(deleteResponse, Does.Contain("Account deleted!"));
        }
    }
}
