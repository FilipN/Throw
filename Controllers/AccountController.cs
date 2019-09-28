using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Throw.Model;  

namespace Throw.Controllers
{
    [Route("api/[controller]")]
    public class AccountController: Controller
    {

        private DataContext repo;

        public AccountController(DataContext repository)
        {
            repo = repository;
        }



    }
}