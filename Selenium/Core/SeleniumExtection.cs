using CerbiSharp.Infrastructure.Base.JavaScript;
using CerbiSharp.Infrastructure.Base.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CerbiSharp.Infrastructure.Base.Selenium.Core
{
    public static class SeleniumExtection
    {

        public static void DragElementAndRelease(this Actions actions, IWebElement element, int xOffset, int yOffset, int pauseForSomeSecond = 1)
        {
            actions.
                ClickAndHold(element).
                MoveToElement(element, xOffset, yOffset).
                Pause(TimeSpan.FromSeconds(pauseForSomeSecond)).
                Release().
                Perform();
        }

        public static void RemoveHideElement(this IWebElement element, IWebDriver webDriver)
        {
            var scriptExecutor = (IJavaScriptExecutor)webDriver;

            if (element != null)
            {
                scriptExecutor.ExecuteScript("arguments[0].style.display = 'none'", element);
            }
        }


        /// <returns>Original window handle Id</returns>
        public static string SwitchTab(this IWebDriver webDriver)
        {
            string originalWindow = webDriver.CurrentWindowHandle;

            foreach (string window in webDriver.WindowHandles)
            {
                if (originalWindow != window)
                {
                    webDriver.SwitchTo().Window(window);
                    break;
                }
            }

            return originalWindow;
        }

        public static void ScrollIntoView(this IWebDriver webDriver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;

            js.ExecuteScript(JavaScriptSeleniumCommands.SCORLL_INTO_VIEW, element);
        }

        public static void ScrollIntoView(this IWebDriver driver, int x, int y)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript(JavaScriptSeleniumCommands.SCORLL_INTO_VIEW, x, y);
        }

        public static void SelectAllAndOverWrite(this Actions actions, IWebElement inputElement, string value)
        {
            actions.
                Click(inputElement).
                PrepareSelectAllActions().
                SendKeys(value).
                SendKeys(Keys.Escape).
                Perform();
        }

        private static Actions PrepareSelectAllActions(this Actions actions)
        {
            var letterAKey = "a";

            return
                actions.
                KeyDown(Keys.Control).
                SendKeys(letterAKey).
                KeyUp(Keys.Control);
        }

        public static decimal ExtractNumberAndConvert(this IWebElement webElement)
        {
            return decimal.Parse(webElement.Text.ExtractNumberAsMatch().Value);
        }

        public static int ExtractIntegerAndConvert(this IWebElement webElement)
        {
            return int.Parse(webElement.Text.ExtractNumberAsMatch().Value);
        }

        public static double ExtractDoubleAndConvert(this IWebElement webElement)
        {
            return double.Parse(webElement.Text.ExtractNumberAsMatch().Value);
        }

        /// <summary>
        /// This method is just use for checkbox Ant in react.
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static bool IsChecked(this IWebElement webElement, IWebDriver webDrvier)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDrvier;

            var valueTracker = js.ExecuteScript(JavaScriptSeleniumCommands.RETURN_VALUE_TRACKER, webElement);

            if (valueTracker != null)
            {
                var check = js.ExecuteScript(JavaScriptSeleniumCommands.CALL_AND_RETURN_VALUE_TRACKER_VALUE, webElement) as string;

                bool isSuccessful = bool.TryParse(check, out bool res);

                if (isSuccessful) return res;
            }
            else
            {
                throw new Exception("There is no value track for element.");
            }

            throw new Exception("Converting value result to boolean was unsuccessful.");
        }


        // ======= 


        private const int DefaultTryingSearchingTimeInSecond = 5;
        private static readonly CultureInfo _irCultureInfo = CultureInfo.CreateSpecificCulture("fa-ir");
        private const string _irDateFormattedString = "yyyy/MM/dd";

        private static readonly Dictionary<TimeUnit, string> s_timeUnitTranslationMap = new Dictionary<TimeUnit, string>
        {
            { TimeUnit.Second, "ثانیه" },
            { TimeUnit.Minute, "دقیقه" },
            { TimeUnit.Hour, "ساعت" },
            { TimeUnit.Day, "روز" },
            { TimeUnit.Week, "هفته" },
            { TimeUnit.Month, "ماه" },
            { TimeUnit.Year, "سال" },

        };

        private static DateTime GetTimeFromDistance(int time, TimeUnit timeUnit)
        {
            if (time < 0)
            {
                throw new Exception("Time can not be negative in GetDistanceTime");
            }

            const int timeAgo = -1;
            const int weekAgo = -7;

            switch (timeUnit)
            {
                case TimeUnit.Second:
                    return DateTime.Now.AddSeconds(timeAgo * time);
                case TimeUnit.Minute:
                    return DateTime.Now.AddMinutes(timeAgo * time);
                case TimeUnit.Hour:
                    return DateTime.Now.AddHours(timeAgo * time);
                case TimeUnit.Day:
                    return DateTime.Now.AddDays(timeAgo * time);
                case TimeUnit.Week:
                    return DateTime.Now.AddDays(weekAgo * time);
                case TimeUnit.Month:
                    return DateTime.Now.AddMonths(timeAgo * time);
                case TimeUnit.Year:
                    return DateTime.Now.AddYears(timeAgo * time);
                default:
                    throw new Exception("There is problem in GetDistanceTime.");
            }
        }

        private static IWebElement FindElementOrDefault(By by, ISearchContext searchContext, int tryingSearchingTimeInSecond)
        {
            IWebElement webElement = null;
            try
            {
                webElement = new SearchContextWait(searchContext, TimeSpan.FromSeconds(tryingSearchingTimeInSecond)).Until(e => e.FindElement(by));
            }
            catch (WebDriverTimeoutException)
            {

            }
            return webElement;
        }

        private static IWebElement FindElementUndoubtedly(By by, ISearchContext SearchContext, int tryingSearchingTimeInSecond)
        {
            IWebElement webElement = null;

            // Throw WebDriverTimeoutException if times out.
            webElement = new SearchContextWait(SearchContext, TimeSpan.FromSeconds(tryingSearchingTimeInSecond)).Until(e => e.FindElement(by));

            return webElement;
        }

        public static IWebElement FindElementByStrategy(this ISearchContext searchContext, By by, SearchingStrategy ss, int tryingSearchingTimeInSecond)
        {
            switch (ss)
            {
                case SearchingStrategy.CanBeNull:
                    return FindElementOrDefault(by, searchContext, tryingSearchingTimeInSecond);

                case SearchingStrategy.CanNotBeNull:
                    return FindElementUndoubtedly(by, searchContext, tryingSearchingTimeInSecond);

                default:
                    throw new ArgumentException($"{nameof(ss)} argument value is not correct.");
            }
        }

        public static IWebElement FindElementByStrategy(this ISearchContext searchContext, By by, SearchingStrategy ss)
        {
            return searchContext.FindElementByStrategy(by, ss, DefaultTryingSearchingTimeInSecond);
        }

        public static IWebElement FindElementByStrategy(this ISearchContext SearchContext, By by)
        {
            return SearchContext.FindElementByStrategy(by, SearchingStrategy.CanBeNull);
        }

        public static ReadOnlyCollection<IWebElement> PgFindElements(this ISearchContext SearchContext, By by, int timeSpan = 100)
        {
            bool result = false;
            int counter = 0;

            ReadOnlyCollection<IWebElement> foundElements = null;

            while (!result)
            {
                try
                {
                    foundElements = SearchContext.FindElements(by);

                    result = true;
                }
                //catch (StaleElementReferenceException)
                //{                    

                //    counter++;
                //}
                catch (Exception)
                {

                    counter++;
                    if (counter == timeSpan)
                    {
                        return null;
                    }
                }
            }
            if (result)
            {
                return foundElements;
            }
            else
            {
                throw new WebDriverException("elementi peida nashod");
            }

        }



        public static bool CheckNullableProperty<T>(T obj1, T obj2)
        {
            if (obj1 == null && obj2 == null)
                return true;

            if (obj1 != null && obj2 != null && obj1.Equals(obj2))
                return true;

            return false;
        }

        /// <summary>
        /// Input must be contain two numbers, and parsing reads from left to right.
        /// <code>GetInterval("0 - 43")</code> returns (0, 43)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>leftSide and rightSide</returns>
        /// <exception cref="ArgumentException"></exception>
        public static (decimal, decimal) GetInterval(this string input)
        {

            MatchCollection intervalMatches = input.ExtractNumbers();

            if (intervalMatches.Count != 2)
            {
                throw new ArgumentException($"{nameof(input)} must be contain two numbers.");
            }

            decimal leftSide = decimal.Parse(intervalMatches[0].Value);
            decimal rightSide = decimal.Parse(intervalMatches[1].Value);

            return (leftSide, rightSide);
        }


        [Flags]
        public enum SearchingStrategy
        {
            CanBeNull,
            CanNotBeNull
        }

        public enum TimeUnit
        {
            Second,
            Minute,
            Hour,
            Day,
            Week,
            Month,
            Year
        }

        /// <summary>
        /// This method is just use for checkbox Ant in react.
        /// </summary>
        /// <param name="webElement"></param>
        /// <returns></returns>
        public static bool IsChecked(this IWebElement webElement)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)webElement;

            var valueTracker = js.ExecuteScript("return arguments[0]._valueTracker", webElement);

            if (valueTracker != null)
            {
                var check = js.ExecuteScript("return arguments[0]._valueTracker.getValue();", webElement) as string;

                bool isSuccessful = bool.TryParse(check, out bool res);

                if (isSuccessful) return res;
            }
            else
            {
                throw new Exception("There is no value track for element.");
            }

            throw new Exception("Converting value result to boolean was unsuccessful.");
        }

        public static string GetIRDateTimeFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString(_irDateFormattedString, _irCultureInfo);
        }

        public static DateTime GetIRDateTimeFormattedString(this string dateAndTime)
        {
            return DateTime.Parse(dateAndTime, _irCultureInfo);
        }


        public static DateTime GetTimeDistance(this IWebElement webElement)
        {
            return webElement.Text.GetTimeDistance();
        }

        public static DateTime GetTimeDistance(this string text)
        {
            Array timeUnits = Enum.GetValues(typeof(TimeUnit));

            TimeUnit? textTimeUnit = null;

            foreach (var timeUnit in timeUnits)
            {
                var selcetedTimeUnit = (TimeUnit)timeUnit;

                if (text.Contains(s_timeUnitTranslationMap[selcetedTimeUnit]))
                {
                    if (textTimeUnit != null)
                    {
                        throw new ArgumentException($"There is more than one time unit in \"{text}\" phrase!");
                    }

                    textTimeUnit = selcetedTimeUnit;
                }
            }

            if (textTimeUnit == null)
            {
                throw new ArgumentException($"There is not any time unit in \"{text}\" phrase!");
            }

            // For second, instead of saying how many second pass from events,
            // it uses some "some moment before".
            if (textTimeUnit == TimeUnit.Second)
            {
                return GetTimeFromDistance(59, TimeUnit.Second);
            }

            Match timeUnitValue = Regex.Match(text, @"\d+");

            var eventTime = GetTimeFromDistance(int.Parse(timeUnitValue.Value), textTimeUnit ?? TimeUnit.Day);

            return eventTime;
        }
    }
}
