using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CerbiSharp.Infrastructure.Base.Selenium.Core
{
    public class SearchContextWait : DefaultWait<ISearchContext>
    {
        private readonly static TimeSpan _defaultPollingInterval = TimeSpan.FromMilliseconds(500.0);

        public SearchContextWait(ISearchContext searchContext, TimeSpan timeOut) : this(searchContext, timeOut, _defaultPollingInterval)
        {

        }

        public SearchContextWait(ISearchContext searchContext, TimeSpan timeOut, TimeSpan pollingInterval) : base(searchContext, new SystemClock())
        {
            Timeout = timeOut;
            PollingInterval = pollingInterval;
            IgnoreExceptionTypes(typeof(NotFoundException));
        }
    }
}
