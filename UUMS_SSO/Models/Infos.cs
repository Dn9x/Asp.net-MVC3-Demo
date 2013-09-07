using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

namespace UUMS_SSO.Models
{
    public class Infos
    {
        //string conn = ConfigurationManager.AppSettings["web_oa_production"];
        string conn = "Password=weboapwd;User ID=web_oa;Data Source=172.24.81.190:1521/pc2eit";

        #region 验证该user 是否有此系统的使用权限
        //驗證此用戶是否有使用該系統的權限
        public DataTable GetPosts(string p_login, string p_app_url, string p_redirect_url, string p_app_code, string SessionID, string UserHostAddress)
        {
            DataTable dt = new DataTable();
            if (p_app_code == "")
            {
                string sql = string.Format("select pkg_uums.post_fm(:p_login,:UserHostAddress,:SessionID,:p_app_url) html from dual");
                OracleParameter[] paramList ={ 
                                new OracleParameter("p_login",OracleType.VarChar,100),
                                new OracleParameter("UserHostAddress",OracleType.VarChar,100),
                                new OracleParameter("SessionID",OracleType.VarChar,100),
                                new OracleParameter("p_app_url",OracleType.VarChar,100)

                };
                paramList[0].Value = p_login;
                paramList[1].Value = UserHostAddress;
                paramList[2].Value = SessionID;
                paramList[3].Value = p_app_url;
                dt = Gbm.Db.OracleHelper.ExecuteDataset(conn, CommandType.Text, sql, paramList).Tables[0];

            }
            else
            {
                string sql = string.Format("select pkg_uums.post_fm(:p_login,:UserHostAddress,:SessionID,:p_app_url,:p_redirect_url,:p_app_code) html from dual");
                OracleParameter[] LoginList ={ 
                                new OracleParameter("p_login",OracleType.VarChar,100),
                                new OracleParameter("UserHostAddress",OracleType.VarChar,100),
                                new OracleParameter("SessionID",OracleType.VarChar,100),
                                new OracleParameter("p_app_url",OracleType.VarChar,100),
                                new OracleParameter("p_redirect_url",OracleType.VarChar,100),
                                new OracleParameter("p_app_code",OracleType.VarChar,100)};
                LoginList[0].Value = p_login;
                LoginList[1].Value = UserHostAddress;
                LoginList[2].Value = SessionID;
                LoginList[3].Value = p_app_url;
                LoginList[4].Value = p_redirect_url;
                LoginList[5].Value = p_app_code;
                dt = Gbm.Db.OracleHelper.ExecuteDataset(conn, CommandType.Text, sql, LoginList).Tables[0];
            }

            return dt;
        }
        #endregion




        /// <summary>
        /// 驗證登錄
        /// </summary>
        /// <param name="UserName">用戶名</param>
        /// <param name="PassWd">用戶密碼</param>
        /// <param name="SessionID">SessionID</param>
        /// <param name="UserHostAddress">主機IP</param>
        /// <returns></returns>
        public String ValidateLogin(string UserName, string PassWd, string SessionID, string UserHostAddress)
        {
            string sql = string.Format("select fun_uums_auth(:username,:passwd,:UserHostAddress,:SessionID) as msg from dual");

            OracleParameter[] paramList ={ 
                                new OracleParameter("username",OracleType.VarChar,100),
                                new OracleParameter("passwd",OracleType.VarChar,100),
                                new OracleParameter("UserHostAddress",OracleType.VarChar,100),
                                new OracleParameter("SessionID",OracleType.VarChar,100)
            };
            paramList[0].Value = UserName;
            paramList[1].Value = PassWd;
            paramList[2].Value = UserHostAddress;
            paramList[3].Value = SessionID;

            DataTable dt = Gbm.Db.OracleHelper.ExecuteDataset(conn, CommandType.Text, sql, paramList).Tables[0];

            //獲取返回值
            string msg = dt.Rows[0]["msg"].ToString();

            //返回結果
            return msg;
        }


    }
}