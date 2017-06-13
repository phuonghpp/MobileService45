namespace MobileService45.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("APP.USER_GROUP")]
    public partial class USER_GROUP
    {
        public decimal ID { get; set; }

        public decimal? USERID { get; set; }

        public decimal? GROUPID { get; set; }
    }
}
