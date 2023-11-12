using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Filters
{
    public class MySampleAsyncActionFilterAttribute : Attribute, IAsyncActionFilter, IOrderedFilter
    {
        private readonly string _name;
        private readonly Guid _randomId;

        public MySampleAsyncActionFilterAttribute(string name, int order = 0)
        {
            _name = name;
            Order = order;
            _randomId = Guid.NewGuid();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"ActionFilter Async Before - {_name} {_randomId}");
            
            
            
            await next();
            Console.WriteLine($"ActionFilter Async After - {_name} {_randomId}");
        }

        public int Order { get; set; }
    }
}