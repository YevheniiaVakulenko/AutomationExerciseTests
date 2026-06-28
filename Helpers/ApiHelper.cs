using RestSharp;

namespace AutomationExerciseTests.Helpers
{
    public static class ApiHelper
    {
        private static readonly RestClient client = new RestClient("https://automationexercise.com");

        public static RestResponse CreateAccountViaApi(UserTestData user)
        {
            var request = new RestRequest("/api/createAccount", Method.Post);
            request.AddParameter("name", user.Name);
            request.AddParameter("email", user.Email);
            request.AddParameter("password", user.Password);
            request.AddParameter("title", user.Title);
            request.AddParameter("birth_date", user.Birth_day);
            request.AddParameter("birth_month", user.Birth_month);
            request.AddParameter("birth_year", user.Birth_year);
            request.AddParameter("firstname", user.FirstName);
            request.AddParameter("lastname", user.LastName);
            request.AddParameter("company", user.Company);
            request.AddParameter("address1", user.Address);
            request.AddParameter("country", user.Country);
            request.AddParameter("zipcode", user.Zipcode);
            request.AddParameter("state", user.State);
            request.AddParameter("city", user.City);
            request.AddParameter("mobile_number", user.MobileNumber);

            var response = client.Execute(request);

            if (!response.Content.Contains("User created!"))
            {
                throw new InvalidOperationException(
                    $"Failed to create test account via API. Response: {response.Content}");
            }
            return response;
        }

        public static string DeleteAccountViaApi(UserTestData user)
        {
            //RestSharp does not parse query parameters for DELETE /api/deleteAccounts
            using var httpClient = new HttpClient();
            var content = new MultipartFormDataContent
            {
                { new StringContent(user.Email), "email" },
                { new StringContent(user.Password), "password" }
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, "https://automationexercise.com/api/deleteAccount")
            {
                Content = content
            };

            var response = httpClient.Send(request);
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
