using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ExceptionHandler.ExceptionHandlers;
using System.IO;
using System.Drawing;
using System.Net;
using System.Configuration;
using System.Text;
using WinSCP;
using System.Web.Mail;



namespace Contrast_Web
{
    public partial class frmChangeProcess : System.Web.UI.Page
    {
        private const int BUFFERSIZE = 4096;
        //protected string userName = "cb1790";
        //protected string userName = Global.Validate();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    fillDetails();                    
                    btnUpdate.Visible = false;
                    Export_Link.Visible = false;
                }
                
                
            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = UserInterfaceExceptionHandler.HandleExcetion(ref ex);

                ShowMessage(ex.Message);
            }
        }

        private void fillDetails()
        {
            try
            {
                DataSet ds = DBLibrary.GetStageCustomerProcess();

              
                ddlStage.DataSource = ds.Tables[0];
                ddlStage.DataTextField = "StageName";
                ddlStage.DataValueField = "StageID";
                ddlStage.DataBind();
                ddlStage.Items.Insert(0, new ListItem("ALL", "0"));
                ddlProcess.DataSource = ds.Tables[1];
                ddlProcess.DataTextField = "ProcessName";
                ddlProcess.DataValueField = "ProcessID";
                ddlProcess.DataBind();
                ddlProcess.Items.Insert(0, new ListItem("ALL", "0"));               
            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = UserInterfaceExceptionHandler.HandleExcetion(ref ex);

                ShowMessage(ex.Message);
            }
        }

      

        public void GridProcessBind()
        {

            DataTable dtWip = DBLibrary.GetWIPDetails( Convert.ToInt32(ddlStage.SelectedValue), Convert.ToInt32(ddlProcess.SelectedValue));

            GVChangeProcess.DataSource = dtWip;
            GVChangeProcess.DataBind();

            if (dtWip.Rows.Count > 0)
            {
                //btnUpdate.Visible = true;
                Export_Link.Visible = true;
            }
            else
            {
                //btnUpdate.Visible = false;
                Export_Link.Visible = false;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ddlStage.SelectedValue.ToString() == "0")
                //{
                //    ShowMessage("Please select stage");
                //    return;
                //}
                GridProcessBind();
            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = UserInterfaceExceptionHandler.HandleExcetion(ref ex);

                ShowMessage(ex.Message);
            }
        }

        protected void GVChangeProcess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkupload = (CheckBox)e.Row.FindControl("chkupload");

                string remark = ((DataRowView)e.Row.DataItem)["Remarks"].ToString();
                string Dispatch = ((DataRowView)e.Row.DataItem)["DispatchDate"].ToString();

                bool id = (bool)((DataRowView)e.Row.DataItem)["NDMSIGNAL"];

                if (remark == "Zipping is completed" && id == true && Dispatch == "")
                {

                    chkupload.Enabled = true;
                }
            }
        }
        protected void chkChangeProcess_CheckedChanged(object sender, EventArgs e)
        {
            //btnupload.Visible = true;
            //btnUpdate.Visible = true;
        }
        protected void chkupload_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            CheckBox chkPr;
            int count = 0;
            try
            {
                
                foreach (GridViewRow gvr in GVChangeProcess.Rows)
                {
                   chkPr = (CheckBox)gvr.FindControl("chkChangeProcess");
                    if (chkPr.Checked == true)
                    {
                        int ProcessId = 1;
                        int ID = Convert.ToInt32(GVChangeProcess.DataKeys[gvr.RowIndex].Values["ID"]);
                        int result = DBLibrary.UpdateProcess(ID, ProcessId);
                        count = count + 1;
                        chkPr.Checked = false;
                        chkPr.Enabled = true;

                    }
                }
                if (count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Updation Successful')", true);
                    
                }
                else
                { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select an Item To REintiate')", true); }

                GridProcessBind();

            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = UserInterfaceExceptionHandler.HandleExcetion(ref ex);

                ShowMessage(ex.Message);
            }
        }
        protected void btnupload_Click(object sender, EventArgs e)
        {
            CheckBox chkPr;
            int count = 0;
            try
            {
                foreach (GridViewRow gvr in GVChangeProcess.Rows)
                {
                    chkPr = (CheckBox)gvr.FindControl("chkupload");
                    if (chkPr.Checked == true)
                    {
                        //int ProcessId = 1;
                        int ID = Convert.ToInt32(GVChangeProcess.DataKeys[gvr.RowIndex].Values["ID"]);
                        string ZipDestinationpath = GVChangeProcess.DataKeys[gvr.RowIndex].Values["ZipDestinationPath"].ToString();
                        string article = GVChangeProcess.DataKeys[gvr.RowIndex].Values["article"].ToString();
                        string Journal = GVChangeProcess.DataKeys[gvr.RowIndex].Values["journal"].ToString();
                        string Zip = GVChangeProcess.DataKeys[gvr.RowIndex].Values["ZipFileName"].ToString();
                        string Version = GVChangeProcess.DataKeys[gvr.RowIndex].Values["stagedisplayname"].ToString();

                        bool StatusZip = ZipUpload_FTP(ZipDestinationpath, Zip, Version);
                        if (StatusZip)
                        {
                            bool StatusReady = ReadyXMLUpload_FTP(ZipDestinationpath, Zip, Version, ID, article, Journal);
                            if (StatusReady)
                            {

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Upload Successful for " + Zip + "')", true);
                                chkPr.Checked = false;
                                chkPr.Enabled = true;
                            }
                        }
                       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Not Uploaded for " + Zip + "')", true);
                        count = count + 1;

                    }
                }
                if (count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('All Uploades Successful')", true);
                    
                }
                else
                { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select an Item To Upload')", true); }

                GridProcessBind();

            }
            catch (Exception ex)
            {
                bool rethrow = false;
                rethrow = UserInterfaceExceptionHandler.HandleExcetion(ref ex);

                ShowMessage(ex.Message);
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void Export_Link_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GVChangeProcess.AllowPaging = false;
                GridProcessBind();
                //Change the Header Row back to white color
                GVChangeProcess.HeaderRow.Style.Add("background-color", "#FFFFFF");
                //Applying stlye to gridview header cells
                for (int i = 0; i < GVChangeProcess.HeaderRow.Cells.Count; i++)
                {
                    GVChangeProcess.HeaderRow.Cells[i].Style.Add("background-color", "#294a77");
                }
                // Hides the first column in the grid (zero-based index)
                GVChangeProcess.HeaderRow.Cells[9].Visible = false;
                GVChangeProcess.HeaderRow.Cells[10].Visible = false;

                // Loop through the rows and hide the cell in the first column
                for (int i = 0; i < GVChangeProcess.Rows.Count; i++)
                {
                    GridViewRow row = GVChangeProcess.Rows[i];
                    row.Cells[9].Visible = false;
                    row.Cells[10].Visible = false;
                }

                GVChangeProcess.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();               
            }
            catch (Exception err)
            {
                ShowMessage(err.Message);
            }
        }

        private void ShowMessage(string strMsg)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), Guid.NewGuid().ToString(), "alert('" + strMsg + "');", true);
        }

        public bool UploadToSFTP(string filename,string url,string username,string password,string spath,string stopath,string hostkey)  
        {  
        try  
         {  
            //get static value from App.config file.  
            //string ftpServerIP = ConfigurationSettings.AppSettings["sftpServerIP"].ToString();  
            //string stringsFtpUserID = ConfigurationSettings.AppSettings["sftpUserID"].ToString();  
            //string stringsFtpPassword = ConfigurationSettings.AppSettings["sftpPassword"].ToString();  
            string stringStrDate = System.DateTime.Now.ToString("dd_MM_yyyy-hh_mm_ss");  
            string stringFileName = filename;  
            //string stringFromPath = ConfigurationSettings.AppSettings["sFromPath"].ToString();  
            //string stringToPath = ConfigurationSettings.AppSettings["sToPath"].ToString();  
            //string stringHostKey = ConfigurationSettings.AppSettings["sHostKey"].ToString();  

          string ftpServerIP = url;
         string  stringsFtpUserID = username;
         string stringsFtpPassword = password;
         string stringFromPath = spath;
         string stringToPath = stopath;

            //string stringsBackUpFolder = "Processed";  
            //create folder for back up data  
            if (!Directory.Exists(stringFromPath))  
            {  
                 // Directory.CreateDirectory(stringFromPath + stringsBackUpFolder);  
            }  
            //check whether file exist or not in local machine.  
            if (!System.IO.File.Exists(stringFromPath))  
            {  
                 // using (FileStream fileStreamLocalFile = File.Create(stringFromPath + stringFileName))  
                  //{  
                        //byte[] byteLocalFile = new UTF8Encoding(true).GetBytes(filename);  
                        //fileStreamLocalFile.Write(byteLocalFile, 0, byteLocalFile.Length);  
                 // }  
            }

            


                SessionOptions sessionOptionsSFTP = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = ftpServerIP,
                    UserName = stringsFtpUserID,
                    Password = stringsFtpPassword,
                    PortNumber = 22,
                    SshHostKeyFingerprint = hostkey
                };

            
            WinSCP.Session sessionSFTP = new WinSCP.Session();  
            sessionSFTP.Open(sessionOptionsSFTP);  
            TransferOptions transferOptionsSFTP = new TransferOptions();  
            transferOptionsSFTP.TransferMode = TransferMode.Binary;  
            transferOptionsSFTP.FilePermissions = null;  
            transferOptionsSFTP.PreserveTimestamp = false;  
            transferOptionsSFTP.ResumeSupport.State = TransferResumeSupportState.Off;  
            TransferOperationResult transferOperationResultSFTP;

            transferOperationResultSFTP = sessionSFTP.PutFiles(stringFromPath + filename, filename, false, transferOptionsSFTP);  
            if (System.IO.File.Exists(stringFromPath + stringFileName))  
            {  
                 // File.Move(stringFromPath + stringFileName, stringFromPath + "\\" + stringsBackUpFolder + "\\" + stringFileName);  
            }  
            transferOperationResultSFTP.Check(); 
 
            if (transferOperationResultSFTP.IsSuccess == true)  
            {  
                  Console.Write("File upload successfully");
                  report("ZipFTP", filename, "Uploaded TO FTP__" + stopath + "");
                  return true;
                  
            }  
            else  
            {  
                  Console.Write("File upload failed");
                  report("ZipFTP_FTP_Error", filename, "File upload failed");
                  return false;
            }  
      }  

      catch (Exception exError)  
      {  
            Console.Write(exError.Message);
            report("ZipFTP_FTP_Error", filename, exError.ToString());
            return false;
      }  
}
        public bool UploadToSFTP(string filename, string url, string username, string password, string spath, string stopath, string hostkey,string article,string journal)
        {
            try
            {
                //get static value from App.config file.  
                //string ftpServerIP = ConfigurationSettings.AppSettings["sftpServerIP"].ToString();  
                //string stringsFtpUserID = ConfigurationSettings.AppSettings["sftpUserID"].ToString();  
                //string stringsFtpPassword = ConfigurationSettings.AppSettings["sftpPassword"].ToString();  
                string stringStrDate = System.DateTime.Now.ToString("dd_MM_yyyy-hh_mm_ss");
                string stringFileName = filename;
                //string stringFromPath = ConfigurationSettings.AppSettings["sFromPath"].ToString();  
                //string stringToPath = ConfigurationSettings.AppSettings["sToPath"].ToString();  
                //string stringHostKey = ConfigurationSettings.AppSettings["sHostKey"].ToString();  

                string ftpServerIP = url;
                string stringsFtpUserID = username;
                string stringsFtpPassword = password;
                string stringFromPath = spath;
                string stringToPath = stopath;

                //string stringsBackUpFolder = "Processed";  
                //create folder for back up data  
                if (!Directory.Exists(stringFromPath))
                {
                    // Directory.CreateDirectory(stringFromPath + stringsBackUpFolder);  
                }
                //check whether file exist or not in local machine.  
                if (!System.IO.File.Exists(stringFromPath))
                {
                    // using (FileStream fileStreamLocalFile = File.Create(stringFromPath + stringFileName))  
                    //{  
                    //byte[] byteLocalFile = new UTF8Encoding(true).GetBytes(filename);  
                    //fileStreamLocalFile.Write(byteLocalFile, 0, byteLocalFile.Length);  
                    // }  
                }




                SessionOptions sessionOptionsSFTP = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = ftpServerIP,
                    UserName = stringsFtpUserID,
                    Password = stringsFtpPassword,
                    PortNumber = 22,
                    SshHostKeyFingerprint = hostkey
                };


                WinSCP.Session sessionSFTP = new WinSCP.Session();
                sessionSFTP.Open(sessionOptionsSFTP);
                TransferOptions transferOptionsSFTP = new TransferOptions();
                transferOptionsSFTP.TransferMode = TransferMode.Binary;
                transferOptionsSFTP.FilePermissions = null;
                transferOptionsSFTP.PreserveTimestamp = false;
                transferOptionsSFTP.ResumeSupport.State = TransferResumeSupportState.Off;
                TransferOperationResult transferOperationResultSFTP;

                transferOperationResultSFTP = sessionSFTP.PutFiles(stringFromPath + filename, filename, false, transferOptionsSFTP);
                if (System.IO.File.Exists(stringFromPath + stringFileName))
                {
                    // File.Move(stringFromPath + stringFileName, stringFromPath + "\\" + stringsBackUpFolder + "\\" + stringFileName);  
                }
                transferOperationResultSFTP.Check();

                
                if (transferOperationResultSFTP.IsSuccess == true)
                {
                    Console.Write("File upload successfully");
                    Notifymail(filename, journal, article,"No Error");
                    report("ReadyFileFTP", filename, "Uploaded TO FTP__" + stopath + "");
                    
                    return true;

                }
                else
                {
                    Console.Write("File upload failed");
                    Notifymail(filename, journal, article, "Upload Failed " + transferOperationResultSFTP.Failures.ToString());
                    report("Ready_FTP_Error", filename, "File upload failed " + transferOperationResultSFTP.Failures.ToString());
                    return false;
                }
            }

            catch (Exception exError)
            {
                Console.Write(exError.Message);
                Notifymail(filename, journal, article,exError.ToString());
                report("Ready_FTP_Error", filename, exError.ToString());
                return false;
            }
        }
        public bool Notifymail(string zip, string journal,string article,string error)

        {
            try
            {
                System.Web.Mail.MailMessage Msg = new System.Web.Mail.MailMessage();
                // Sender e-mail address.
                Msg.From = "itrakadmin@adi-mps.com";
                // Recipient e-mail address.
                Msg.To = "divyashree.bs@adi-mps.com,mallikarjuna.swamy@adi-mps.com";
                //adding 
                Msg.Cc = "Software.mpsj@adi-mps.com";

                //o.To.Add("Software.mpsj@adi-mps.com");
                //o.To.Add("suresh.p@mps-in.com");

                Msg.Subject = "MRW FTP Notification";
                Msg.Body = "Ready Signal uploaded for " +journal +" | '"+article+"', orderID id is "+zip+" Error = "+error+" ";
                // your remote SMTP server IP.
                SmtpMail.SmtpServer = "192.168.1.79";//your ip address
                SmtpMail.Send(Msg);
                return true;
            }
            catch (Exception ex)
            {
                report("Ready_FTP_Error", zip, ex.ToString());
                ex.ToString();
                return false;
            }
        
        
        }
        public bool ZipUpload_FTP(string path,string name,string version)
        {



            string sURI = "ftp://ewiisftp.elsevier.com/ftpdata/dropzones/mcmbkpew/";
            string zippathNew;
            string username = "mcmbkpew";
            string password = "mcmbkpu821034";

            string hostKey = "ssh-rsa 2048 a9:c8:c5:4a:dc:a7:22:0d:80:b8:f0:77:0a:73:24:bf";
            try
            {
                #region FTPUpload

                if (path.StartsWith("d:") && version=="A300")
                {

                    path = path.Replace("d:", "\\\\Bj-b00575");
                    sURI = "ftp://ewiisftp.elsevier.com/ftpdata/dropzones/mpssatpew/" + name;
                    username = "mpssatpew";
                    password = "j8y4Z*1";
                    hostKey = "ssh-rsa 2048 a9:c8:c5:4a:dc:a7:22:0d:80:b8:f0:77:0a:73:24:bf";
                }

                // Create FTP request variables

                

                zippathNew = path;

                bool status = UploadToSFTP(name, "ewiisftp.elsevier.com", username, password, zippathNew, sURI, hostKey);
                return status;
 
                #endregion FTPUpload
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
                report("ZipFTP_FTP_Error", name, ex.ToString());
            }
            
        }
        public bool ReadyXMLUpload_FTP(string path,string name, string version,int id,string article,string journal)
        {
            string sURI = "ftp://ewiisftp.elsevier.com/ftpdata/dropzones/mcmbkpew/" + name;
            string readypathNew = "\\\\192.168.3.137\\s280_dd\\NDM\\MRWReady\\S280\\";
            string username = "mcmbkpew";
            string password = "mcmbkpu821034";
            name = name.Replace(".zip", ".ready.xml");
            string hostKey = "ssh-rsa 2048 a9:c8:c5:4a:dc:a7:22:0d:80:b8:f0:77:0a:73:24:bf";
            try
            {
                #region FTPUpload

                if (path.StartsWith("d:") && version == "A300")
                {

                    readypathNew = readypathNew.Replace(readypathNew, "\\\\192.168.3.137\\s280_dd\\NDM_A300\\MRWReady\\A300\\");
                    sURI = "ftp://ewiisftp.elsevier.com/ftpdata/dropzones/mpssatpew/" + name;
                    username = "mpssatpew";
                    password = "j8y4Z*1";
                }

                // Create FTP request variables



                bool status = UploadToSFTP(name, "ewiisftp.elsevier.com", username, password, readypathNew, sURI, hostKey, article, journal);
               int dbstatus = DBLibrary.Else_UpdateProcessftp(id);
               return status;
                //FileInfo uploadfile = new FileInfo(readypathNew);
                //FtpWebRequest ftpReq;

                //// Create the buffer variable
                //byte[] buffer = new byte[BUFFERSIZE];

                //// Create request by initializing it with the file we want to FTP
                //// Supply network credentials to the request and tell server the file size
                //ftpReq = (FtpWebRequest)FtpWebRequest.Create(sURI);
                //ftpReq.Credentials = new NetworkCredential(username, password);

                //// Set connection parameters
                //ftpReq.UseBinary = true;
                //ftpReq.KeepAlive = false;
                //ftpReq.Method = WebRequestMethods.Ftp.UploadFile;

                //FileStream fs = null;
                //Stream writeStream = null;
                //try
                //{
                //    // Its time to send the file, so create a FileStream to read from the file
                //    fs = uploadfile.OpenRead();

                //    // Return our data stream used to upload the file
                //    writeStream = ftpReq.GetRequestStream();

                //    // Write to the stream while there are still bytes left to stream
                //    int bytesRead = fs.Read(buffer, 0, BUFFERSIZE);

                //    while (bytesRead != 0)
                //    {
                //        writeStream.Write(buffer, 0, bytesRead);
                //        bytesRead = fs.Read(buffer, 0, BUFFERSIZE);
                //    }

                //}
                //catch (Exception ex)
                //{

                //    //Label1.Text = ex.ToString();
                //    throw ex;
                //    return false;
                //}
                //finally
                //{

                //    // Close the streams (dont cross the streams, Ray... that would be bad)
                //    if (fs != null)
                //        fs.Close();

                //    if (writeStream != null)
                //        writeStream.Close();
                //    report("ReadySignalFTP", name, "Uploaded TO FTP__" + sURI + "");
                //    int dbstatus = DBLibrary.Else_UpdateProcessftp(id);
                //}
                
                #endregion FTPUpload
            }
            catch (Exception ex)
            {
                throw ex;
                report("ReadySignalFTP_Error", name, ex.ToString());
                return false;
                
                
            }
            
        }
        public bool report(string rpname, string articlname, string call)
        {
            try
            {

                DateTime dt1 = DateTime.Today;
                string dtn = rpname + "_" + dt1.ToString("s");
                string newdtn = dtn.Substring(0, dtn.Length - 9);
                newdtn += ".txt";
                string path = "\\\\192.168.3.137\\s280_dd\\FTP\\Log\\" + newdtn + "";
                // string call = Request.UserHostAddress;

                if (!File.Exists(path))
                {
                    StreamWriter outFile = null;
                    outFile = File.CreateText(path);
                    outFile.Close();
                    outFile = new StreamWriter(path, false, System.Text.Encoding.UTF8);
                    outFile.Dispose();
                    TextWriter tw = new StreamWriter(path, true);

                    // tw.Writeline(issue2.Text, issue3.Text, issue1.Text);
                    tw.WriteLine("'" + articlname + "-------" + call + "....." + DateTime.Now + "'");
                    tw.Close();
                    return true;
                }
                else
                {


                    TextWriter tw = new StreamWriter(path, true);
                    // tw.Writeline(issue2.Text, issue3.Text, issue1.Text);
                    tw.WriteLine("'" + articlname + "-------" + call + "....." + DateTime.Now + "'");
                    tw.Close();
                    return true;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool reporttest()
        {
            try
            {
                string rpname = "Test_";
                string articlname = "Test";
                string call = "Test";


                DateTime dt1 = DateTime.Today;
                string dtn = rpname + "_" + dt1.ToString("s");
                string newdtn = dtn.Substring(0, dtn.Length - 9);
                newdtn += ".txt";
                string path = "\\\\192.168.3.137\\s280_dd\\FTP\\Log\\" + newdtn + "";
                // string call = Request.UserHostAddress;

                if (!File.Exists(path))
                {
                    StreamWriter outFile = null;
                    outFile = File.CreateText(path);
                    outFile.Close();
                    outFile = new StreamWriter(path, false, System.Text.Encoding.UTF8);
                    outFile.Dispose();
                    TextWriter tw = new StreamWriter(path, true);

                    // tw.Writeline(issue2.Text, issue3.Text, issue1.Text);
                    tw.WriteLine("'" + articlname + "-------" + call + "....." + DateTime.Now + "'");
                    tw.Close();
                    return true;
                }
                else
                {


                    TextWriter tw = new StreamWriter(path, true);
                    // tw.Writeline(issue2.Text, issue3.Text, issue1.Text);
                    tw.WriteLine("'" + articlname + "-------" + call + "....." + DateTime.Now + "'");
                    tw.Close();
                    return true;

                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        protected void TestBTN_Click(object sender, EventArgs e)
        {
            int dbstatus = DBLibrary.Else_UpdateProcessftp(22759);
            reporttest();
        }

        protected void chkFTP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFTP.Checked)
            {
                btnupload.Visible = true;
                btnUpdate.Visible = false;
            }
            else
            {
                btnUpdate.Visible = true;
                btnupload.Visible = false;
            }
        }
        
    }
}