using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using Base64_Framework.Models;
using Newtonsoft.Json;

namespace Base64_Framework.Controllers
{
    public class FacturaController : ApiController
    {
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        // GET: api/Factura
        public IHttpActionResult Get()
        {
            return Json(new RequestModel { Error = "Suministro no proporcionado." });
        }

        // GET: api/Factura/5
        public IHttpActionResult Get(String id)
        {
            logger.Trace($"Obteniendo archivo en base64, NIS: {id}");

            int count = 1;
            RequestModel Base64File = new RequestModel();
            do
            {
                try
                {
                    Base64File = GetBase64String(id, count);
                    if (Base64File.Error != null)
                    {
                        count += 1;
                        continue;
                    }
                    if (Base64File.Base64 != null)
                        return Json(Base64File);
                }
                catch (Exception e)
                {
                    logger.Error($"Error no controlado:  Ha ocurrurido un error al obtener el archivo en base64, {e.Message}");
                }
            } while (count < 4);

            return Json(Base64File);

        }

        private RequestModel GetBase64String(string id, int count)
        {
            try
            {
                if (id != string.Empty && id.Length != 7)
                    throw new Exception("Este suministro no es válido.");
                System.Threading.Thread.Sleep(2000);
                string PdfsPath = Properties.Settings.Default.PdfPath;
                Byte[] fileBytes = System.IO.File.ReadAllBytes($"{PdfsPath}\\NIS_{id}.pdf");
                String base64 = Convert.ToBase64String(fileBytes);
                return new RequestModel { Base64 = base64, Error = null };
            }
            catch (Exception e)
            {
                logger.Error($"Error: Intentos {count}/3 , Ha ocurrurido un error al obtener el archivo en base64, {e.Message}");
                return new RequestModel { Base64 = null, Error = "Error: Ha ocurrurido un error al obtener el archivo en base64" };
            }
        }

    }
}
