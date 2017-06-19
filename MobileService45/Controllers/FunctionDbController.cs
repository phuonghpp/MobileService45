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
    [RoutePrefix("FUNCTION_DB")]
    public class FunctionDbController : ApiController
    {
        [Route("BD_STB")]
        [HttpGet]
        public  async Task<IHttpActionResult> BDSTB(string Mobile,string Password ,int NO)
        {
            if (!Datacs.IsValidMobile(Mobile, Password)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("NO", OracleDbType.Int32);
            p1.Value = NO;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "BD_STB", param);
            if (Result == null)
            {
                return BadRequest();

            }
            return Ok(Result);
        }
        [Route("GET_DM_DSLAM")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDMDSLAM(string Mobile,string Password,string SSYSNAME)
        {
            if (!Datacs.IsValidMobile(Mobile, Password)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SSYSNAME", OracleDbType.Varchar2);
            p1.Value = SSYSNAME;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "GET_DM_DSLAM", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }
        [Route("INSERT_HUY_DV")]
        [HttpGet]
        public async Task<IHttpActionResult> InsertHuyDv(string Mobile,string Password, string SACC)
        {
            if (!Datacs.IsValidMobile(Mobile, Password)) return Unauthorized();

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SACC", OracleDbType.Varchar2);
            p1.Value = SACC;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "INSERT_HUY_DV", param);
            if (Result == null)
            {
                return BadRequest();    
            }
            return Ok(Result);
        }
        [Route("INSERT_HUY_PORT")]
        [HttpGet]
        public async Task<IHttpActionResult> InsertHuyPort(string Mobile,string Password, string SACC)
        {
            if (!Datacs.IsValidMobile(Mobile, Password)) return Unauthorized();

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SACC", OracleDbType.Varchar2);
            p1.Value = SACC;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "INSERT_HUY_PORT", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }
        [Route("UPDATE_CSSVUNG")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateCSSVung(string Mobile, string Password, string MADK)
        {
            if (!Datacs.IsValidMobile(Mobile, Password)) return Unauthorized();

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("MADK", OracleDbType.Varchar2);
            p1.Value = MADK;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_CSSVUNG", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }

    }
}
