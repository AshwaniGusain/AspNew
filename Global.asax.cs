using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Contrast_Web
{
    public class Global : System.Web.HttpApplication
    {
        #region Global Constans

        public const sbyte DDL_INIT_INDEX = -1;
        public const string DDL_INITIAL_ALLTXT = "ALL";
        public const string DDL_INITIAL_TXT = "-Select-";
        public const string DDL_INITIAL_VAL = "0";
        public const string DDL_INITIAL_STRVAL = "";
        public const string MSG_VAL_DATE = "Date is not in correct format";
        public const string MSG_GRD_EMPTY = "No record found";
        public const string MSG_ERROR = "Some error occurs, Please contact to administrator";
        public const byte STS_SEND_BU = 8;
        public const string zipFileUpload = "ZipUpload";
        public const string StageID = "StageID";
        public const string SubStageId = "SubStageID";

        public const string DB_FIELD_ID = "FieldID";
        public const string DB_FIELD_NAME = "FieldName";


        static string _userId = string.Empty;
        public static string userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        static string _userName = string.Empty;

        public static string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        static string _roleID = string.Empty;

        public static string RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        static string _roleName;

        public static string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }


        static string userIdstring = string.Empty;
        public static string UserIdstring
        {
            get { return userIdstring; }
            set { userIdstring = value; }
        }



        public enum PTSRoles
        {
            WebAdmin,
            ProdAdmin,
            User,
            Viewuser
        }

        private PTSRoles _ptsRoleName;

        public PTSRoles PtsRoleName
        {
            get { return _ptsRoleName; }
            set { _ptsRoleName = value; }
        }


        static string redirectUrl = string.Empty;

        public static string RedirectUrl
        {
            get { return redirectUrl; }
            set { redirectUrl = value; }
        }





        #endregion


        public static string TamperProofStringDecode(string value1)
        {
            string key = "0920";
            string dataValue = "";
            string calcHash = "";
            string storedHash = "";
            //value1 = value1.Replace('¿', '+');
            value1 = HttpUtility.UrlDecode(value1);
            value1 = value1.Replace(' ', '+');
            System.Security.Cryptography.MACTripleDES mac3des = new System.Security.Cryptography.MACTripleDES();
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            mac3des.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));

            try
            {
                dataValue = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value1.Split('-')[0]));
                storedHash = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(value1.Split('-')[1]));
                calcHash = System.Text.Encoding.UTF8.GetString(mac3des.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataValue)));

                if (storedHash != calcHash)
                {
                    //Data was corrupted 
                    return "";
                    // throw new ArgumentException("Hash value does not match");
                }
                //This error is immediately caught below 
            }
            catch (Exception ex)
            {
                return "";
                //throw new ArgumentException("Invalid TamperProofString");
            }


            return dataValue;
        }
        /// <summary>
        /// Validate UseeId Staring
        /// </summary>
        /// <returns></returns>
        public static string Validate()
        {

            try
            {
                //UserIdstring = "";
                //_userId = "CB1790";
                RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["Redirecturl"].ToString();


                if (System.Web.HttpContext.Current.Request.QueryString["Username"] != null)
                {
                    UserIdstring = System.Web.HttpContext.Current.Request.QueryString["Username"].ToString();
                    //COMMENTED 
                    _userId = TamperProofStringDecode(UserIdstring);
                    //_userId = UserIdstring;
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Redirect(RedirectUrl);
                }


            }
            catch (Exception Ex)
            {
                _userId = "";
            }
            return _userId;
        }
        public static string GetUserIdstring()
        {

            try
            {
                //UserIdstring = "Y2IzMDE4-aWqNmiZTdqw=";


                //RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["Redirecturl"].ToString();


                if (System.Web.HttpContext.Current.Request.QueryString["Username"] != null)
                {
                    UserIdstring = System.Web.HttpContext.Current.Request.QueryString["Username"].ToString();

                }
                else
                {
                    System.Web.HttpContext.Current.Response.Redirect(RedirectUrl);
                }


            }
            catch (Exception Ex)
            {
                UserIdstring = "";
            }
            return UserIdstring;
        }


    }
}