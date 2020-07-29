using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using RequestResponseLogging.API.Models;
using RequestResponseLogging.API.Services;
using RequestResponseLogging.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestResponseLogging.API.Middleware
{
    public class RequestResponseLogging
    {
        private readonly RequestDelegate _next;
        private readonly IRequestResponseService _service;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private RequestResponseLog logEntity;
        public RequestResponseLogging(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

        }
        public async Task Invoke(HttpContext context, IRequestResponseService service)
        {
            //First, get the incoming request
            var request = await FormatRequest(context.Request);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;

                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                //Format the response from the server
                var response = await FormatResponse(context.Response);

                //Save log to chosen datastore
                service.Insert(logEntity);

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }

        }


        private async Task<RequestResponseLog> FormatRequest(HttpRequest request)
        {

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await request.Body.CopyToAsync(requestStream);

            var requestHeader = new StringBuilder(Environment.NewLine);
            foreach (var header in request.Headers)
            {
                requestHeader.AppendLine($"{header.Key}:{header.Value}");
            }
            logEntity = new RequestResponseLog();
            logEntity.Url = $"{request.Host}{request.Path}";
            logEntity.RequestHeader = requestHeader.ToString();
            logEntity.RequestBody = ReadStreamInChunks(requestStream, Convert.ToInt32(request.ContentLength));
            logEntity.QueryString = request.QueryString.HasValue ? request.QueryString.Value : null;
            logEntity.RequestDate = DateTime.Now;
            logEntity.TraceId = Activity.Current?.Id ?? request.HttpContext.TraceIdentifier;
            logEntity.Scheme = request.Scheme;

            request.Body.Position = 0;
            return logEntity;
        }

        private async Task<RequestResponseLog> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            logEntity.ResponseContent = text;
            logEntity.ResponseDate = DateTime.Now;
            logEntity.StatusCode = response.StatusCode;

            return logEntity;
        }

        private static string ReadStreamInChunks(Stream stream,int contentLength)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[contentLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   contentLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
    }
}
