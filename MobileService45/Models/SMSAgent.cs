using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Threading.Tasks;

namespace MobileService45.Models
{
    public class SMSSender
    {
    //'REQID	Request ID
    //'LABELID	ID của nhãn :7575
    //'TEMPLATEID	ID của mẫu tin nhắn. 32293:VNPT Thanh Hoa {P1} vao thoi diem {P2}. DTHT: {P3}
    //'ISTELCOSUB	Sử dụng nhóm thuê bao của nhà mạng. Giá trị 0 hoặc 1:   chon 0
    //'PARAMS.NUM	Số thứ tự của tham số truyền vào mẫu bản tin:   3
    //'PARAMS.CONTENT	Nội dung của tham số tương ứng
    //'CONTRACTTYPEID	Tin nhắn QC = 2, tin nhắn CSKH = 1  :1
    //'SCHEDULETIME	Đặt lịch gửi tin. Cấu trúc là : dd/MM/yyyy hh24:mi, ví dụ : 08/05/2012 16:30    để trống thì nhắn luôn
    //'MOBILELIST	Danh sách các số thuê bao cần gửi, các thuê bao phân cách bởi dấu phẩy , và không có khoảng trắng   :84912...
    //'AGENTID	ID của nhà đại lý (Vinaphone cấp): 116
    //'APIUSER	Username của API (Vinaphone cấp):VNPT-VTTH(VTTH) 
    //'APIPASS	Password của API (Vinaphone cấp):VTTH!@#321(VTTH)
    //'USERNAME	User đăng nhập của Agent:VNPT-TH-cskh
    //'CONTRACTID	ID của hợp đồng:    403
        private string REQID { get; set; }
        private string LABELID { get; set; }
        private string TEMPLATEID { get; set; }
        private string MOBILEGROUPID { get; set; }
        private string ISTELCOSUB { get; set; }
        private List<string> PARAMS { get; set; }
        private string CONTRACTTYPEID { get; set; }
        private string SCHEDULETIME { get; set; }
        private string MOBILELIST { get; set; }
        private string AGENTID { get; set; }
        private string APIUSER { get; set; }
        private string APIPASS { get; set; }
        private string USERNAME { get; set; }
        private string CONTRACTID { get; set; }
        public SMSSender(string _REQID,string _LABELID,string _TEMPLATEID,string _MOBILEGROUPID
            ,string _ISTELCOSUB,List<string> _PARAMS, string _CONTRACTTYPEID,
            string _SCHEDULETIME,string _MOBILELIST,string _AGENTID,string _APIUSER,string _APIPASS
            ,string _USERNAME,string _CONTRACTID) {
            this.REQID = _REQID;
            this.LABELID = _LABELID;
            this.TEMPLATEID = _TEMPLATEID;
            this.MOBILEGROUPID = _MOBILEGROUPID;
            this.ISTELCOSUB = _ISTELCOSUB;
            this.PARAMS = _PARAMS;
            this.CONTRACTTYPEID = _CONTRACTTYPEID;
            this.SCHEDULETIME = _SCHEDULETIME;
            this.MOBILELIST = _MOBILELIST;
            this.AGENTID = _AGENTID;
            this.APIUSER = _APIUSER;
            this.APIPASS = _APIPASS;
            this.USERNAME = _USERNAME;
            this.CONTRACTID = _CONTRACTID;//403
            
        }
        public SMSSender()
        {
            //'ISTELCOSUB	Sử dụng nhóm thuê bao của nhà mạng. Giá trị 0 hoặc 1:   chon 0
            // random reqid
            Random rnd = new Random();
            this.REQID = rnd.Next(1, 1000000000).ToString();
            this.LABELID = "35662";
            this.TEMPLATEID = "122550";
            this.MOBILEGROUPID = string.Empty;
            this.ISTELCOSUB = "0";
            this.PARAMS = new List<string>();
            this.CONTRACTID = "3090";
            this.SCHEDULETIME = string.Empty;
            this.MOBILELIST = string.Empty;
            this.AGENTID = WebConfigurationManager.AppSettings["SMSAgentID"];
            this.APIUSER = WebConfigurationManager.AppSettings["SMSApiUser"];
            this.APIPASS = WebConfigurationManager.AppSettings["SMSApiPass"];
            this.USERNAME = WebConfigurationManager.AppSettings["SMSUserName"];
            this.CONTRACTTYPEID = WebConfigurationManager.AppSettings["SMSContractId"];
        }

