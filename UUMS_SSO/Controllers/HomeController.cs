using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OracleClient;
using UUMS_SSO.Models;
using System.Web.Security;
using UUMS_SSO.Controllers.Filter;
using System.Text;

namespace UUMS_SSO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(string name, string pswd)
        {
            var result = "";

            Infos info = new Infos();

            var sesid = Session.SessionID;

            var res = info.ValidateLogin(name, pswd, sesid, Request.UserHostAddress);

            Session["sesid"] = sesid;

            string[] UserMSG = new string[2];

            //得到相關信息
            if (Util.SplitString(res, ref UserMSG))
            {
                Session["usern"] = UserMSG[0];
                result = "Y";
            }
            else
            {
                result = "用戶名或密碼不正確";
            }

            return Content(result);
        }

        [HttpPost]
        public ActionResult Login(LogOnModel model)
        {
            if (ModelState.IsValid)
            {
                string returnUrl = Request.Url.AbsoluteUri;

                Infos info = new Infos();

                var sesid =  Session.SessionID;

                var result = info.ValidateLogin(model.UserName, model.Password, sesid, Request.UserHostAddress);
                
                Session["sesid"] = sesid;

                string[] UserMSG = new string[2];

                //得到相關信息
                if (Util.SplitString(result, ref UserMSG))
                {
                    Session["usern"] = UserMSG[0];
                    FormsAuthentication.SetAuthCookie(UserMSG[0], true);
                }
            }

            if (Util.jump_type == "2")
            {
                Util.jump_type = "1";
                return Redirect(Util.jump_url);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Jump()
        {
            var username = HttpContext.User.Identity.Name;

            if (username.Length == 0)
            {
                Util.jump_type = "2";
                Util.jump_url = Request.Url.AbsoluteUri;
                return RedirectToAction("Index", "Home", new { t = "login", j = Request["j"], d = Request["d"] });
            }

            var url = HttpUtility.HtmlDecode(Request["j"]);
            var h = Request["d"];

            var post = SetPost(username, h, url);

            ViewBag.Href = post;

            return View();
        }

        private string SetPost(string login, string aid, string ahref)
        {
            string p_app_url;      //應用登錄網址
            string p_redirect_url; //應用登錄後重定向網址

            //tag_a_id = "aLOTUS_OA_PSA_01";

            //根據a標簽開頭之屬性來確定app_url
            if (aid.IndexOf("LOTUS_OA") == 0)
            {
                //context.Response.Write("LOTUS_OA_PSA");
                p_app_url = "http://wtcfs13.passivecomponent.com/names.nsf?Login";
                p_redirect_url = "LOTUS_OA";
            }
            else if (aid.IndexOf("LOTUS_OA_GBM") == 0)
            {
                //context.Response.Write("LOTUS_OA_PSA");
                p_app_url = "http://gbmns31.gbmgroup.com/names.nsf?Login";
                p_redirect_url = "LOTUS_OA";
            }
            else if (aid.IndexOf("LOTUS_MAIL") == 0)
            {
                //context.Response.Write("LOTUS_MAIL");
                p_app_url = "http://gbmns31.gbmgroup.com/names.nsf?Login";
                p_redirect_url = aid;
            }
            else if (aid.IndexOf("LOTUS_MAIL_COMPACT") == 0)
            {
                //context.Response.Write("LOTUS_MAIL_COMPACT");
                p_app_url = "http://gbmns31.gbmgroup.com/names.nsf?Login";
                p_redirect_url = aid;
            }
            else　//默認一般WEB應用
            {
                //context.Response.Write("General WEB APP:"+tag_a_id);
                p_app_url = ahref;
                p_redirect_url = aid;
            }

            DataTable dt = new Infos().GetPosts(login, p_app_url, ahref, p_redirect_url, Session["sesid"].ToString(), Request.UserHostAddress);
            string msg = "";
            if (dt.Rows.Count > 0)
            {
                string t = dt.Rows[0]["html"].ToString().Substring(4, 2);
                if (dt.Rows[0]["html"].ToString().Substring(4, 2) != "NG")
                {
                    msg = dt.Rows[0]["html"].ToString();

                    msg = GetUrl(msg, true);
                }
                else
                {
                    msg = GetUrl(ahref, false);
                }
            }

            return msg;
        }

        private string GetUrl(string post, bool type)
        {
            StringBuilder  str = new StringBuilder();

            str.Append("<!DOCTYPE>");
            str.Append("<html>");
            str.Append("<head>");
            str.Append("<title></title>");
            str.Append("</head>");
            str.Append("<body>");
            str.Append("<script>");

            if (!type)
            {
                str.Append("window.location = '"+post+"';");
            }
            else
            {
                str.Append("var c ={dk:new Array(-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,62,-1,-1,-1,63,52,53,54,55,56,57,58,59,60,61,-1,-1,-1,-1,-1,-1,-1,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,-1,-1,-1,-1,-1,-1,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,-1,-1,-1,-1,-1),dc:function(sr){var sa = new Array();var c1, c2, c3, c4;var p=0;sr = sr.replace(/[^A-Za-z0-9\\+\\/]/g,'');while(p+4<=sr.length){c1 = this.dk[sr.charCodeAt(p++)];c2=this.dk[sr.charCodeAt(p++)];c3 = this.dk[sr.charCodeAt(p++)];c4=this.dk[sr.charCodeAt(p++)];sa.push(String.fromCharCode((c1<<2&0xff)+(c2>>4),(c2<<4&0xff)+(c3>>2),(c3<<6&0xff)+c4));} if(p+1<sr.length){c1=this.dk[sr.charCodeAt(p++)];c2 = this.dk[sr.charCodeAt(p++)];if(p<sr.length){c3=this.dk[sr.charCodeAt(p)];sa.push(String.fromCharCode((c1<<2&0xff)+(c2>>4),(c2<<4&0xff)+(c3>>2)));}else {sa.push(String.fromCharCode((c1<<2&0xff)+(c2>>4)));}}return sa.join('');}};");
                str.Append("document.write(c.dc('" + post + "'));");
                str.Append("window.onload = function () {");
                str.Append("document.getElementById('frmUumsLogin').submit();");
                str.Append("}");
            }
            
            str.Append("</script>");
            str.Append("</body>");
            str.Append("</html>");


            return str.ToString();
        }

    }
}
