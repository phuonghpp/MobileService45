using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MobileService45.DAL;


namespace MobileService45.Controllers
{
    [RoutePrefix("GIAOTIEPVersion1")]
    public class GiaoTiepVersion1Controller : ApiController
    {
        [HttpGet]
        [Route("LOAD_KH_BD")]
        public async Task<IHttpActionResult> LoadKHBD(string Mobile, string Password,string OTP, string SMADV, string SACC)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return  Content(HttpStatusCode.BadRequest, CheckMobile);

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SMADV", OracleDbType.Varchar2);
            OracleParameter p2 = new OracleParameter("SACC", OracleDbType.Varchar2);
            p1.Value = SMADV;
            p2.Value = SACC;
            param.Add(p1);
            param.Add(p2);

            var Result = Datacs.GetData("GIAOTIEP.", "LOAD_KH_BD", param);
            if (Result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);
        }
        //[HttpGet]
        //[Route("TestSelfBuild")]
        //public IHttpActionResult get()
        //{
        //    return Content(HttpStatusCode.BadRequest, "Any object");
        //}
        [HttpGet]
        [Route("LOAD_MUC_PROFILE")]
        public async Task<IHttpActionResult> LoadMucProfile(string Mobile, string Password,string OTP, string SPROFILE, string SIDTB)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return  Content(HttpStatusCode.BadRequest, CheckMobile);


            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SPROFILE", OracleDbType.Varchar2);
            OracleParameter p2 = new OracleParameter("SIDTB", OracleDbType.Varchar2);
            p1.Value = SPROFILE;
            p2.Value = SIDTB;
            param.Add(p1);
            param.Add(p2);

            var Result = Datacs.GetData("GIAOTIEP.", "LOAD_MUC_PROFILE", param);
            if (Result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);

        }
        [Route("XNBD_CAPDONG")]
        [HttpGet]
        public async Task<IHttpActionResult> XNBDCapDong(string Mobile, string Password,string OTP, int SLBD, int SIDTB, string SIDDT, string SPROFILE)
        {
            var CheckMobile = await Datacs.IsValidMobile(Mobile, Password, OTP);
            if (CheckMobile != "true") return Content(HttpStatusCode.BadRequest, CheckMobile);

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SLBD", OracleDbType.Int32);
            OracleParameter p2 = new OracleParameter("SIDTB", OracleDbType.Int32);
            OracleParameter p3 = new OracleParameter("SIDDT", OracleDbType.Varchar2);
            OracleParameter p4 = new OracleParameter("SPROFILE", OracleDbType.Varchar2);
            param.Add(p1);
            param.Add(p2);
            param.Add(p3);
            param.Add(p4);

            var Result = Datacs.GetData("GIAOTIEP.", "XNBD_CAPDONG", param);

            if (Result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Không thực hiện được yêu cầu, vui lòng xem lại thông tin");
            }
            return Ok(Result);



        }
    }
}
