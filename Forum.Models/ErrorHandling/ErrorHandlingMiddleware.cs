using Forum.Models.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Forum.WebApi.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as ResponseException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;

            if (customException != null)
            {
                message = customException.Message;
                statusCode = customException.StatusCode;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new ErrorOptions
            {
                Message = message,
                StatusCode = statusCode
            }));
        }
    }
}
