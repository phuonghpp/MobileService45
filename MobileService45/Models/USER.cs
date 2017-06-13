namespace MobileService45.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Threading.Tasks;
    using MobileService45.Models;
    using System.Linq;

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

        OracleMobileDB db = new OracleMobileDB();

        
        public USER()
        {

        }

        public Task<string> NewOTPAsyn()
        {
            var temp = Guid.NewGuid().ToString().ToUpper();
            foreach (char c in temp)
            {
                if (c >= 'A' & c <= 'Z') OTP = c + OTP;
            }
            OTP = OTP.Substring(0, 10);
            TIME_OTP = DateTime.UtcNow;
            return Task.FromResult<string>(OTP);
        }

    }
}
