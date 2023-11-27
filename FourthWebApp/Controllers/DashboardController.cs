using Data_Access;
using FourthWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace MvcMovie.Controllers
{
    public class DashboardController : BaseController
    {
        MovieDalSql _movieDalSql = new MovieDalSql();
        AccountDalSql _accountDalSql = new AccountDalSql();
       // GET: Dashboard
        public ActionResult Index()

        {
            int loggedInUserId = (int)Session["UserId"];
            List<MovieViewModel> cards = _movieDalSql.GetMoviesForKendoGrid();
            StringBuilder sb = new StringBuilder();
            //int userType = _accountDalSql.GetUserTypeByUserId(loggedInUserId);

            //ViewBag.UserType = userType;

            UserViewModel user = new UserViewModel();
            user.UserId = loggedInUserId;
            user.UserTypeId =_accountDalSql.GetUserTypeByUserId(loggedInUserId);
            //sb.Append("<div style='display: flex;'>");
            sb.Append("<div class='row'>");
            

            foreach (var card in cards)
            {
                sb.Append("<div class='col-sm-3'>");
                sb.Append("<div class='card' style='margin-bottom:20px'>");
                sb.Append("<div class='card-body'>");
                //sb.Append($"<a href='/ShowTime/BookTicket/{card.Id}'>");
                sb.Append($"<img class='card-img-top' src='{card.MovieImage}' alt='Card image cap' style=' height:280px'>");
                //sb.Append("</a>");

                
                sb.Append($"<h5 class='card-title'>{card.Title}</h5>");
                sb.Append($"<p class='card-text'>{card.Genre}</p>");
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            sb.Append("</div>");
            //sb.Append("</div>");

            ViewBag.CardHtml = sb.ToString();

            return View(user);
        }






    }
}