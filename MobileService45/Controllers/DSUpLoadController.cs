using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MobileService45.DAL;
using Oracle.ManagedDataAccess.Client;

namespace MobileService45.Controllers
{
    public class DSUpLoadController : ApiController
    {
        [HttpGet]
        [Route("DSUpLoad")]
        public async Task<IHttpActionResult> GetDsUpLoad()
        {
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter SMADV = new OracleParameter("CNTT", OracleDbType.Varchar2);
            SMADV.Value = "CNTT";

            param.Add(SMADV);
            var datare= Datacs.GetData("DONGBOSL.", "LIST_YCXN_UPLOAD", param);
            return Ok(datare);

        }

        [HttpGet]
        [Route("XNUpload")]
        public async Task<IHttpActionResult> XNUpload(string Acc,string P_Onu, string Matt, string Pass_ONU,string ONU_ID,string NV,string TB_DCUOI)
        {
            List<OracleParameter> param = new List<OracleParameter>();
            OracleParameter lsACC = new OracleParameter("lsACC", OracleDbType.Varchar2);
            OracleParameter lsP_ONU = new OracleParameter("lsP_ONU", OracleDbType.Varchar2);
            OracleParameter sMATT = new OracleParameter("sMATT", OracleDbType.Varchar2);
            OracleParameter sPASS_ONU = new OracleParameter("sPASS_ONU", OracleDbType.Varchar2);
            OracleParameter sONU_ID = new OracleParameter("sONU_ID", OracleDbType.Varchar2);
            OracleParameter sNV = new OracleParameter("sNV", OracleDbType.Varchar2);
            OracleParameter sTB_DCUOI = new OracleParameter("sTB_DCUOI", OracleDbType.Varchar2);
            
            param.Add(lsACC);
            param.Add(lsP_ONU);
            param.Add(sMATT);
            param.Add(sPASS_ONU);
            param.Add(sONU_ID);
            param.Add(sNV);
            param.Add(sTB_DCUOI);
            var datare = Datacs.GetData("DONGBOSL.", "XN_UPLOAD", param);
            return Ok(datare);
        }
    }
}
