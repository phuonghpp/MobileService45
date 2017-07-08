using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MobileService45.DAL;
using MobileService45.AlineTest;
using System.Web.Configuration;
using MobileService45.ViewModels;

namespace MobileService45.Controllers
{
    [RoutePrefix("DoKiemVersion1")]
    public class DoKiemVersion1Controller : ApiController
    {
        [HttpGet]
        [Route("GET_DM_DSLAM")]
        public async Task<IHttpActionResult> GetDmDSLAM( string Mobile,string Password,string OTP,string sMADV)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return Content(HttpStatusCode.BadRequest, CheckMobile);
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("sMADV", OracleDbType.Varchar2);
            p1.Value = sMADV;
            param.Add(p1);
            var Result = Datacs.GetData("DONGBOSL.", "GET_DM_DSLAM", param);
            if (Result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [HttpGet]
        [Route("GET_PORT_DSLAM")]
        public async Task<IHttpActionResult> GetPortDslam(string Mobile,string Password,string OTP,string sIDDSLAM)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return Content(HttpStatusCode.BadRequest, CheckMobile);
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("sIDDSLAM", OracleDbType.Varchar2);
            p1.Value = sIDDSLAM;
            param.Add(p1);
            var Result = Datacs.GetData("DONGBOSL.", "GET_PORT_DSLAM", param);
            if (Result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("CheckAccount")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckAccountAndPort(string Mobile,string Password, string OTP,string Account)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return Content(HttpStatusCode.BadRequest, CheckMobile);
            try
            {
                ServicesSoapClient Client = new ServicesSoapClient("ServicesSoap");
                ServicesSoapClient Client2 = new ServicesSoapClient("ServicesSoap");
                string User = WebConfigurationManager.AppSettings["AlineTestAccount"];
                string PassWord = WebConfigurationManager.AppSettings["AlineTestPassword"];
                var Res = Client.GetAccount(User, PassWord, Account);
                var vcid = Res[0].VCID;
                int Position = vcid.IndexOf("/");
                string test = vcid.Substring(Position + 1, vcid.Length - Position - 1);
                vcid = vcid.Substring(0, Position);
                
                int id = int.Parse(vcid);
                string input = String.Empty;
                if (test == "11")
                {
                    input = Res[0].Ip + ":" + Res[0].Port + "/" + vcid;
                }

                else { input = Res[0].Ip + ":" + Res[0].Port; }
                var detail = Client2.TestOnePort(User, PassWord, input, 1);
                //DoKiemViewModel Tobereturn = new DoKiemViewModel(Res[0], detail.Sl2TestResults[0]);

                if (detail == null)
                {
                    return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
                }
                return Ok(detail);
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
