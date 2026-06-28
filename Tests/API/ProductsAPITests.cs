using AutomationExerciseTests.Helpers;
using RestSharp;
using System.Net;

namespace AutomationExerciseTests.Tests.API
{
    public class ProductsAPITests: BaseAPITest
    {
        // TC-12
        [Test]
        public void GetProductsList_ShouldReturn200_AndNonEmptyList()
        {
            var request = new RestRequest("/api/productsList", Method.Get);
            var response = client.Execute<ProductsResponse>(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.products, Is.Not.Empty);
        }

        // TC-13
        [Test]
        public void PostProductsList_ShouldReturn405_MethodNotAllowed()
        {
            var request = new RestRequest("/api/productsList", Method.Post);
            var response = client.Execute(request);
            Assert.That(response.Content, Does.Contain("\"responseCode\": 405"));
            Assert.That(response.Content, Does.Contain("This request method is not supported"));
        }

        // TC-14
        [Test]
        public void GetBrandsList_ShouldReturn200_AndNonEmptyList()
        {
            var request = new RestRequest("/api/brandsList", Method.Get);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("\"brands\""));
        }
    }
}
