using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelValidation.Models;

#nullable enable
namespace ModelValidation.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    // [Consumes(), Produces()]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> GetProductDetails(int id, string name)
        {
            // https://localhost:5001/WeatherForecast/GetProductDetails?Id=10

            if (id <= 0)
            {
                ModelState.AddModelError(nameof(id), $"{nameof(id)} is required");
            }

            if (String.IsNullOrEmpty(name))
            {
                ModelState.AddModelError(nameof(name), $"{nameof(name)} is Required");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return $"Id = {id}, Name = {name}";
        }

        [HttpGet]
        public string GetStudent([Required] int id, [Required] string name)
        {
            // https://localhost:5001/WeatherForecast/GetStudent?Id=one
            // Model state represents errors that come from two subsystems: model binding and model validation. Errors that originate from model binding are generally data conversion errors
            return $"Id = {id}, Name = {name}";
        }

        [HttpGet]
        public string AddStudent([FromQuery] Student student)
        {
            // https://localhost:5001/WeatherForecast/AddStudent?Id=15&Name=Wasit
            // https://localhost:5001/WeatherForecast/AddStudent?Id=15&Name=Basit
            // https://localhost:5001/WeatherForecast/AddStudent?Id=15&Name=Basit&Contact=123

            return "Student Added!";
        }

        [HttpGet]
        public Student ContentNegotiationTest()
        {
            // only default & xml output formatter is configured
            // accept header =  application/zip, application/xml,application/json

            return new Student()
            {
                Id = 10,
                Name = "Basit"
            };
        }
    }
}