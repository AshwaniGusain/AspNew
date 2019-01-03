using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Contrast_Web
{
    public partial class Log_file : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string OrderId = Request.QueryString["OrderId"].ToString();
                String file_name = "\\\\192.168.1.21\\Temp\\A300Datadelivery\\Log\\MBEF" + OrderId + ".txt";                
                foreach (string line in System.IO.File.ReadAllLines(@file_name))
                {
                    lblLog.Text += line + "<br>";
                }

                //if (System.IO.File.Exists(file_name) == true)
                //{
                //    System.IO.StreamReader objReader;
                //    System.IO.File.ReadAllLines(@"\\192.168.1.21\Temp\A300Datadelivery\Log\MBEF6531.txt");
                //    foreach (string line in System.IO.File.ReadAllLines(@"\\192.168.1.21\Temp\A300Datadelivery\Log\MBEF6531.txt"))
                //        {
                //            lblLog.Text = line;
                //        }
                   
                //    //objReader.Close();

                //}
                //else
                //{
                //    ShowMessage("Error log not available");
                //}
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            } 
        }

        private void ShowMessage(string strMsg)
        {            
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), Guid.NewGuid().ToString(), "alert('" + strMsg + "');", true);
        }
    }
}
