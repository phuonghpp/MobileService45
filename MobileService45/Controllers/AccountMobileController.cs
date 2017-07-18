using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.Models;
using System.Threading.Tasks;
using MobileService45.ViewModels;
using MobileService45.DAL;

namespace MobileService45.Controllers
{
    [RoutePrefix("Account")]
    public class AccountMobileController : ApiController
    {

        [Route("ConfirmAccountAndOTP")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckAccountInformation(string Mobile,string Password,string OTP)
        {
            var User =await Datacs.FindByMobile(Mobile);
            if (User == null) return BadRequest("Không tìm thấy tài khoản tương ứng với số điện thoại ");
            if (!User.IsValidPassword(Password)) return BadRequest("Mật khẩu không chính xác");
            if (!User.IsValidOTP(OTP)) return BadRequest("OTP không chính xác");
            if (!User.IsOTPLive(OTP)) return BadRequest("OTP hết hạn sử dụng");

            return Ok(User);
            

        }
        [Route("BornToBeDeleted")]
        [HttpGet]
        public async Task<IHttpActionResult> TestObject(string Mobile)
        {
            var ToBeReturn = await FindByMobile(Mobile);
            if(ToBeReturn is USER)
            {
                var returned = ToBeReturn as USER;
                return Ok("this is user : + alive otp"+returned.ALIVE_OTP.ToString()+"thanks");
            }
            if (ToBeReturn is string)
                return Ok(ToBeReturn as string);
            return BadRequest();
        }
        private async Task<object> FindByMobile(string Mobile)
        {
            if (Mobile == "hello") return new USER();
            return "Ok";
        }

        // Register chưa có kế hoạch sử dụng, cần chỉnh sửa lại sau
        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] RegisterViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await CreateAccountAsyn(model.FullName, model.Mobile, model.Password,model.CountMe);
            if (result != "Success")
            {
                return BadRequest(result);
            }
            return Ok("Đã tạo tài khoản : "+model.Mobile+" thành công! ");

        }
        // demo Login for basic test from begin of project/ need to be removed soon!
        [HttpGet]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(string Mobile,string Password )
        {
            try
            {
                USER FindUser;
                FindUser =await Datacs.FindByMobile(Mobile);
                if (FindUser == null)
                {
                    // Mobile number doen't exit
                    return Ok(0);
                }
                if(FindUser.PASSWORD!=Password)
                {
                    // user password is incorrect
                    return Ok(1);
                }
            }
            catch
            {
                return Ok(0);
            }
            //string OTP = await User.NewOTPAsyn();
            return Ok(2);
        }

        // cần fix lại login OTP và Register vì còn trùng đoạn code khá nhiều, nên đẩy sang models USER
        // cần refactor để đạt SOLID
        [HttpGet]
        [Route("LoginGetOTP")]
        public async Task<IHttpActionResult> LoginOTP(string Mobile, string Password)
        {

            USER FindUser;
            try
            {
                
                FindUser =await Datacs.FindByMobile(Mobile); 
                if (FindUser == null)
                {
                    // Mobile number doen't exit
                    return BadRequest("Số điện thoại không đúng");
                }
                if (FindUser.PASSWORD != Password)
                {
                    // user password is incorrect
                    return BadRequest("Mật khẩu không chính xác");
                }
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
            //string OTP = await User.NewOTPAsyn();
            string tempotp = "";
            using (var db = new OracleMobileDB())
            {
                
             //   var random = Guid.NewGuid().ToString();
             //   int i = 0;

                tempotp = new Random().Next(100000, 999999).ToString();
                
                FindUser.OTP = tempotp;
                try
                {
                    var user = db.USERS.Where(x => x.ID == FindUser.ID).FirstOrDefault<USER>();
                    FindUser.OTP = tempotp;
                    FindUser.TIME_OTP = DateTime.UtcNow.AddMinutes(1 - Convert.ToInt32(user.ALIVE_OTP));
                    // vì OTP là key nên ko thay đổi được, phải drop rồi tạo mới
                    db.USERS.Remove(user);
                    db.USERS.Add(FindUser);
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    return InternalServerError(ex);
                }
                var SMSSender = new SMSSender();
                var sent = await SMSSender.Send("OTP cho Đo Kiểm Mobile :"+tempotp, FindUser.MOBILE);
            }
            return Ok("Đã Sent OTP tới "+FindUser.MOBILE);
        }
        //private bool IsValidMobile(string _Mobile)
        //{

        //    OracleMobileDB db = new OracleMobileDB();
        //    List<USER> LUsser = new List<USER>();
        //    LUsser = db.USERS.Where(x => x.MOBILE == _Mobile).ToList();
        //    db.Dispose();
        //    if (LUsser.Count == 0) return true;
        //    else return false;

        //}
        //private USER FindByMobileAsyn(string _Mobile)
        //{
        //    USER user = new USER();
        //    try
        //    {
        //        using (OracleMobileDB db = new OracleMobileDB())
        //        {
        //             user = db.USERS.Single(x => x.MOBILE == _Mobile);
        //            return (user);

        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //private USER LoginAsyn(string _Mobile, string _Password)
        //{
        //    USER user = new USER();
        //    OracleMobileDB db = new OracleMobileDB();
        //    try
        //    {
        //         user = db.USERS.Where(x => x.MOBILE == _Mobile)
        //            .Where(x=> x.PASSWORD == _Password).Single();
        //        db.Dispose();
        //        db.Database.Connection.Close();
        //        return (user);
        //    }
        //    catch
        //    {
        //        return (null);
        //    }
        //}


        // cần refactor lại code, đẩy đoạn tạo mới user sang model User
        private async Task<string> CreateAccountAsyn(string Fullname, string Mobile, string Password, string CountMe)
        {
            if (CountMe != "1234567") return "Count again";
            if (await Datacs.FindByMobile(Mobile) != null)
            {
                return "Số điện thoại đã có người sử dụng";
            }
            var User = new USER();
            using (var db = new OracleMobileDB()) {
                
                int id;
            if (db.USERS.Count() != 0)
            {
                id = db.USERS.Count() + 1;
            }
            else
            {
                id = 1;
            }
            
            //cần encrypt password sau
            User.ID = id;
            User.MOBILE = Mobile;
            User.FULLNAME = Fullname;
            User.PASSWORD = Password;
            User.MADV = "CNTT";
            User.CREATETIME = DateTime.UtcNow;
            User.TIME_OTP = DateTime.UtcNow;
            User.ALIVE_OTP = Convert.ToByte(30);
                //string temp = Guid.NewGuid().ToString().ToUpper();
                //foreach (char c in temp)
                //{
                //    if (c >= 'A' & c <= 'Z') User.OTP = c + User.OTP;
                //}
                //User.OTP = User.OTP.Substring(0, 4);
            User.OTP = new Random().Next(100000, 999999).ToString();
            try
            {
                db.USERS.Add(User);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                var debugger = ex.Message;
                return (ex.Message);
            }
                var SMSSender = new SMSSender();
                var sent = await SMSSender.Send("Đã tạo mới tài khoản Mobile Đo Kiểm thành công .Số điện thoại : "+User.MOBILE+"Mật khẩu : "+User.PASSWORD+"OneTimePassword"+  User.OTP, User.MOBILE);
            }
           
            return ("Success");


        }
    }
}
