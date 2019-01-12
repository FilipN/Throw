using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        ErrorLogDataAccessLayer error = new ErrorLogDataAccessLayer();

        [HttpGet("Index")]
        public IEnumerable<User> Index()
        {
            return objuser.GetUsers();
        }


        [HttpPost("Login")]
        public string Login([FromBody] dynamic input)
        {
            try
            {
                JObject jInput = input as JObject;
                User user = new User { Email = jInput["email"].ToString(), Name = jInput["name"].ToString(), Photo = jInput["picture"]["data"]["url"].ToString() };
                if (objuser.CheckIfUserExists(user) == null)
                    return "Error";

                if (objuser.CheckIfUserExists(user) == false)
                    objuser.AddUser(user);
                return "Success";
            }
            catch(Exception e)
            {
                ErrorLog log = new ErrorLog { Component = this.GetType().Name, Function = MethodBase.GetCurrentMethod().Name, Description = e.Message, Time = DateTime.Now };
                error.AddError(log);
                return null;
            }
        }

        /*[HttpPost("Create")]
        public int Create(User user)
        {
            return objuser.AddUser(user);
        }


        [HttpGet("Details/{id}")]
        public User Details(int id)
        {
            return objuser.GetUserData(id);
        }

        [HttpPut("Edit")]
        public int Edit(User user)
        {
            return objuser.UpdateUser(user);
        }

        [HttpDelete("Delete/{id}")]
        public int Delete(int id)
        {
            return objuser.DeleteUser(id);
        }*/

    }
}