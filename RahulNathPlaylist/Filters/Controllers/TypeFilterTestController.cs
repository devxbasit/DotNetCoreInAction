using Filters.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Filters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [TypeFilter(typeof(MySample2AsyncResultFilterAttribute), Arguments = new object[] { "Controller" })]
    public class TypeFilterTestController : ControllerBase
    {
        [HttpGet]
        [TypeFilter(typeof(MySample2AsyncResultFilterAttribute), Arguments = new object[] { "Action" })]
        public string TypedFilter()
        {
            return "Ok";
        }
    }
}