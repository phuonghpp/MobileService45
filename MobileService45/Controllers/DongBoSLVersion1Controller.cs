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
    [RoutePrefix("DongBoSLV1")]
    public class DongBoSLVersion1Controller : ApiController
    {
        [Route("ADD_DEL_PASS")]
        [HttpGet]
        public async Task<IHttpActionResult> AddDelPass(string Mobile, string Password,string OTP, string P_NhanVien, string P_Account, Int64 P_Slot, Int64 P_Port, Int64 P_Vpi)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP))
            {
                return Unauthorized();
            }
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("P_NHANVIEN", OracleDbType.Varchar2);
            p1.Value = P_NhanVien;
            OracleParameter p2 = new OracleParameter("P_ACCOUNT", OracleDbType.Varchar2);
            p2.Value = P_Account;
            OracleParameter p3 = new OracleParameter("P_SLOT", OracleDbType.Int64);
            p3.Value = P_Slot;
            OracleParameter p4 = new OracleParameter("P_PORT", OracleDbType.Int64);
            p4.Value = P_Port;
            OracleParameter p5 = new OracleParameter("P_VPI", OracleDbType.Int64);
            p5.Value = P_Vpi;
            param.Add(p1);
            param.Add(p2);
            param.Add(p3);
            param.Add(p4);
            param.Add(p5);

            var result = Datacs.GetData("DONGBOSL.", "ADD_DEL_PASS", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Route("GET_DICHVU_BY_ACCOUNT")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDichVuByAccount(string Mobile, string Password,string OTP, string P_Account)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("P_ACCOUNT", OracleDbType.Varchar2);
            p1.Value = P_Account;
            param.Add(p1);
            var result = Datacs.GetData("DONGBOSL.", "GET_DICHVU_BY_ACCOUNT", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Route("GET_DSKH")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDSKH(string Mobile, string Password,string OTP, string Smadv, string skey)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SMADV", OracleDbType.Varchar2);
            OracleParameter p2 = new OracleParameter("SKEY", OracleDbType.Varchar2);
            p1.Value = Smadv;
            p2.Value = skey;
            param.Add(p1);
            param.Add(p2);
            var result = Datacs.GetData("DONGBOSL.", "GET_DSKH", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Route("GET_LOG_BY_ACCOUNT")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLogByAccount(string mobile, string password,string OTP, string P_Account)
        {
            if (!await Datacs.IsValidMobile(mobile, password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("P_ACCOUNT", OracleDbType.Varchar2);
            p1.Value = P_Account;
            param.Add(p1);
            var result = Datacs.GetData("DONGBOSL.", "GET_LOG_BY_ACCOUNT", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Route("LIST_TBDC_KH")]
        [HttpGet]
        public async Task<IHttpActionResult> ListTBDCKH(string Mobile, string Password,string OTP, string SDevice)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SDEVICE", OracleDbType.Varchar2);
            p1.Value = SDevice;
            param.Add(p1);
            var result = Datacs.GetData("DONGBOSL.", "LIST_TBDC_KH", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        [Route("LIST_TBYC_UPLOAD")]
        [HttpGet]
        public async Task<IHttpActionResult> ListTBYCUpload(string Mobile, string Password,string OTP, string Smatt, string Stt_Dslam)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SMATT", OracleDbType.Varchar2);
            OracleParameter p2 = new OracleParameter("STT_DSLAM", OracleDbType.Varchar2);
            p1.Value = Smatt;
            p2.Value = Stt_Dslam;
            param.Add(p1);
            param.Add(p2);
            var result = Datacs.GetData("DONGBOSL.", "LIST_TBYC_UPLOAD", param);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);

        }
        [Route("LIST_YCXN_UPLOAD")]
        [HttpGet]
        public async Task<IHttpActionResult> ListYCXNUpload(string Mobile, string Password,string OTP)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            var user =await Datacs.FindByMobile(Mobile);

            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("SMADV", OracleDbType.Varchar2);
            p1.Value = user.MADV;
            param.Add(p1);
            var Result = Datacs.GetData("DONGBOSL.", "LIST_YCXN_UPLOAD", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }
        [Route("XN_UPLOAD")]
        [HttpGet]
        public async Task<IHttpActionResult> XNUpload(string Mobile, string Password,string OTP, string Lsacc, string Lsp_Onu, string Smatt, string Spass_Onu, string Sonu_Id, string SNV, string Stb_Dcoui)
        {
            if (!await Datacs.IsValidMobile(Mobile, Password,OTP)) return Unauthorized();
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter p1 = new OracleParameter("LSACC", OracleDbType.Varchar2);
            OracleParameter p2 = new OracleParameter("LSP_ONU", OracleDbType.Varchar2);
            OracleParameter p3 = new OracleParameter("SMATT", OracleDbType.Varchar2);
            OracleParameter p4 = new OracleParameter("SPASS_ONU", OracleDbType.Varchar2);
            OracleParameter p5 = new OracleParameter("SONU_ID", OracleDbType.Varchar2);
            OracleParameter p6 = new OracleParameter("SNV", OracleDbType.Varchar2);
            OracleParameter p7 = new OracleParameter("STB_DCOUI", OracleDbType.Varchar2);
            p1.Value = Lsacc;
            p2.Value = Lsp_Onu;
            p3.Value = Smatt;
            p4.Value = Spass_Onu;
            p5.Value = Sonu_Id;
            p6.Value = SNV;
            p7.Value = Stb_Dcoui;
            param.Add(p1);
            param.Add(p2);
            param.Add(p3);
            param.Add(p4);
            param.Add(p5);
            param.Add(p6);
            param.Add(p7);
            var Result = Datacs.GetData("DONGBOSL.", "XN_UPLOAD", param);
            if (Result == null)
            {
                return BadRequest();
            }
            return Ok(Result);
        }
    }
}
