namespace Isitar.DependencyUpdater.Api.Middlewares
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Common;
    using Application.Common.Exceptions;
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Services;

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IJsonSerializer jsonSerializer;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, IJsonSerializer jsonSerializer)
        {
            this.next = next;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    var errDict = validationException.Errors
                        .GroupBy(e => e.PropertyName.FirstCharacterToLower(), e => e.ErrorMessage)
                        .ToDictionary(e => e.Key, e => e.AsEnumerable());
                    result = jsonSerializer.Serialize(errDict);
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
                case ArgumentException _:
                    code = HttpStatusCode.InternalServerError;
                    result = "unspecified error";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            if (string.Empty == result)
            {
                result = jsonSerializer.Serialize(new[] {exception.Message});
            }

            return context.Response.WriteAsync(result);
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}