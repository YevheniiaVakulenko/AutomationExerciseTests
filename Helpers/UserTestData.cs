namespace AutomationExerciseTests.Helpers
{
    public class UserTestData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Birth_day { get; set; }
        public string Birth_month { get; set; }
        public string Birth_year { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string MobileNumber { get; set; }

        public static UserTestData GenerateRandomUser()
        {
            string uniqueId = Guid.NewGuid().ToString();
            return new UserTestData
            {
                Name = "QA Tester",
                Email = $"qa_test_{uniqueId}@example.com",
                Password = "TempPass123!",
                Title = "Mr",
                Birth_day = "1",
                Birth_month = "1",
                Birth_year = "1990",
                FirstName = "QA",
                LastName = "Tester",
                Company = "Portfolio Project",
                Address = "123 Test Street",
                Country = "United States",
                State = "Cambridgeshire",
                City = "Cambridge",
                Zipcode = "AB1 2CD",
                MobileNumber = "01234567890"
            };
        }
    }
}
