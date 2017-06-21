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
            if (User.PASSWORD != Password) return BadRequest("Mật khẩu không chính xác");
            if (User.OTP != OTP) return BadRequest("OTP không đúng");
            //if (User.TIME_OTP <= DateTime.UtcNow.AddMinutes(30)) return BadRequest("OTP time out");

            return Ok(User);
            

        }


        [Route("Register")]
        [HttpPost]
        private async Task<IHttpActionResult> Register([FromBody] RegisterViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result =  CreateAccountAsyn(model.FullName, model.Mobile, model.Password);
            if (result == false)
            {
                if (Datacs.FindByMobile(model.Mobile)!=null)
                {
                    return BadRequest("Số điện thoại đã được đăng ký, vui lòng nhập số mới ");
                }
                return BadRequest("Đang có lỗi hệ thống chưa tạo mới được tài khoản, vui lòng thử lại sau");
            }

            return Ok("Đã tạo tài khoản mới thành công! ");

        }
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
                    return BadRequest("Mobile doen't exit");
                }
                if (FindUser.PASSWORD != Password)
                {
                    // user password is incorrect
                    return BadRequest("Password is incorrect");
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
                //var UserList = db.USERS.Where(x => x.ID == FindUser.ID).ToList<USER>();
                //USER UserToUpdate = UserList.Single<USER>();
                var random = Guid.NewGuid().ToString();
                int i = 0;
                foreach( char c in random)
                {
                    i = (c % 26)+65;
                    tempotp += (char)i;
                }
                tempotp = tempotp.Substring(0, 10);
                
                FindUser.OTP = tempotp;
                var user = db.USERS.Where(x => x.ID == FindUser.ID).Single<USER>();
                db.USERS.Remove(user);
                db.USERS.Add(FindUser);
                db.SaveChanges();
            }
            return Ok(tempotp);
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
        private bool CreateAccountAsyn(string _FullName, string _Mobile, string _Password)
        {
            OracleMobileDB db = new OracleMobileDB();
            var User = new USER();
            if (db.USERS.Where(x => x.MOBILE == _Mobile).Count() != 0) return (false);
            if (db.USERS.Count() == 0)
            {
                User.ID = db.USERS.Count() + 1;
            }
            else
            {
                User.ID = 1;
            }
            //cần encrypt password sau
            User.MOBILE = _Mobile;
            User.FULLNAME = _FullName;
            User.PASSWORD = _Password;
            User.CREATETIME = DateTime.UtcNow;
            User.TIME_OTP = DateTime.UtcNow;
            string temp = Guid.NewGuid().ToString().ToUpper();
            foreach (char c in temp)
            {
                if (c >= 'A' & c <= 'Z') User.OTP = c + User.OTP;
            }
            User.OTP = User.OTP.Substring(0, 10);
            try
            {
                db.USERS.Add(User);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return (false);
            }
            return (true);


        }
    }
}
