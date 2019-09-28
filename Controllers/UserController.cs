using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Throw.Model;

namespace Throw.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private DataContext repo;

        public UserController(DataContext repository)
        {
            repo = repository;
        }

        [HttpPost("login")]
        public JObject LoginUser([FromBody]JObject userIN)
        {

            string email = userIN["email"].ToString();
            string image = userIN["image"].ToString();
            string name = userIN["name"].ToString();
            string token = userIN["token"].ToString();
            User user = new User { Email = email, DisplayName = name, Image = image };
            bool saved = repo.SaveUser(user);



            return new JObject() { { "runResult", saved } };
        }
    }
}