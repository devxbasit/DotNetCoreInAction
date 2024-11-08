using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Filters
{
    public class MySampleAsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly string _name;
        private readonly Guid _randomId;

        public MySampleAsyncResourceFilterAttribute(string name)
        {
            _name = name;
            _randomId = Guid.NewGuid();
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine($"ResourceFilter Async Before - {_name} {_randomId}");
            await next();
            Console.WriteLine($"ResourceFilter Async After - {_name} {_randomId}");
        }
    }
}