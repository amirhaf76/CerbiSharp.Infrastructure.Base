using CerbiSharp.Infrastructure.Base.Selenium.Core;
using CerbiSharp.Infrastructure.Base.Tools;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace CerbiSharp.Infrastructure.Base.Selenium.Browsers
{
    public class Browser
    {
        private readonly BrowserType _browser;
        private WebDriver _webDriver;
        private WebDriverWait _webDriverWait;


        public string BrowserDirectory { get; set; }

        public IWebDriver WebDriver => _webDriver;

        public BrowserType Type => _browser;

        public Actions Actions { get; private set; }

        public WebDriverWait WebDriverWait
        {
            get
            {
                return _webDriverWait;
            }
        }


        public Browser(BrowserType browserType)
        {
            _browser = browserType;
            BrowserDirectory = Environment.CurrentDirectory;
        }


        public void Init()
        {
            switch (_browser)
            {
                case BrowserType.Chrome:
                    new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

                    ChromeOptions options = new ChromeOptions();


                    options.AddArgument("no-sandbox");
                    options.AddArgument("--disable-web-security");

                    TimeSpan commandTimeOut = TimeSpan.FromMinutes(15);
                    _webDriver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, commandTimeOut);

                    break;
                case BrowserType.Firefox:
                    var fireFoxOpetion = new FirefoxOptions();

                    fireFoxOpetion.AddArgument("no-sandbox");
                    fireFoxOpetion.AddArgument("--disable-web-security");

                    //new DriverManager().SetUpDriver(new FirefoxConfig(), VersionResolveStrategy.MatchingBrowser);
                    _webDriver = new FirefoxDriver();

                    break;
            }

            _webDriver.Manage().Window.Maximize();

            _webDriverWait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(30));

            Actions = new Actions(_webDriver);

            CreateBrowserDirectory(BrowserDirectory);
        }

        public void Init(string url)
        {
            Init();

            Goto(url);
        }

        public string GetTitle()
        {
            return _webDriver.Title;
        }

        public static void WaitForSomeSeconds(int during = 1000)
        {
            Thread.Sleep(during);
        }

        public void Goto(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
        }

        public void DragElementAndRelease(IWebElement element, int xOffset, int yOffset, int pauseForSomeSecond = 1)
        {
            Actions.DragElementAndRelease(element, xOffset, yOffset, pauseForSomeSecond);
        }

        public void EnterWithoutLogin()
        {
            // !!! Notice: These parameters need to be set manually. 

            var js = (IJavaScriptExecutor)_webDriver;

            string cookie = "%7B%22signalRToken%22%3A%7B%22token%22%3A%22IgrpiYhGX0Th3l7AOL7qgUjZwfTkkXkt4dXAhh9tPp2pbL50CxZVeBuu5UOu%22%2C%22date%22%3A1656765082972%7D%7D";
            _webDriver.Manage().Cookies.AddCookie(new Cookie("AUTHENTICATION", cookie));

            string key = "oidc.user:web.hitobit.com:6d6d0bec-c4b9-4deb-abdd-9c4b9cf01ef8";
            string value = "{\"id_token\":\"1656755455\",\"session_state\":\"\",\"access_token\":\"eyJhbGciOiJSUzI1NiIsImtpZCI6IjIyNDEwODk4RURBMUQzQkQwRTU1OUM0MjQ0NTk1MkJDNTU1OUVDOUNSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6IklrRUltTzJoMDcwT1ZaeENSRmxTdkZWWjdKdyJ9.eyJuYmYiOjE2NTY3NTU0NTUsImV4cCI6MTY1Njc2NzQ1NSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzYwMCIsImF1ZCI6WyJjbGllbnQiLCJub3RpZiJdLCJjbGllbnRfaWQiOiI2ZDZkMGJlYy1jNGI5LTRkZWItYWJkZC05YzRiOWNmMDFlZjgiLCJEb21haW5JZCI6IjIiLCJzdWIiOiJkNzgwZWRlYi05YWEyLTRhMjUtMmZhMy0wOGRhMDNmNjUxYmEiLCJhdXRoX3RpbWUiOjE2NTY3NDM1MTEsImlkcCI6ImxvY2FsIiwiZGV2aWNlSWQiOiJhNmUzOGViMy1kOGM0LTRhODUtYjdkZC1kODBmMTY3MDRmZTEiLCJNRkEiOiJUcnVlIiwicm9sZSI6IlVzZXIiLCJqdGkiOiIyMDlBNEI3ODRCMDQyNjcyRDhDQTg5M0M2RTlCOUQ1MCIsImlhdCI6MTY1Njc1NTQ1NSwic2NvcGUiOlsiY2xpZW50IiwiY2xpZW50LnNlY3JldCIsImNsaWVudC5zZXJ2aWNlIiwibm90aWYiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsibWZhIl19.tPb4S0mbY7dGE9I3ed9dLXS4H2MwDNKovAawlPVhvmzHQZQwKQKPv7cN5ZxgQSKcXEBAjHj7WZcVwXRlFrK - UHSKEN9J0pqTLcrUuEH80gR - P8KLtxMB6X3e6PpGv_xg2xcHttq_5E5HCRR4HKSIcGkR7m6mJvLdCkRP9_SX_62HyGCeIxF5G - C1sY37mlDugVyxVH6Osr_whLqszIMdPkM84OFLrX5pBdLfmMtcNVELdi8k3UDPWce8XxxXxa7SL14JqJ3gNIbeFrXRxy9ljEPlZO8odlNX0rT_dgxgbJvTa92mb6TNjvrP6bluTkB0RPhlhOmW62VkpoPYOhzPXw\",\"refresh_token\":\"D0C2B9DE8CC3C09BC63F3306BDD3F34FCC8FF803D5440F09DFB855872F565113\",\"token_type\":\"Bearer\",\"scope\":\"client client.secret client.service notif offline_access\",\"profile\":{\"nbf\":1656755455,\"exp\":1656767455,\"iss\":\"https://localhost:7600\",\"aud\":[\"client\",\"notif\"],\"client_id\":\"6d6d0bec-c4b9-4deb-abdd-9c4b9cf01ef8\",\"DomainId\":\"2\",\"sub\":\"d780edeb-9aa2-4a25-2fa3-08da03f651ba\",\"auth_time\":1656743511,\"idp\":\"local\",\"deviceId\":\"a6e38eb3-d8c4-4a85-b7dd-d80f16704fe1\",\"MFA\":\"True\",\"role\":\"User\",\"jti\":\"209A4B784B042672D8CA893C6E9B9D50\",\"iat\":1656755455,\"scope\":[\"client\",\"client.secret\",\"client.service\",\"notif\",\"offline_access\"],\"amr\":[\"mfa\"]},\"expires_at\":1656767455}";

            js.ExecuteScript("localStorage.setItem(arguments[0], arguments[1])", key, value);

            key = "AUTHENTICATION";
            value = "{\"businessUserId\":null,\"workSpaceType\":null}";
            js.ExecuteScript("localStorage.setItem(arguments[0], arguments[1])", key, value);

            key = "customUniqData";
            value = "a6e38eb3-d8c4-4a85-b7dd-d80f16704fe1";
            js.ExecuteScript("localStorage.setItem(arguments[0], arguments[1])", key, value);

            _webDriver.Navigate().Refresh();
        }

        public void ReportExceptionAndScreenShots(IEnumerable<string> errorMessages)
        {
            string exceptionGuid = Guid.NewGuid().ToString();

            var errorFileName = $"error_{exceptionGuid}";
            var errorImageName = $"error_{exceptionGuid}";

            GetAndSaveScreenShot(errorImageName);

            var exceptionData = string.Join($"{Environment.NewLine}===== Next Error ====={Environment.NewLine}", errorMessages);

            SaveDataAsTextFile(exceptionData, errorFileName);
        }

        public void GetAndSaveScreenShot(string fileName)
        {
            SeleniumHelper.GetAndSaveScreenShot(fileName, BrowserDirectory, _webDriver);
        }

        public void SaveDataAsTextFile(string data, string fileName)
        {
            Helper.SaveDataAsTextFile(data, fileName, BrowserDirectory);
        }

        public string SwitchTab()
        {
            return _webDriver.SwitchTab();
        }

        public void RemoveHideElement(IWebElement element)
        {
            var scriptExecutor = (IJavaScriptExecutor)_webDriver;

            if (element != null)
            {
                scriptExecutor.ExecuteScript("arguments[0].style.display = 'none'", element);
            }
        }

        public void Refresh()
        {
            _webDriver.Navigate().Refresh();
        }

        public void Close()
        {
            _webDriver.Quit();
        }


        private static void CreateBrowserDirectory(string fullAddress)
        {
            Directory.CreateDirectory(fullAddress);
        }
    }
}
