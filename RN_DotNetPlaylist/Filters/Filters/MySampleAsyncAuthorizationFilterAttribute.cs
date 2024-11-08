using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters.Filters
{
    public class MySampleAsyncAuthorizationFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _name;
        private readonly Guid _randomId;

        public MySampleAsyncAuthorizationFilterAttribute(string name)
        {
            _name = name;
            _randomId = Guid.NewGuid();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine($"Authorization Filter - {_name} {_randomId}");

            // to short circuit, all we need to do is to set context.Result (return  a cached data)
            // context.Result = new ContentResult()
            // {
            //     Content = "OK"
            // };

            await Task.CompletedTask;
        }
    }
}