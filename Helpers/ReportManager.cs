//using AventStack.ExtentReports;
//using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
namespace AutomationExerciseTests.Helpers
{
    public static class ReportManager
    {
        private static ExtentReports extent;
        private static ExtentTest currentTest;

        public static ExtentTest CurrentTest => currentTest;

        public static void InitializeReport()
        {
            if (extent == null)
            {
                var spark = new ExtentSparkReporter("TestResultReport.html");
                extent = new ExtentReports();
                extent.AttachReporter(spark);
            }
        }

        public static void StartTest(string testName)
        {
            currentTest = extent.CreateTest(testName);
        }

        public static void LogPass(string message)
        {
            currentTest.Pass(message);
        }

        public static void LogFail(string message)
        {
            currentTest.Fail(message);
        }

        public static void FlushReport()
        {
            extent.Flush();
        }
    }
}