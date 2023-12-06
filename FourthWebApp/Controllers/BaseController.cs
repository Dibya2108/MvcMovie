using Data_Access;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;

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

        public UserViewModel SessionUser
        {
            get
            {
                if (((SessionContext)Session["MvcMovie"]) == null)
                    Response.Redirect(ConfigurationManager.AppSettings["BaseUrl"] + "Account/Login");
                else
                {
                    return ((SessionContext)Session["MvcMovie"]).SessionUser;
                }

                return null;

                
            }
        }

        public bool IsAdmin
        {
            get
            {

                if (SessionUser.UserTypeId == 1) //1=>admin, //|| SessionUser.UserTypeId == 4 || SessionUser.UserTypeId == 7

                    return true;

                return false;

            }

        }

    }
}