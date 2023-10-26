using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelBinding.Model;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json", "application/xml")]
    public class WeatherForecastController : ControllerBase
    {
        [BindProperty(SupportsGet = true, Name = "TestBindId")]
        public int TestBind { get; set; }

        [HttpPost, HttpGet]
        [Route("{age}/age")]
        public string Get([FromBody] WeatherForecast weatherForecast, [FromQuery(Name = "FirstName")] string name,
            [FromRoute] int age, [FromHeader(Name = "Content-Length")] string contentLength, [FromForm] bool isActive)
        {
            // https://localhost:5001/WeatherForecast/18/age?FirstName=Basit
            return $"Id = {weatherForecast.Id}, name = {name}, age = {age}, content length = {contentLength}";
        }

        [HttpGet("[action]")]
        public string GetArray([FromQuery] Student student)
        {
            //https://localhost:5001/WeatherForecast/GetArray?marks=10&Marks=20&Marks=30
            //https://localhost:5001/WeatherForecast/GetArray?marks[2]=10&Marks[1]=20&Marks[0]=30
            return student.Marks[0].ToString();
        }


        [HttpGet("[action]")]
        public string GetDict([FromQuery] Student student)
        {
            // https://localhost:5001/WeatherForecast/GetDict?SelectedCourses[1035]=english&SelectedCourses[1036]=Maths
            student.SelectedCourses.TryGetValue(1035, out string course);
            return course;
        }

        [HttpGet("[action]")]
        public int BindPropertyTest()
        {
            //https://localhost:5001/WeatherForecast/BindPropertyTest?TestBindId=99
            return TestBind;
        }

        [HttpGet("[action]")]
        public string XmlTest(Employee employee)
        {
            // https://localhost:5001/WeatherForecast/XmlTest
            /*
             <Employee>
                <Id>10</Id>
                <Name>Basit</Name>
            </Employee> 
            */

            return $"XMl Serialization in Action, id  = {employee.Id}, Name = {employee.Name}";
        }

        [HttpGet("[action]/{employeeId}")]
        public Employee GetEmployeeById(int employeeId)
        {
            return new Employee()
            {
                Id = 10,
                Name = "Basit"
            };
        }
    }
}