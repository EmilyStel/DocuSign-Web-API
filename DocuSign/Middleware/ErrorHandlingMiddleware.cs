using System.Net;
using System.Text.Json;
using Domain.Exceptions;

namespace DocuSign.Middleware
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
			catch (Exception exception)
			{
				//await HandleExceptionAsync(context, ex);

				context.Response.ContentType = "application/json";
				var result = JsonSerializer.Serialize(new { error = exception.Message });

                switch (exception)
				{
					case NotFoundException:
						context.Response.StatusCode = (int)HttpStatusCode.NotFound;
						break;

					case AlreadyExistException:
						context.Response.StatusCode = (int)HttpStatusCode.Conflict;
						break;
					case InvalidException:
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						break;
					default:
						context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
				}

				await context.Response.WriteAsync(result);
            }
        }

		//private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		//{
  //          context.Response.ContentType = "application/json";
  //          var result = JsonSerializer.Serialize(new { error = exception.Message });

		//	if (exception is NotFoundException)
		//	{
		//		context.Response.StatusCode = (int)HttpStatusCode.NotFound;
		//	}

		//	else if (exception is AlreadyExistException)
		//	{
		//		context.Response.StatusCode = 409;
		//	}

		//	else if (exception is InvalidException)
		//	{
		//		context.Response.StatusCode = 409;
  //          }

		//	else
		//	{
		//		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		//	}

  //          return context.Response.WriteAsync(result);
		//}
    }
}

