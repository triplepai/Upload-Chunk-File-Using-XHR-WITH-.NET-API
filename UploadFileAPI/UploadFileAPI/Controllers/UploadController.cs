using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UploadFileAPI.Models;

namespace UploadFileAPI.Controllers
{
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {
        public static void AppendAllBytes(string path, byte[] bytes)
        {

            using (var stream = new FileStream(path, File.Exists(path) ? FileMode.Append : FileMode.CreateNew))
            {
                stream.Write(bytes, 0, bytes.Length);
            }

        }
        [Route("uploads")]
        [HttpPost]
        public async Task<HttpResponseMessage> Uploads()
        {
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            foreach (var stream in filesReadToProvider.Contents)
            {
                var fileBytes = await stream.ReadAsByteArrayAsync();
                AppendAllBytes("C:\\F2F\\" + stream.Headers.ContentDisposition.FileName.Replace("\"", ""), fileBytes);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "success!");
        }

        //const string StoragePath = @"C:\F2F";
        //[Route("uploads")]
        //[HttpPost]
        //public void Post()
        //{
        //    if (Request.Content.IsMimeMultipartContent())
        //    {
        //        var streamProvider = new MultipartFormDataStreamProvider("c:/F2F/");
        //        var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
        //        {
        //            if (t.IsFaulted || t.IsCanceled)
        //                throw new HttpResponseException(HttpStatusCode.InternalServerError);
        //        });
        //    }
        //    else
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
        //    }
        //}


    }
}
