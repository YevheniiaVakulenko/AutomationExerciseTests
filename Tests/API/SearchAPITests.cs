using AutomationExerciseTests.Helpers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutomationExerciseTests.Tests.API
{
    public class SearchAPITests: BaseAPITest
    {
        // TC-15
        [Test]
        public void SearchProduct_WithValidTerm_ShouldReturnMatches()
        {
            var request = new RestRequest("/api/searchProduct", Method.Post);
            request.AddParameter("search_product", "dress");

            var response = client.Execute<ProductsResponse>(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.products, Has.All.Matches<Product>(p =>p.name.ToLower().Contains("dress") || p.category.category.ToLower().Contains("dress")));
        }

        // TC-16
        [Test]
        public void SearchProduct_MissingParameter_ShouldReturnError()
        {
            var request = new RestRequest("/api/searchProduct", Method.Post);

            var response = client.Execute(request);

            Assert.That(response.Content, Does.Contain("Bad request"));
        }
    }
}
