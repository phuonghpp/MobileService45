namespace MobileService45.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APP.LOGS")]
    public partial class LOG
    {
        public decimal ID { get; set; }

        public decimal? USERID { get; set; }

        public decimal? MENUID { get; set; }

        public DateTime? LOGTIME { get; set; }

        [StringLength(100)]
        public string IP { get; set; }
    }
}
