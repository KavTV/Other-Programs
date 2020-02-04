using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System;

namespace Instagram_BOT
{
    class Program
    {
        static int likedPosts = 0;
        static void Main(string[] args)
        {
            while (true)
            {
                bool hiddenChrome = false;
                Console.WriteLine("Write instagram email");
                string inputEmail = Console.ReadLine();
                Console.WriteLine("Write instagram password");
                string inputPassword = ReadPassword();
                //string inputPassword = Console.ReadLine();
                Console.WriteLine("How many posts do you want to like? (Max 54)");
                int inputLikes = int.Parse(Console.ReadLine());
                if (inputLikes > 54)
                {
                    inputLikes = 54;
                }
                Console.WriteLine("Would you like to run chrome hidden? [y/n]");
                string inputChrome = Console.ReadLine();
                if (inputChrome == "y" || inputChrome == "Y")
                {
                    hiddenChrome = true;
                }

                LikeBOT(inputEmail, inputPassword, inputLikes, hiddenChrome);
            }
        }

        private static void LikeBOT(string email, string password, double likes, bool hiddenChrome)
        {
            //ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            //service.HideCommandPromptWindow = true;

            ChromeOptions option = new ChromeOptions();
            if (hiddenChrome == true)
            {
            option.AddArgument("--headless");
            }
            ChromeDriver chromeDriver = new ChromeDriver(option);
            

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
            element.SendKeys(email);

            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/main/div/article/div/div[1]/div/form/div[3]/div/label/input");
            element.SendKeys(password);

            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/main/div/article/div/div[1]/div/form/div[4]/button");
            element.Click();
            Console.WriteLine("Successfully logged in");
            Thread.Sleep(3000);

            // Siger nej til notifikationer kun hvis du kører den uden headless (chrome hidden)
            if (hiddenChrome == false)
            {
            element = chromeDriver.FindElementByXPath("/html/body/div[4]/div/div/div[3]/button[2]");
            element.Click();
            Thread.Sleep(3000);
            }

            Console.WriteLine("Searching for tag...");
            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/input");
            element.SendKeys("#dog");
            Thread.Sleep(1000);

            element = chromeDriver.FindElementByXPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/div[2]/div[2]/div/a[1]");
            element.Click();
            Console.WriteLine("Successfully searched for tag...");
            Thread.Sleep(6000);

            
            likes = Math.Floor(likes/3);
            
            for (int i = 1; i <= likes; i++)
            {

                for (int j = 1; j <= 3; j++)
                {
                    try
                    {
                        // Åbn billede
                        chromeDriver.FindElement(By.XPath($"/html/body/div[1]/section/main/article/div[2]/div/div[{i}]/div[{j}]")).Click();
                        // Like billede
                        Thread.Sleep(1100);
                        chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[2]/div/article/div[2]/section[1]/span[1]/button")).Click();
                        // Luk billede
                        Thread.Sleep(1300);
                        chromeDriver.FindElement(By.XPath("/html/body/div[4]/div[3]/button")).Click();
                        likedPosts++;
                        Console.WriteLine("Liked posts: " + likedPosts);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went wrong");
                        Console.WriteLine(e);
                        Thread.Sleep(1000);
                    }

                }

            }
            likedPosts = 0;
            chromeDriver.Quit();
            
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }


    }
}
