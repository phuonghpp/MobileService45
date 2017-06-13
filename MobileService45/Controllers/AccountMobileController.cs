using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MobileService45.Models;
using System.Threading.Tasks;
using MobileService45.ViewModels;


namespace MobileService45.Controllers
{
    [RoutePrefix("Account")]
    public class AccountMobileController : ApiController
    {
        OracleMobileDB db = new OracleMobileDB();

        [Route("Register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] RegisterViewModel model )
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new USER();
            var result = await CreateAccountAsyn(model.UserName, model.Mobile, model.Password);
            if (result == false)
            {
                if (IsValidMobile(model.Mobile))
                {
                    return BadRequest("Số điện thoại đã được đăng ký, vui lòng nhập số mới ");
                }
                return BadRequest("Đang có lỗi hệ thống chưa tạo mới được tài khoản, vui lòng thử lại sau");
            }

            return Ok("Đã tạo tài khoản mới thành công! ");

        }
        [HttpGet]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(string _Mobile,string _Password )
        {
            var User = new USER();
            try
            {
               
                var FindUser = await FindByMobileAsyn(_Mobile);
                if (FindUser == null)
                {
                    // Mobile number doen't exit
                    return Ok(0);
                }
                User = await LoginAsyn(_Mobile, _Password);
                if (User == null)
                {
                    // user password is incorrect
                    return Ok(1);
                }
            }
            catch
            {
                return Ok(0);
            }
            string OTP = await User.NewOTPAsyn();
            return Ok(2);
        }
        private bool IsValidMobile(string _Mobile)
        {


            var user = db.USERS.Where(x => x.MOBILE == _Mobile).Count();

            if (user == 0) return true;
            else return false;

        }
        private Task<USER> FindByMobileAsyn(string _Mobile)
        {
            try
            {
                var user = db.USERS.Where(x => x.MOBILE == _Mobile).First();

                return Task.FromResult<USER>(user);
            }
            catch
            {
                return Task.FromResult<USER>(null);
            }
        }
        private Task<USER> LoginAsyn(string _Mobile, string _Password)
        {
            try
            {
                var user = db.USERS.Where(x => x.MOBILE == _Mobile & x.PASSWORD == _Password).FirstOrDefault();
                return Task.FromResult<USER>(user);
            }
            catch
            {
                return Task.FromResult<USER>(null);
            }
        }
        public Task<bool> CreateAccountAsyn(string _FullName, string _Mobile, string _Password)
        {
            var User = new USER();
            if (db.USERS.Where(x => x.MOBILE == _Mobile).Count() != 0) return Task.FromResult<bool>(false);
            if (db.USERS != null)
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
                return Task.FromResult<bool>(false);
            }
            return Task.FromResult<bool>(true);


        }
    }
}