        public static async Task<string> Send(string _PARAMS,string MobilePhone)
        {
            //this.MOBILELIST = MobilePhone;
            //if (this.PARAMS == null)
            //{
            //    this.PARAMS = new List<string>();
            //}
            //this.PARAMS.Add(_PARAMS);
            //var xmlbody = GetXMLString();
            MobilePhone = MobilePhone[0] == '0' ? MobilePhone.Substring(1) : MobilePhone;
            var URLString = WebConfigurationManager.AppSettings["SMSGateWay"];
            int Reqid = new Random().Next(1, 1000000000);
            var testxmlbody = @"<RQST name=""send_sms_list""><REQID>"+Reqid+
                "</REQID><LABELID>35662</LABELID><TEMPLATEID>121316</TEMPLATEID>"
                + "<ISTELCOSUB>0</ISTELCOSUB><CONTRACTTYPEID>1</CONTRACTTYPEID>"
                +"<SCHEDULETIME></SCHEDULETIME><MOBILELIST>"
                +"84"+MobilePhone+"</MOBILELIST><AGENTID>116</AGENTID><APIUSER>VNPT-VTTH</APIUSER>"+
                "<APIPASS>VTTH!@#321</APIPASS><USERNAME>TH_CS</USERNAME><CONTRACTID>3090</CONTRACTID>"+
                "<PARAMS><NUM>1</NUM><CONTENT>"+_PARAMS+"</CONTENT></PARAMS></RQST>";
            var Client = new HttpClient();
            var httpContent = new StringContent(testxmlbody, Encoding.UTF8, "text/xml");
            try
            {
                 
                var response = await Client.PostAsync(URLString, httpContent);
                Client.Dispose();
                var code = response.StatusCode.ToString();
                return code;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        
        public  string GetXMLString()
        {
            var content = new StringBuilder();
            string RQST = @"<RQST name=""send_sms_list"">";
            content.Append(RQST);
            content.Append("<REQID>" + this.REQID + "</REQID>");
            content.Append("<LABELID>" + this.LABELID + "</LABELID>");
            content.Append("<TEMPLATEID>" + this.TEMPLATEID + "</TEMPLATEID>");
            content.Append("<ISTELCOSUB>" + this.ISTELCOSUB + "</ISTELCOSUB>");
            content.Append("<CONTRACTTYPEID>" + this.CONTRACTTYPEID + "</CONTRACTTYPEID>");
            content.Append("<SCHEDULETIME>" + this.SCHEDULETIME + "</SCHEDULETIME>");
            content.Append("<MOBILELIST>" + this.MOBILELIST + "</MOBILELIST>");
            content.Append("<AGENTID>" + this.AGENTID + "</AGENTID>");
            content.Append("<APIUSER>" + this.APIUSER + "</APIUSER>");
            content.Append("<APIPASS>" + this.APIPASS + "</APIPASS>");
            content.Append("<USERNAME>" + this.USERNAME + "</USERNAME>");
            content.Append("<CONTRACTID>" + this.CONTRACTTYPEID + "</CONTRACTID>");
            int run = 0;
            foreach(var s in this.PARAMS)
            {
                content.Append("<PARAMS>");
                content.Append("<NUM>"+(++run)+ "</NUM><CONTENT>"+s+ "</CONTENT>");
                content.Append("</PARAMS>");
            }
            content.Append("</RQST>");
            return content.ToString();
        }

    }
}