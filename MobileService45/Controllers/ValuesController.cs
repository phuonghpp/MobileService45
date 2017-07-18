using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.Models;
using System.Threading.Tasks;
using MobileService45.ViewModels;
using MobileService45.AlineTest;
using MobileService45.Mytv;
using System.Web.Configuration;

namespace MobileService45.Controllers
{
    // this is test api, not for use!
    public class ValuesController : ApiController
    {
        OracleMobileDB db = new OracleMobileDB();
        // GET api/values
        public async Task<IHttpActionResult> Get(string Mobile, string Content, string CountMe)
        {
            //var user = db.USERS.FirstOrDefault();
            //db.Dispose();
            //return user.FULLNAME;
            if (CountMe != "1234567") return BadRequest("Count again!");
            var SMSSender = new SMSSender();
            var sent =    await  SMSSender.Send(Content, Mobile);
            return Ok(sent);

        }

        [HttpPost]
        [Route("GetInfor")]

        public async Task<IHttpActionResult> GetInfor(string _PhoneNumber, string _Password, string _OTP, string _Account)
        {
            // chua bat loi input
            if (CheckUser(_PhoneNumber, _Password, _OTP) == false) return Unauthorized();
            ServicesSoapClient Client = new ServicesSoapClient("ServicesSoap");
            string User = WebConfigurationManager.AppSettings["AlineTestAccount"];
            string Password = WebConfigurationManager.AppSettings["AlineTestPassword"];



            //var response = await Client.GetAccountAsync(User, Password, _Account);
            //return Ok(response);
            var res = Client.GetAccount(User, Password, _Account);
            return Ok(res);
        }


        [HttpGet]
        [Route("GetMytv")]

        public async Task<IHttpActionResult> GetMytv(string _PhoneNumber,string _Password,string _OTP,string _Account)
        {
            // chua bat loi input
            
            if (CheckUser(_PhoneNumber, _Password, _OTP) == false) return Unauthorized();
            SubscriberManagementSoapClient Client = new SubscriberManagementSoapClient("SubscriberManagementSoap");
            AuthHeader Header = new AuthHeader();
            Header.strUserName = WebConfigurationManager.AppSettings["MytvAccount"];
            Header.strPassword = WebConfigurationManager.AppSettings["MytvPassword"];

           // GetPackageNameResponse response = await Client.GetPackageNameAsync(Header, _Account);
            var res = Client.GetPackageName(Header, _Account);


            //return Ok(response);
            return Ok(res); ;


        }
        [HttpGet]
        [Route("GetAccountMyTVList")]
        public async Task<IHttpActionResult> GetAccountMyTVList(string _OTP)
        {
            if (_OTP != "000000") return BadRequest();
            var result = db.Database.SqlQuery<string>("SELECT ACCOUNT FROM bd_mac_mytv").ToList();
            return Ok(result);
        }
        // need to be more specific on return checkuser
        private bool CheckUser(string _Mobile, string _Password, string _OTP)
        {
            var user = db.USERS
                         .Where(x => x.MOBILE == _Mobile & x.PASSWORD == _Password &_OTP=="000000")
                         .First();
            if (user != null & user.TIME_OTP <= DateTime.UtcNow.AddHours(5))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
