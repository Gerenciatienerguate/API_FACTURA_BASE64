using API_CHATBOT_PDF.Interfaces;
using API_CHATBOT_PDF.Models;
using API_CHATBOT_PDF.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using SharedInvoicePdfLib.Builders;
using SharedInvoicePdfLib.Helpers;
using SharedInvoicePdfLib.Services;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_CHATBOT_PDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenFacturaController : ControllerBase
    {

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Configuration _configuration;


        public GenFacturaController(IOptions<Configuration> configuration)
        {
            _configuration = configuration.Value;
        }

        // GET api/<GenFacturaController>/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {

            _logger.Trace($"Generando factura del NIS: {id}");

            try
            {

                if (id.ToString().Length > 7) throw new WebServiceExceptionDto() { Message = "El NIS no puede tener mas de 7 digitos." };
                List<string> listNisesToGeneratePdf = new List<string> { id.ToString() };

                // CREACION DE FACTURA EN API DE FACTURAS
                string url = InvoicePdfUrlBuilder.Build(_configuration.PdfGeneratorUri, listNisesToGeneratePdf);
                var logger = new InvoicePdfLogger();
                var pdfService = new InvoicePdfService(logger);
                bool result = await pdfService.RequestGeneratePdfAsync(url);

                return new
                {
                    Message = "Generando Factura..."
                    /*Success = true,
                    Message = "Factura Generada correctamente",
                    Data = new InvoinceFile()
                    {
                        Base64 = ""
                    },
                    StatusCode = 200*/
                };
            }
            catch (Exception exc)
            {
                _logger.Error($"Error al generar factura: {id} - {exc.Message}");
                throw;
            }

        }
    }
}
