using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.DAL;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MobileService45.Models;


namespace MobileService45.Controllers
{
    [RoutePrefix("FUNCTION_DBVersion1")]
    public class FunctionDbVersion1Controller : ApiController
    {
        [Route("BD_STB")]
        [HttpGet]
        public async Task<IHttpActionResult> BDSTB(string Mobile, string Password,string OTP    , int NO)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("NO", OracleDbType.Int32);
            p1.Value = NO;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "BD_STB", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");

            }
            return Ok(Result);
        }
        [Route("GET_DM_DSLAM")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDMDSLAM(string Mobile, string Password,string OTP, string SSYSNAME)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SSYSNAME", OracleDbType.Varchar2);
            p1.Value = SSYSNAME;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "GET_DM_DSLAM", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("INSERT_HUY_DV")]
        [HttpGet]
        public async Task<IHttpActionResult> InsertHuyDv(string Mobile, string Password,string OTP, string SACC)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SACC", OracleDbType.Varchar2);
            p1.Value = SACC;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "INSERT_HUY_DV", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("INSERT_HUY_PORT")]
        [HttpGet]
        public async Task<IHttpActionResult> InsertHuyPort(string Mobile, string Password,string OTP, string SACC)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SACC", OracleDbType.Varchar2);
            p1.Value = SACC;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "INSERT_HUY_PORT", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("UPDATE_CSSVUNG")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateCSSVung(string Mobile, string Password,string OTP, string MADK)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("MADK", OracleDbType.Varchar2);
            p1.Value = MADK;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_CSSVUNG", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("UPDATE_DM_DSLAM")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateDmDslam(string Mobile, string Password,string OTP, int SIDDSLAM, string SIP, int SVLAN_HSI, int SVLAN_MYTV, int SVLAN_LSTV, string SUSER, string SPASS)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SIDDSLAM", OracleDbType.Int32);
            OracleParameter p2 = new OracleParameter("SIP", OracleDbType.Varchar2);
            OracleParameter p3 = new OracleParameter("SVLAN_HSI", OracleDbType.Int32);
            OracleParameter p4 = new OracleParameter("SVLAN_MYTV", OracleDbType.Int32);
            OracleParameter p5 = new OracleParameter("SVLAN_LSTV", OracleDbType.Int32);
            OracleParameter p6 = new OracleParameter("SUSER", OracleDbType.Varchar2);
            OracleParameter p7 = new OracleParameter("SPASS", OracleDbType.Varchar2);
            p1.Value = SIDDSLAM;
            p2.Value = SIP;
            p3.Value = SVLAN_HSI;
            p4.Value = SVLAN_MYTV;
            p5.Value = SVLAN_LSTV;
            p6.Value = SUSER;
            p7.Value = SPASS;
            param.Add(p1);
            param.Add(p2);
            param.Add(p3);
            param.Add(p4);
            param.Add(p5);
            param.Add(p6);
            param.Add(p7);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_DM_DSLAM", param);

            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("UPDATE_PROFILE")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateProfile(string Mobile, string Password,string OTP, int SIDDT)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SIDDT", OracleDbType.Int32);
            p1.Value = SIDDT;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_PROFILE", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [HttpGet]
        [Route("UPDATE_TO_CSSVUNG")]
        public async Task<IHttpActionResult> UpdateToCssVung(string Mobile, string Password,string OTP, string SMAKH)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SMAKH", OracleDbType.Varchar2);
            p1.Value = SMAKH;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_TO_CSSVUNG", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);

        }
        [Route("UPDATE_TDDSLAM")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateTDDSLam(string Mobile, string Password,string OTP, int SIDDT)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (!(CheckMobile is USER)) return BadRequest(CheckMobile as string);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SIDDT", OracleDbType.Int32);
            p1.Value = SIDDT;
            param.Add(p1);

            var Result = Datacs.GetData("FUNCTION_DB.", "UPDATE_TTDSLAM", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);

        }


    }
}
