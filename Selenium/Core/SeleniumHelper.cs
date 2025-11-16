using OpenQA.Selenium;

namespace CerbiSharp.Infrastructure.Base.Selenium.Core
{
    public class SeleniumHelper
    {
        public static void GetAndSaveScreenShot(string fileName, string path, ITakesScreenshot takesScreenshot)
        {
            // question 
            Screenshot screenshot = takesScreenshot.GetScreenshot();

            string pathAndNameFile = Path.Combine(path, $"{fileName}.png");

            screenshot.SaveAsFile(pathAndNameFile);
        }



    }
}
