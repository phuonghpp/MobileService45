using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using MobileService45.Models;
using System.Data.Entity;


namespace MobileService45.Controllers
{
    [RoutePrefix("EFADV")]
    public class EntityFrameworkADVController : ApiController
    {   
        private static async Task<USER> GetUser(string mobile)
        {
            USER User = null;
            using(var context = new OracleMobileDB())
            {
                User = await (context.USERS.Where(x => x.MOBILE == mobile).FirstOrDefaultAsync<USER>());

            }
            return User;
        }
        [HttpGet]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(string Mobile,string Password)
        {
            USER User =await GetUser(Mobile);
            return Ok(User);
        }

    }
}
