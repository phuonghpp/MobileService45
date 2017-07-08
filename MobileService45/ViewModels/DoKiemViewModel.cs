using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MobileService45.AlineTest;

namespace MobileService45.ViewModels
{
    public class DoKiemViewModel
    {
         public   CustomerAccountInfo AccountInfor { get; set; }
         public   ServiceTestData2 Detail { get; set; }
         public   DoKiemViewModel(CustomerAccountInfo acc,ServiceTestData2 detail)
        {
            this.AccountInfor = acc;
            this.Detail = detail;
        }
    }
}