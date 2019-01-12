using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Throw.Models;

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        UserDataAccessLayer objuser = new UserDataAccessLayer();

        [HttpGet]
        [Route("api/User/Index")]
        public IEnumerable<User> Index()
        {
            return objuser.GetUsers();
        }


        [HttpPost("Login")]
        public string Login([FromBody] dynamic input)
        {
            JObject jInput = input as JObject;
            User user = new User { Email = jInput["email"].ToString(), Name = jInput["name"].ToString(), Photo = jInput["picture"]["data"]["url"].ToString() };
            if(!objuser.CheckIfUserExists(user))
                objuser.AddUser(user);
            return "Success";
        }

        [HttpPost("Create")]
        public int Create(User user)
        {
            return objuser.AddUser(user);
        }


        [HttpGet]
        [Route("api/Employee/Details/{id}")]
        public User Details(int id)
        {
            return objuser.GetUserData(id);
        }

        [HttpPut]
        [Route("api/Employee/Edit")]
        public int Edit(User user)
        {
            return objuser.UpdateUser(user);
        }

        [HttpDelete]
        [Route("api/Employee/Delete/{id}")]
        public int Delete(int id)
        {
            return objuser.DeleteUser(id);
        }

    }
}