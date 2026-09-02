using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace supershop
{
 
    /// <summary>
    /// ///////////
    /// Author : Tuaha
    /// Country: Canada
    /// </summary>
    public static class UserInfo
    {
            public static string Userid { get; set; }
            public static string UserName { get; set; }
            public static string UserPassword { get; set; }
            public static string usertype { get; set; }
            public static string invoiceNo { get; set; }
            public static string Shopid { get; set; }
            public static string usernamWK { get; set; }
    }

    public static class ReportValue  // use in report
    {
        public static string StartDate { get; set; }
        public static string EndDate { get; set; }
        public static string emp { get; set; }
       // public static string Reportid { get; set; }
        public static string Terminal { get; set; }
        //public static string StartDateGroupby { get; set; }
        //public static string EndDateGroupby { get; set; }
    }

    public static class parameter
    {
        public static string helpid { get; set; }
        public static string peopleid { get; set; }
        public static string autoprintid { get; set; }
        
    }
        /// <summary>VAT / discount rate of the current shop (tbl_terminallocation).</summary>
        public static class vatdisvalue
        {
            public static string vat
            {
                get
                {
                    return DataAccess.ExecuteSQLScaler("select VAT from tbl_terminallocation where Shopid = @s",
                        DataAccess.P("@s", UserInfo.Shopid));
                }
            }

            public static string dis
            {
                get
                {
                    return DataAccess.ExecuteSQLScaler("select Dis from tbl_terminallocation where Shopid = @s",
                        DataAccess.P("@s", UserInfo.Shopid));
                }
            }
        }
}
