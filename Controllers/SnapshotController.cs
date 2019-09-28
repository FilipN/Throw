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
    public class SnapshotController : Controller
    {
        private DataContext repo;

        public SnapshotController(DataContext repository)
        {
            repo = repository;
        }

        [HttpPost("snapforproject")]
        public JObject GetSnapsForProject([FromBody]JObject project)
        {
            string username = project["identity"].ToString();
            string projectGuid = project["guid"].ToString();

            string res = repo.GetSnapshotsByGUID(projectGuid);
            JObject result = new JObject() { { "result", res } };
            return result;

        }
    }
}