using supershop.AdminPanelForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using howto_control_print_preview;

namespace supershop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
              Application.Run(new Login());
          // // Application.Run(new BarCode.Barcode_machine());
          ////  Application.Run(new  Items.SRtaxcalc());
          //     UserInfo.UserName = "a";
          //     UserInfo.usertype = "1"; // 1= admin
          //     UserInfo.Shopid = "MTQC02";
          //     Application.Run(new SalesRegister());
          // Application.Run(new Home());
        }
    }
}
