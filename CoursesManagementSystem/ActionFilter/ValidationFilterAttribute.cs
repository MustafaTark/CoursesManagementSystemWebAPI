using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using CoursesManagementSystem.Repo;
using CoursesManagementSystem.Models;
using CoursesManagementSystem.DTO;

namespace CoursesManagementSystem.ActionFilter
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is UserForRegisterDTO);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
