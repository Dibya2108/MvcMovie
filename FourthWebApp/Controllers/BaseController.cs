using Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FourthWebApp.Controllers
{
    public class BaseController : Controller
    {

        AccountDalSql _accountDalSql = new AccountDalSql();
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            int userId = (int)Session["UserId"];
            int userType = _accountDalSql.GetUserTypeByUserId(userId);

            ViewBag.UserType = userType;

            base.OnActionExecuting(filterContext);
        }
    }
}