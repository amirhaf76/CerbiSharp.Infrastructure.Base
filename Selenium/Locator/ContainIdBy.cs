using OpenQA.Selenium;


namespace CerbiSharp.Infrastructure.Base.Selenium.Locator
{
    public class ContainIdBy : By
    {
        public ContainIdBy(string testid)
        {
            string cssSelector = $"[data-testid*='{testid}']";

            By by = CssSelector($"[data-testid*='{testid}']");

            FindElementMethod = by.FindElement;

            FindElementsMethod = by.FindElements;
        }
    }
}

