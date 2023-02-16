using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerManagement.Exceptions
{
    public class GeneralExceptionHandler : IActionFilter, IOrderedFilter
    {
        public int Order => Int32.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new ObjectResult(new
                {
                    context.Exception.Message
                })
                {
                    StatusCode = 400
                };

                context.ExceptionHandled = true;
            }

            if (!context.ExceptionHandled && context.Exception != null)
            {
                context.Result = new OkObjectResult(new
                {
                    context.Exception.Message
                })
                {
                    StatusCode = 503
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
