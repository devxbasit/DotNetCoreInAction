using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Filters
{
    public class MySampleActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly string _name;
        private readonly Guid _randomId;

        public MySampleActionFilterAttribute(string name)
        {
            _name = name;
            _randomId = Guid.NewGuid();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine($"ActionFilter Sync Before - {_name} {_randomId}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine($"ActionFilter Sync After - {_name}  {_randomId}");
        }
    }
}