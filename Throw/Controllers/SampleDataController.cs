using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public string WeatherForecasts()
        {
            return "";
        }

        [HttpPost("[action]")]
        public string Login([FromBody] dynamic input)
        {
            JObject jInput = input as JObject;


            return "{name:Filip}";
        }
    }
}