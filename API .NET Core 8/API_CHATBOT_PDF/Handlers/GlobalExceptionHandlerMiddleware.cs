using API_CHATBOT_PDF.Models;
using Newtonsoft.Json;
using System.Net;

namespace API_CHATBOT_PDF.Handlers
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ResponseDto response = new ResponseDto()
            {
                //Success = false,
                //Message = "Ha ocurrido un error al generar la factura del suministro.",
                //Data = null,
                //StatusCode = 400
                Base64 = null,
                Error = "Ha ocurrido un error al obtener el archivo"
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
