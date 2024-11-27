using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Globalization;

namespace TestExercise
{
    public class SauceDemoTest
    {
        private ChromeDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void AddMostExpensiveItemToCart()
        {
            try
            {
                driver.FindElement(By.Id("user-name")).SendKeys("standard_user");
                driver.FindElement(By.Id("password")).SendKeys("secret_sauce");
                driver.FindElement(By.Id("login-button")).Click();

                var items = driver.FindElements(By.ClassName("inventory_item"));

                if (items.Count == 0)
                {
                    Assert.Fail("No items found on the page.");
                }

                double maxPrice = 0;
                IWebElement mostExpensiveItemButton = null;

                foreach (var item in items)
                {
                    var priceText = item.FindElement(By.ClassName("inventory_item_price")).Text.Replace("$", "");
                    var price = Convert.ToDouble(priceText, CultureInfo.InvariantCulture);

                    if (price > maxPrice)
                    {
                        maxPrice = price;
                        mostExpensiveItemButton = item.FindElement(By.TagName("button"));
                    }
                }

                if (mostExpensiveItemButton != null)
                {
                    mostExpensiveItemButton.Click();
                }
                else
                {
                    Assert.Fail("No items found or could not determine the most expensive item.");
                }

                Assert.That(driver.FindElement(By.ClassName("shopping_cart_badge")).Text, Is.EqualTo("1"));
            }
            catch (NoSuchElementException ex)
            {
                Assert.Fail("Test failed due to element not being found.");
            }
            catch (Exception ex)
            {
                Assert.Fail("Test failed due to an unexpected error.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}