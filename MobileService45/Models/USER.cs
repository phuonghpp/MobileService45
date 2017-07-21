namespace MobileService45.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;

    [Table("APP.USERS")]
    public partial class USER
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string MOBILE { get; set; }

        [StringLength(50)]
        public string FULLNAME { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string PASSWORD { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string OTP { get; set; }

        public DateTime? TIME_OTP { get; set; }

        public byte? ALIVE_OTP { get; set; }

        public DateTime? CREATETIME { get; set; }

        public bool? ENABLE { get; set; }

        [StringLength(10)]
        public string MADV { get; set; }
        

        public bool IsValidPassword(string Password  )
        {
            if (this.PASSWORD == Password) return true;
            return false;
        }
        public bool IsValidOTP(string OTP)
        {
            //if (this.OTP == OTP||OTP=="0000") return true;
            if (this.OTP == OTP ) return true;
            return false;
        }
        public bool IsOTPLive(string OTP)
        {
           // if (OTP == "0000") return true;
            var TimeOTPLive = TIME_OTP ?? DateTime.UtcNow;
            TimeOTPLive = TimeOTPLive.AddMinutes(Convert.ToInt32(ALIVE_OTP));
            if (TimeOTPLive < DateTime.UtcNow) return false;
            TIME_OTP = DateTime.UtcNow;
            return true;
        }

    }
}
