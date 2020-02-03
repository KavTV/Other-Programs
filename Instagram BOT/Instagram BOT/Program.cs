using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System;

namespace Instagram_BOT
{
    class Program
    {
        static void Main(string[] args)
        {
            int likedPosts = 0;
            //ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;
            ChromeOptions option = new ChromeOptions();
            
            option.AddArgument("--headless");
            ChromeDriver chromeDriver = new ChromeDriver();
            

            chromeDriver.Navigate().GoToUrl("https://www.instagram.com");
            Thread.Sleep(4000);
            IWebElement element;

            Console.WriteLine("Clicking log in...");
            element = chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/article/div[2]/div[2]/p/a"));
            element.Click();

            Thread.Sleep(3000);
            Console.WriteLine("writing info...");
            // Skriver login oplysninger..
            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/main/div/article/div/div[1]/div/form/div[2]/div/label/input");
            element.SendKeys("simbasbreve@gmail.com");
            
            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/main/div/article/div/div[1]/div/form/div[3]/div/label/input");
            element.SendKeys("");
            
            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/main/div/article/div/div[1]/div/form/div[4]/button");
            element.Click();
            Console.WriteLine("Successfully logged in");
            Thread.Sleep(3000);

            // Siger nej til notifikationer kun hvis du kører den uden headless (chrome hidden)
            element = chromeDriver.FindElementByXPath("/html/body/div[4]/div/div/div[3]/button[2]");
            element.Click();
            Thread.Sleep(3000);

            Console.WriteLine("Searching for tag...");
            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/input");
            element.SendKeys("#dog");
            Thread.Sleep(1000);

            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/div[2]/div[2]/div/a[1]");
            element.Click();
            Console.WriteLine("Successfully searched for tag...");
            Thread.Sleep(6000);

            //// Åbn billede
            //chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/article/div[2]/div/div[1]/div[1]")).Click();
            //// Like billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
            //// Luk billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/button[1]")).Click();

            //// Åbn billede
            //chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/article/div[2]/div/div[1]/div[2]")).Click();
            //// Like billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
            //// Luk billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/button[1]")).Click();

            //// Åbn billede
            //chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/article/div[2]/div/div[1]/div[3]")).Click();
            //// Like billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
            //// Luk billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/button[1]")).Click();

            //// Åbn billede
            //chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/article/div[2]/div/div[2]/div[1]")).Click();
            //// Like billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
            //// Luk billede
            //Thread.Sleep(1000);
            //chromeDriver.FindElement(By.XPath("/html/body/div[4]/button[1]")).Click();

            for (int i = 1; i <= 100; i++)
            {

                for (int j = 1; j <= 3; j++)
                {
                    try
                    {
                // Åbn billede
                chromeDriver.FindElement(By.XPath($"/html/body/div[1]/section/main/article/div[2]/div/div[{i}]/div[{j}]")).Click();
                // Like billede
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
                // Luk billede
                Thread.Sleep(1000);
                chromeDriver.FindElement(By.XPath("/html/body/div[4]/button[1]")).Click();
                    likedPosts++;
                    Console.WriteLine("Liked posts: "+ likedPosts);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }

            }

            chromeDriver.Quit();
        }
    }
}
