using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using WebScrapingAPI.Abstract;
using WebScrapingAPI.Hub;
using WebScrapingAPI.Model;

namespace WebScrapingAPI.Service
{
    public class CnpjService : ICnpjService
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;    
        private IHubContext<HubClient, IHubClient> _hubContext;
        public CnpjService(SeleniumConfigurations configurations, IHubContext<HubClient, IHubClient> hubContext)
        {
            _configurations = configurations;
            _hubContext = hubContext;
        }

        public void Carregar(string cnpj, string idConnection)
        {
            this.LoadPage();

            string capcha = GetCapcha();

            this.SetCnpj(cnpj);

            _hubContext.Clients.Client(idConnection).EnviarImagemCapcha(capcha);           
        }

        public void ConsultaCnpj(string capcha, string idConnection)
        {            
            this.SetCapcha(capcha);
            _driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            var screenshot = (_driver as ChromeDriver).GetScreenshot();
            _hubContext.Clients.Client(idConnection).EnviarCaptura(screenshot.AsBase64EncodedString);
        }

        private void SetCapcha(string capcha)
        {
            _driver.FindElement(By.Id("txtTexto_captcha_serpro_gov_br")).Click();
            _driver.FindElement(By.Id("txtTexto_captcha_serpro_gov_br")).SendKeys(Keys.Home);
            foreach (var item in capcha.ToCharArray())
            {
                _driver.FindElement(By.Id("txtTexto_captcha_serpro_gov_br")).SendKeys(item.ToString());
            }
        }

        private void SetCnpj(string cnpj) 
        {
            _driver.FindElement(By.Id("cnpj")).Click();            
            _driver.FindElement(By.Id("cnpj")).SendKeys(Keys.Home);
            foreach(var item in cnpj.ToCharArray())
            {
                _driver.FindElement(By.Id("cnpj")).SendKeys(item.ToString());
            }
        }

        private void LoadPage()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--headless", "--incognito");
            _driver = new ChromeDriver(_configurations.PathDriverChrome, options);

            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_configurations.Timeout);
            _driver.Navigate().GoToUrl($"{_configurations.Url}");
        }

        private string GetCapcha()
        {
            string result = string.Empty;
            var remElement = _driver.FindElement(By.Id("imgCaptcha"));
            Point location = remElement.Location;

            var screenshot = (_driver as ChromeDriver).GetScreenshot();
            using (MemoryStream memoryStream = new MemoryStream())
            using (MemoryStream stream = new MemoryStream(screenshot.AsByteArray))
            {
                using (var bitmap = new Bitmap(stream))
                {
                    RectangleF part = new RectangleF(location.X, location.Y, remElement.Size.Width, remElement.Size.Height);
                    using (Bitmap bn = bitmap.Clone(part, bitmap.PixelFormat))
                    {
                        bn.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        result = Convert.ToBase64String(memoryStream.GetBuffer());
                    }
                }
            }

            return result;
        }
    }
}
