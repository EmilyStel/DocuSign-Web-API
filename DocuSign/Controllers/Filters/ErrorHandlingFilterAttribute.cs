using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DocuSign.Api.Controllers.Filters
{
	public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
	{
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.Result = new ObjectResult(new { error = "error" });
            context.ExceptionHandled = true;
        }
    }
}

