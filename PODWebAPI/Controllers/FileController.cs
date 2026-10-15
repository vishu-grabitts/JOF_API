using POD.Logics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("")]
    public class FileController : ApiController
    {
        Output output = new Output();

        #region fileaccess
        [HttpGet]
        [Route("api/Files/{*filename}")]
        [APIAuthorizeAttribute]
        public HttpResponseMessage Files(string filename)
        {
            try
            {
                //var stream = new MemoryStream();
                //// processing the stream.

                //var result = new HttpResponseMessage(HttpStatusCode.OK)
                //{
                //    Content = new ByteArrayContent(stream.ToArray())
                //};
                //result.Content.Headers.ContentDisposition =
                //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                //    {
                //        FileName = filename
                //    };
                //result.Content.Headers.ContentType =
                //    new MediaTypeHeaderValue("application/octet-stream");

                //return result;

                //var path = System.Web.HttpContext.Current.Server.MapPath("~/Files/"+ filename); ;
                //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                //var stream = new FileStream(path, FileMode.Open);
                //result.Content = new StreamContent(stream);
                //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                //result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
                //result.Content.Headers.ContentType = new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(filename));
                //result.Content.Headers.ContentLength = stream.Length;
                //return result;

                //Create HTTP Response.
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                
                //Set the File Path.
                //string filePath = "D:\\Files\\" + filename;
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Files/"+ filename);
                if(Path.GetExtension(filePath)==".pdf")
                {
                    filePath = System.Web.HttpContext.Current.Server.MapPath("~/Files/MailPDF/" + filename);
                }
                //Check whether File exists.
                if (!File.Exists(filePath))
                {
                    //Throw 404 (Not Found) exception if File not found.
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ReasonPhrase = string.Format("File not found: {0} .", filename);
                    throw new HttpResponseException(response);
                }

                //Read the File into a Byte Array.
                byte[] bytes = File.ReadAllBytes(filePath);

                //Set the Response Content.
                response.Content = new ByteArrayContent(bytes);

                //Set the Response Content Length.
                response.Content.Headers.ContentLength = bytes.LongLength;

                //Set the Content Disposition Header Value and FileName.
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                //response.Content.Headers.ContentDisposition.FileName = filename;

                string mimetype = MimeMapping.GetMimeMapping(filename);
                //Set the File Content Type.
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(filename));

                //FileStream fileStream = new FileStream(filePath, FileMode.Open);
                //Image image = Image.FromStream(fileStream);
                //MemoryStream memoryStream = new MemoryStream();
                //image.Save(memoryStream, ImageFormat.Jpeg);
                //response.Content = new ByteArrayContent(memoryStream.ToArray());
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                return response;

            }
            catch (Exception ex)
            {
                output.ResponseCode = 500;
                output.Message = ex.Message.ToString();
                output.Data = null;
                return Request.CreateResponse(HttpStatusCode.InternalServerError, output);
            }
        }
        #endregion


    }
}
