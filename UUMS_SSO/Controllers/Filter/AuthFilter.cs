using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UUMS_SSO.Models;

namespace UUMS_SSO.Controllers.Filter
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        //
        // GET: /AuthFilter/

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //获取session
            var username = filterContext.HttpContext.User.Identity.Name;

            if (username == null)
            {
                //设置页面跳转
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Index"
                };
            }

        }
    }
}
