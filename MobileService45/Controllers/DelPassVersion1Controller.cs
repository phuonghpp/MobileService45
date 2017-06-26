using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.DAL;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace MobileService45.Controllers
{
    [RoutePrefix("DEL_PASSVersion1")]
    public class DelPassVersion1Controller : ApiController
    {
        [Route("GET_DS_DEL_PASS")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDSDelPass(string Mobile, string Password,string OTP , int P_Count)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("P_COUNT", OracleDbType.Int32);
            p1.Value = P_Count;
            param.Add(p1);
            var Result = Datacs.GetData("DEL_PASS.", "GET_DS_DEL_PASS", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }
        [Route("UPDATE_DEL_PASS")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateDelPass(string Mobile, string Password,string OTP, int P_ID, string P_Account, int P_Status, string P_Descript)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("P_ID", OracleDbType.Int32);
            OracleParameter p2 = new OracleParameter("P_ACCOUNT", OracleDbType.Varchar2);
            OracleParameter p3 = new OracleParameter("P_STATUS", OracleDbType.Int32);
            OracleParameter p4 = new OracleParameter("P_DESCRIPT", OracleDbType.Varchar2);
            p1.Value = P_ID;
            p2.Value = P_Account;
            p3.Value = P_Status;
            p4.Value = P_Descript;
            param.Add(p1);
            param.Add(p2);
            param.Add(p3);
            param.Add(p4);

            var Result = Datacs.GetData("DEL_PASS.", "UPDATE_DEL_PASS", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }

    }
}
