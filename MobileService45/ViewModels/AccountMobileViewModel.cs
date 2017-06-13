using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MobileService45.ViewModels
{
    public class AccountMobileViewModel
    {

    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(50,ErrorMessage ="Tên đăng nhập giới hạn 50 ký tự ")]
        public string UserName { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Cần điền đúng định dạng của điện thoại")]
        public string Mobile { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Password cần 8-20 ký tự", MinimumLength = 8)]
        public string Password { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Cần điền đúng định dạng của điện thoại")]
        public string Mobile { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Password cần 8-20 ký tự", MinimumLength = 8)]
        public string Password { get; set; }
    }
    // ViewModel to validate data
    public class GetMytvViewModel
    {
        [StringLength(10, MinimumLength = 10, ErrorMessage = "OTP phải có 10 ký tự")]
        public string OTP { get; set; }

        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    // ViewModel to validate data
    public class GetInforViewModel
    {
        [StringLength(10, MinimumLength = 10, ErrorMessage = "OTP phải có 10 ký tự")]
        public string OTP { get; set; }

        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}