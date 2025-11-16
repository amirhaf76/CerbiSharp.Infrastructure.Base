using OpenQA.Selenium;


namespace CerbiSharp.Infrastructure.Base.Selenium.Locator
{
    public class TestIdBy : By
    {
        public TestIdBy(string testid)
        {
            By by = CssSelector($"[data-testid='{testid}']");

            FindElementMethod = by.FindElement;

            FindElementsMethod = by.FindElements;
        }
    }
}

