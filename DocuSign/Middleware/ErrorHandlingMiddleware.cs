using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocuSign.Api.Middleware
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
				await _next(context);
			}
			catch(Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError;
			var result = JsonSerializer.Serialize(new { error = "OOOO" });
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = 500;
			return context.Response.WriteAsync(result);
		}


    }
}

