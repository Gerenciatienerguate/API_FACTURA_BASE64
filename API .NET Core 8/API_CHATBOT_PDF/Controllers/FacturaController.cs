using API_CHATBOT_PDF.Interfaces;
using API_CHATBOT_PDF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using SharedInvoicePdfLib.Helpers;
using SharedInvoicePdfLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_CHATBOT_PDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Configuration _configuration;
        private readonly IFileManagerService _fileManagerService;

        public FacturaController(IFileManagerService fileManagerService, IOptions<Configuration> configuration)
        {
            _fileManagerService = fileManagerService;
            _configuration = configuration.Value;
        }

        // GET api/<FacturaController>/5
        [HttpGet("{id}")]
        public async Task<ResponseDto> Get(int id)
        {

            _logger.Trace($"Obteniendo factura del NIS: {id}");

            try
            {

                if (id.ToString().Length > 7) throw new WebServiceExceptionDto() { Message = "El NIS no puede tener mas de 7 digitos." };

                // Verificacion de existencia de la factura                
                string filePath = $"{_configuration.BaseFilePath}\\NIS_{id}.pdf";
                byte[] fileBytesArray = await _fileManagerService.GetFileBytesArray(filePath);
                string fileBase64String = _fileManagerService.GetBase64(fileBytesArray);

                var logger = new InvoicePdfLogger();
                var pdfService = new InvoicePdfService(logger);
                pdfService.DeleteInvoicePdfFiles(_configuration.BaseFilePath, new List<string> { id.ToString() });

                return new ResponseDto()
                {
                    Base64 = fileBase64String,
                    Error = null
                    /*Success = true,
                    Message = "Factura obtenida correctamente",
                    Data = new InvoinceFile()
                    {
                        Base64 = fileBase64String
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
