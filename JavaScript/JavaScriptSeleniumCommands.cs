namespace CerbiSharp.Infrastructure.Base.JavaScript
{
    public class JavaScriptSeleniumCommands
    {
        public const string SCORLL_INTO_VIEW = "arguments[0].scrollIntoView(false)";
        public const string SCROLL_X_Y = "window.scrollBy(arguments[0], arguments[1])";
        public const string RETURN_VALUE_TRACKER = "return arguments[0]._valueTracker";
        public const string CALL_AND_RETURN_VALUE_TRACKER_VALUE = "return arguments[0]._valueTracker.getValue()";
    }
}
