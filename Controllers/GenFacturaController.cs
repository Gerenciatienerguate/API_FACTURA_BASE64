using Base64_Framework.Models;
using System;
using System.Web.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;


namespace Base64_Framework.Controllers
{
    public class GenFacturaController : ApiController
    {

        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // GET api/<controller>
        public IHttpActionResult Get()
        {
            return Json(new RequestModel { Error = "Suministro no proporcionado." });
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(string id)
        {
            logger.Trace($"Generando factura NIS:{id}");

            IWebDriver driver = null;
            try
            {
                if (id != string.Empty && id.Length != 7)
                    throw new Exception("Este suministro no es válido.");

                string path = AppContext.BaseDirectory;
                //FirefoxDriverService service = FirefoxDriverService.CreateDefaultService($@"{path}\gecko\", "geckodriver.exe");
                //service.FirefoxBinaryPath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
                //var options = new FirefoxOptions();
                //options.AddArgument("no-sandbox");
                //IWebDriver driver = new FirefoxDriver(service, options);

                EdgeDriverService service = EdgeDriverService.CreateDefaultService($@"{path}\gecko\", "msedgedriver.exe");
                driver = new EdgeDriver(service);
                string pdfGeneratorApiUri = Properties.Settings.Default.PdfGeneratorApiUri;
                driver.Navigate().GoToUrl($"{pdfGeneratorApiUri}/?nises={id}");
                System.Threading.Thread.Sleep(10000);
                driver.Quit();
                return Json(new Response { Message = "Generando factura..." });

            }
            catch (Exception e)
            {
                if (driver != null)
                    driver.Quit();

                logger.Error($"Error: Ha ocurrido un error al generar la factura, {e.Message}");
                return Json(new RequestModel { Error = "Error: Ha ocurrido un error al generar la factura" });

            }
        }

    }
}