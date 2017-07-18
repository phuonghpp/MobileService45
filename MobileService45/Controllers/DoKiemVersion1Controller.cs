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
            if (CheckMobile != "true") return BadRequest(CheckMobile);
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("sMADV", OracleDbType.Varchar2);
            p1.Value = sMADV;
            param.Add(p1);
            var Result = Datacs.GetData("DONGBOSL.", "GET_DM_DSLAM", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [HttpGet]
        [Route("GET_PORT_DSLAM")]
        public async Task<IHttpActionResult> GetPortDslam(string Mobile,string Password,string OTP,string sIDDSLAM)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return BadRequest(CheckMobile);
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("sIDDSLAM", OracleDbType.Varchar2);
            p1.Value = sIDDSLAM;
            param.Add(p1);
            var Result = Datacs.GetData("DONGBOSL.", "GET_PORT_DSLAM", param);
            if (Result == null)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        [Route("CheckAccount")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckAccountAndPort(string Mobile,string Password, string OTP,string Account)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return BadRequest(CheckMobile);
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
                    return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
                }
                detail = ChangeRateToString(detail);
                return Ok(detail);
            }
            catch(Exception ex)
            {
                return BadRequest("Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
        }
        private ServiceTestData2 ChangeRateToString(ServiceTestData2 data)
        {
            if (data.GponTestResults != null)
            {
                var DicGpon = new Dictionary<string, string>();
                DicGpon.Add("5","Kém");
                DicGpon.Add("4","Đạt");
                DicGpon.Add("3","Tốt");
                try
                {
                    data.GponTestResults[0].QualityAttenuationDown = DicGpon[data.GponTestResults[0].QualityAttenuationDown];
                    data.GponTestResults[0].QualityAttenuationUp = DicGpon[data.GponTestResults[0].QualityAttenuationUp];
                    data.GponTestResults[0].QualityBitDown = DicGpon[data.GponTestResults[0].QualityBitDown];
                    data.GponTestResults[0].QualityBitUp = DicGpon[data.GponTestResults[0].QualityBitUp];
                }
                catch(Exception ex)
                {
                    string debugmessage = ex.Message;
                }
                return data;
            }
            if (data.XdslTestResults != null)
            {
                var DicXdsl = new Dictionary<string, string>();
                DicXdsl.Add("5","Kém");
                DicXdsl.Add("4","Đạt");
                DicXdsl.Add("3","Tốt");
                DicXdsl.Add("2","Rất tốt");
                DicXdsl.Add("1","Xuất sắc");

                try
                {
                    data.XdslTestResults[0].DownQuality = DicXdsl[data.XdslTestResults[0].DownQuality];
                    data.XdslTestResults[0].UpQuality = DicXdsl[data.XdslTestResults[0].UpQuality];

                }
                catch(Exception ex)
                {
                    string debug = ex.Message;
                }
             
                return data;
            }
            if (data.Sl2TestResults != null)
            {
                var DicSL2 = new Dictionary<string, string>();
                DicSL2.Add("3","Tốt");
                DicSL2.Add("2","Đạt");
                DicSL2.Add("1","Kém");
                  try
                {
                    data.Sl2TestResults[0].DownRateQuality = DicSL2[data.Sl2TestResults[0].DownRateQuality];
                    data.Sl2TestResults[0].UpRateQuality = DicSL2[data.Sl2TestResults[0].UpRateQuality];
                }
                catch(Exception ex)
                {
                    string debug = ex.Message;
                }
               
                return data;
            }

            return data;
        }
    }
}
