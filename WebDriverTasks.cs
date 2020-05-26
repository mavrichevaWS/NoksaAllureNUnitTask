using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

using Allure.Commons;
using Allure.NUnit.Attributes;
using Allure.Commons.Model;

namespace WebDriverTasks
{
    [TestFixture]
    [AllureSuite("Web driver tasks")]
    public class WebDriverTasks : AllureReport
    {
        IWebDriver driver = new ChromeDriver(@"C:\Users\user\source\repos\Selenium WebDriver Tasks\Selenium WebDriver Tasks\bin\Debug");

        public void TestInitialize()
        {
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);
        }


        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Set up has started");
            AllureLifecycle.Instance.SetCurrentTestActionInException(() =>
            {
                AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                driver.TakeScreenshot().AsByteArray);
            });
        }

        // Method for making screenshot for falling tests
        [TearDown]
        public void TeardownTest()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status.Equals(TestStatus.Failed))
            {
                if (TestContext.CurrentContext.Result.Outcome.Label == "Error")
                {
                    Console.WriteLine("Test is in Error");
                    AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                    driver.TakeScreenshot().AsByteArray);
                }
                else
                {
                    Console.WriteLine("Test Failed");
                    AllureLifecycle.Instance.AddAttachment("Step Screenshot", AllureLifecycle.AttachFormat.ImagePng,
                    driver.TakeScreenshot().AsByteArray);
                }
            }
        }

        // Login test
        [AllureSubSuite("Login check test")]
        [AllureSeverity(SeverityLevel.Critical)]
        [AllureLink("Some link2")]
        [AllureTest("Login check test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("22222")]
        [AllureTms("2331")]
        [TestCase("seleniumtests@tut.by", "123456789zxcvbn")]
        [TestCase("seleniumtests2@tut.by", "123456789zxcvbn")]
        public void NewEmailTest(string login, string pw)
        {
            string isItAuthorized = "Selenium Test";

            driver.Navigate().GoToUrl(UserConstantData.URL);

            // Implicit wait
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            // Thread sleep
            Thread.Sleep(5000); // This is Implicit wait because the code will be fulfilled only once when DOM is loading.

            // Make the screenshot
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(System.IO.Path.Combine("C:\\Users\\user\\source\\repos\\Selenium WebDriver Tasks\\Selenium WebDriver Tasks\\assets", "tut_by.png"), ScreenshotImageFormat.Png);

            IWebElement searchEnterButton = driver.FindElement(By.CssSelector("a.enter"));
            searchEnterButton.Click();

            IWebElement searchLoginField = driver.FindElement(By.CssSelector("input[type='text']"));
            searchLoginField.SendKeys(login);

            IWebElement searchPwField = driver.FindElement(By.CssSelector("input[type='password']"));
            searchPwField.SendKeys(pw);

            IWebElement searchApplyButton = driver.FindElement(By.CssSelector("input.button.m-green.auth__enter"));
            searchApplyButton.Click();

            // Explicit wait
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            IWebElement findProfileName = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("span.uname")));
            string profileName = findProfileName.Text;
            AllureLifecycle.Instance.RunStep("Login test", () => { Assert.IsTrue(profileName == isItAuthorized); });
        }

        // Options select test
        [TestCase(TestName = "Options select test")]
        [AllureSubSuite("Checking the required of selection values")]
        [AllureSeverity(SeverityLevel.Critical)]
        [AllureLink("Some link")]
        [AllureTest("Options select test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("11111")]
        [AllureTms("2651")]
        public void MultiselectTest()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL2);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            string state1 = "California";
            string state2 = "Miami";
            string state3 = "Pennsylvania";

            IList<IWebElement> elements = driver.FindElements(By.CssSelector("select[name='States'] > option:nth-child(3n+1)"));
            foreach (IWebElement e in elements)
            {
                e.Click();
                AllureLifecycle.Instance.RunStep("Options select", () =>
                {
                    if (e.Selected)
                    {
                        Assert.IsTrue(e.Text == state1 || e.Text == state2 || e.Text == state3);
                        Console.WriteLine("Value of the option item is selected: " + e.Selected + " " + e.Text);
                    }
                });
            }
        }

        // Confirm box test
        [TestCase(TestName = "Confirm box test     ")]
        [AllureSubSuite("Checking of confirm box")]
        [AllureSeverity(SeverityLevel.Critical)]
        [AllureLink("Some link3")]
        [AllureTest("Confirm box test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("33333")]
        [AllureTms("1254")]
        public void ConfirmBox()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL3);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IWebElement searchConfirmButton = driver.FindElement(By.CssSelector("button[onclick='myConfirmFunction()']"));
            searchConfirmButton.Click();

            var confirm = driver.SwitchTo().Alert();
            confirm.Accept();

            AllureLifecycle.Instance.RunStep("Confirm box", () =>
            {
                IWebElement clickResult = driver.FindElement(By.Id("confirm-demo"));
                Console.WriteLine(clickResult.Text);
                if (clickResult.Text == "You pressed OK!") Console.WriteLine("Confirm test successful");
            });
        }

        // Prompt box test
        [TestCase(TestName = "Prompt box test")]
        [AllureSubSuite("Checking of prompt box")]
        [AllureSeverity(SeverityLevel.Normal)]
        [AllureLink("Some link4")]
        [AllureTest("Prompt box test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("44444")]
        [AllureTms("1133")]
        public void PromptBox()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL3);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IWebElement searchPromptButton = driver.FindElement(By.CssSelector("button[onclick='myPromptFunction()']"));
            searchPromptButton.Click();

            var prompt = driver.SwitchTo().Alert();
            prompt.SendKeys("This is a test prompt message");
            prompt.Accept();

            AllureLifecycle.Instance.RunStep("Prompt box", () =>
            {
                IWebElement clickResult = driver.FindElement(By.CssSelector("#prompt-demo"));
                Console.WriteLine(clickResult.Text);

                if (clickResult.Text == "You have entered 'This is a test prompt message' !") Console.WriteLine("Prompt test successful");
            });
        }

        // Alert box test
        [TestCase(TestName = "Alert box test")]
        [AllureSubSuite("Checking of alert box")]
        [AllureSeverity(SeverityLevel.Normal)]
        [AllureLink("Some link5")]
        [AllureTest("Alert box test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("55555")]
        [AllureTms("1025")]
        public void AlertBox()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL3);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IWebElement searchAlertButton = driver.FindElement(By.CssSelector("button[onclick='myAlertFunction()']"));
            searchAlertButton.Click();

            var expectedAlertText = "I am an alert box!";
            var alert = driver.SwitchTo().Alert();
            AllureLifecycle.Instance.RunStep("Alert box", () =>
            {
                Assert.AreEqual(expectedAlertText, alert.Text);
                if (alert.Text == expectedAlertText) Console.WriteLine("Alert test successful");
                alert.Accept();
            });
        }

        // User wait test
        [TestCase(TestName = "User wait test")]
        [AllureSubSuite("Waiting for user logo loading")]
        [AllureSeverity(SeverityLevel.Minor)]
        [AllureLink("Some link6")]
        [AllureTest("User wait test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("66666")]
        [AllureTms("8732")]
        public void UserWaiter()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL4);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IWebElement searchNewUserButton = driver.FindElement(By.CssSelector("button#save"));
            searchNewUserButton.Click();

            AllureLifecycle.Instance.RunStep("Logo loading", () =>
            {
                WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
                IWebElement foto = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("img[src*='randomuser']")));
                bool image = foto.Displayed && foto.Enabled;
                if (image) Console.WriteLine("Test successful");
            });
        }

        // Waiting 50% test
        [TestCase(TestName = "Waiting 50% test")]
        [AllureSubSuite("Waiting for 50% for browser refreshing")]
        [AllureSeverity(SeverityLevel.Blocker)]
        [AllureLink("Some link7")]
        [AllureTest("Waiting 50% test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("77777")]
        [AllureTms("4673")]
        public void downloadingPercentage()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL5);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IWebElement searchDownloadButton = driver.FindElement(By.CssSelector("button#cricle-btn"));
            searchDownloadButton.Click();

            Thread.Sleep(10300);
            IWebElement percent = driver.FindElement(By.CssSelector("div.percenttext"));
            string percentage = percent.Text;
            string value = percentage.Remove(percentage.Length - 1, 1);
            AllureLifecycle.Instance.RunStep("Waiting 50%", () =>
            {
                if (Int32.Parse(value) >= 50)
                {
                    driver.Navigate().Refresh();
                    Console.WriteLine(Int32.Parse(value));
                    Console.WriteLine("Test successful");
                }
            });
        }

        // Select table data test
        [TestCase(TestName = "Select table data test")]
        [AllureSubSuite("Select some interesting rows from table")]
        [AllureSeverity(SeverityLevel.Minor)]
        [AllureLink("Some link8")]
        [AllureTest("Select table data test")]
        [AllureOwner("Elena Mavricheva")]
        [AllureIssue("88888")]
        [AllureTms("9823")]
        public void selectionOfResults()
        {
            driver.Navigate().GoToUrl(UserConstantData.URL6);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(5000);

            IList<IWebElement> elements = driver.FindElements(By.CssSelector("span > a.paginate_button"));
            int value = elements.Count;

            for (int i = 1; i <= value; i++)
            {
                if (driver.FindElement(By.CssSelector("a#example_next")).Enabled == true)
                {
                    IWebElement table = driver.FindElement(By.Id("example"));

                    var columns = table.FindElements(By.TagName("th"));
                    var rows = table.FindElements(By.TagName("tr"));

                    int rowIndex = 1;
                    List<object> _tableDataColections2 = new List<object>();

                    foreach (var row in rows)
                    {
                        int colIndex = 0;
                        var colDatas = row.FindElements(By.TagName("td"));
                        List<TableDataColection> _tableDataColections = new List<TableDataColection>();

                        AllureLifecycle.Instance.RunStep("Select table data", () => 
                        { 
                            foreach (var colValue in colDatas)
                            {
                                _tableDataColections.Add(new TableDataColection
                                {
                                    ColumnName = columns[colIndex].Text,
                                    ColumnValue = colValue.Text
                                });
                                colIndex++;
                            }
                        });

                        if (rows.IndexOf(row) == rows.Count - 1)
                        {
                            IWebElement searchPaginateButtonElem = driver.FindElement(By.CssSelector("a#example_next"));
                            searchPaginateButtonElem.Click();
                        }

                        _tableDataColections2.Add(_tableDataColections);

                        rowIndex++;
                    }

                    var data = from rowData in _tableDataColections2 select rowData;

                    foreach (var tableData in data)
                    {
                        /* if (tableData.ColumnName == "Age") { Console.WriteLine(tableData.ColumnName + " " + tableData.Age); }
                        else if (tableData.ColumnName == "Salary") { Console.WriteLine(tableData.ColumnName + " " + tableData.Salary); }
                        else { Console.WriteLine(tableData.ColumnName + " " + tableData.ColumnValue); } */
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void TestCleanUp()
        {
            driver.Close();
        }
    }
}
