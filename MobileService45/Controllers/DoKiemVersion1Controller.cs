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
                return BadRequest(ex.Message);
            }
        }
        private ServiceTestData2 ChangeRateToString(ServiceTestData2 data)
        {
            if (data.GponTestResults != null)
            {
                var DicGpon = new Dictionary<string, int>();
                DicGpon.Add("Kém", 5);
                DicGpon.Add("Đạt", 4);
                DicGpon.Add("Tốt", 3);
                foreach(var d in DicGpon)
                {
                    if (data.GponTestResults[0].QualityAttenuationDown == d.Value.ToString())
                    {
                        data.GponTestResults[0].QualityAttenuationDown = d.Key;
                    }
                    if (data.GponTestResults[0].QualityAttenuationUp == d.Value.ToString())
                    {
                        data.GponTestResults[0].QualityAttenuationUp = d.Key;
                    }
                    if (data.GponTestResults[0].QualityBitDown == d.Value.ToString())
                    {
                        data.GponTestResults[0].QualityBitDown = d.Key;
                    }
                    if (data.GponTestResults[0].QualityBitUp == d.Value.ToString())
                    {
                        data.GponTestResults[0].QualityBitUp = d.Key;
                    }
                }

                //var ChatLuongSuyHaoXuong = data.GponTestResults[0].QualityAttenuationDown;
                //switch (ChatLuongSuyHaoXuong)
                //{
                //    case "5":
                //        data.GponTestResults[0].QualityAttenuationDown = "Kém";
                //        break;
                //    case "4":
                //        data.GponTestResults[0].QualityAttenuationDown = "Đạt";
                //        break;
                //    case "3":
                //        data.GponTestResults[0].QualityAttenuationDown = "Tốt";
                //        break;

                //}
                //var ChatLuongSuyHaoLen = data.GponTestResults[0].QualityAttenuationUp;
                //switch (ChatLuongSuyHaoLen)
                //{
                //    case "5":
                //        data.GponTestResults[0].QualityAttenuationUp = "Kém";
                //        break;
                //    case "4":
                //        data.GponTestResults[0].QualityAttenuationUp = "Đạt";
                //        break;
                //    case "3":
                //        data.GponTestResults[0].QualityAttenuationUp = "Tốt";
                //        break;

                //}
                //var ChatLuongLoiBitXuog = data.GponTestResults[0].QualityBitDown;
                //switch (ChatLuongSuyHaoLen)
                //{
                //    case "5":
                //        data.GponTestResults[0].QualityBitDown = "Kém";
                //        break;
                //    case "4":
                //        data.GponTestResults[0].QualityBitDown = "Đạt";
                //        break;
                //    case "3":
                //        data.GponTestResults[0].QualityBitDown = "Tốt";
                //        break;

                //}
                //var ChatLuongLoiBitLen = data.GponTestResults[0].QualityBitUp;
                //switch (ChatLuongSuyHaoLen)
                //{
                //    case "5":
                //        data.GponTestResults[0].QualityBitUp = "Kém";
                //        break;
                //    case "4":
                //        data.GponTestResults[0].QualityBitUp = "Đạt";
                //        break;
                //    case "3":
                //        data.GponTestResults[0].QualityBitUp = "Tốt";
                //        break;

                //}
                return data;
            }
            if (data.XdslTestResults != null)
            {
                //var ChatLuongXuong = data.XdslTestResults[0].DownQuality;
                //var ChatLuongLen = data.XdslTestResults[0].UpQuality;
                var DicXdsl = new Dictionary<string, int>();
                DicXdsl.Add("Kém", 5);
                DicXdsl.Add("Đạt", 4);
                DicXdsl.Add("Tốt", 3);
                DicXdsl.Add("Rất tốt", 2);
                DicXdsl.Add("Xuất sắc", 1);
                foreach(var d in DicXdsl)
                {
                    if (data.XdslTestResults[0].DownQuality == d.Value.ToString())
                    {
                        data.XdslTestResults[0].DownQuality = d.Key;
                    }
                    if (data.XdslTestResults[0].UpQuality == d.Value.ToString())
                    {
                        data.XdslTestResults[0].UpQuality = d.Key;
                    }
                }
                //switch (ChatLuongXuong)
                //{
                //    case "5":
                //        data.XdslTestResults[0].DownQuality = "Kém";
                //        break;
                //    case "4":
                //        data.XdslTestResults[0].DownQuality = "Đạt";
                //        break;
                //    case "3":
                //        data.XdslTestResults[0].DownQuality = "Tốt";
                //        break;
                //    case "2":
                //        data.XdslTestResults[0].DownQuality = "Rất tốt";
                //        break;
                //    case "1":
                //        data.XdslTestResults[0].DownQuality = "Xuất sắc";
                //        break;
                //}
                //var ChatLuongLen = data.XdslTestResults[0].UpQuality;
                //switch (ChatLuongLen)
                //{
                //    case "5":
                //        data.XdslTestResults[0].UpQuality = "Kém";
                //        break;
                //    case "4":
                //        data.XdslTestResults[0].UpQuality = "Đạt";
                //        break;
                //    case "3":
                //        data.XdslTestResults[0].UpQuality = "Tốt";
                //        break;
                //    case "2":
                //        data.XdslTestResults[0].UpQuality = "Rất tốt";
                //        break;
                //    case "1":
                //        data.XdslTestResults[0].UpQuality = "Xuất sắc";
                //        break;
                //}
                return data;
            }
            if (data.Sl2TestResults != null)
            {
                var DicSL2 = new Dictionary<string, int>();
                DicSL2.Add("Tốt", 3);
                DicSL2.Add("Đạt", 2);
                DicSL2.Add("Kém", 1);
                //var ChatLuongXuong = data.Sl2TestResults[0].DownRateQuality;
                //var ChatLuongLen = data.Sl2TestResults[0].UpRateQuality;
                foreach ( var dic in DicSL2)
                {
                    if(data.Sl2TestResults[0].DownRateQuality == dic.Value.ToString())
                    {
                        data.Sl2TestResults[0].DownRateQuality = dic.Key;
                    }
                    if (data.Sl2TestResults[0].UpRateQuality == dic.Value.ToString())
                    {
                        data.Sl2TestResults[0].UpRateQuality = dic.Key;
                    }
                }
                //switch (ChatLuongXuong)
                //{
                //    case "3":
                //        data.Sl2TestResults[0].DownRateQuality = "Tốt";
                //        break;
                //    case "2":
                //        data.Sl2TestResults[0].DownRateQuality = "Đạt";
                //        break;
                //    case "1":
                //        data.Sl2TestResults[0].DownRateQuality = "Kém";
                //        break;
                //}
               
                //switch (ChatLuongLen)
                //{
                //    case "3":
                //        data.Sl2TestResults[0].UpRateQuality = "Tốt";
                //        break;
                //    case "2":
                //        data.Sl2TestResults[0].UpRateQuality = "Đạt";
                //        break;
                //    case "1":
                //        data.Sl2TestResults[0].UpRateQuality = "Kém";
                //        break;
                //}
                return data;
            }

            return data;
        }
    }
}
