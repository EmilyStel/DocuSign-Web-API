using System.Net;
using System.Text.Json;
using Domain.Exceptions;

namespace DocuSign.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception exception)
			{
				context.Response.ContentType = "application/json";
				var result = JsonSerializer.Serialize(new { error = exception.Message });

                context.Response.StatusCode = exception switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    AlreadyExistException => (int)HttpStatusCode.Conflict,
                    InvalidException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };
                await context.Response.WriteAsync(result);
            }
        }
	}
}

