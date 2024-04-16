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
        //public ActionResult Index()

        //{
        //    int loggedInUserId = (int)Session["UserId"];
        //    List<MovieViewModel> cards = _movieDalSql.GetMoviesForKendoGrid();
        //    StringBuilder sb = new StringBuilder();
        //    //int userType = _accountDalSql.GetUserTypeByUserId(loggedInUserId);

        //    //ViewBag.UserType = userType;

        //    UserViewModel user = new UserViewModel();
        //    user.UserId = loggedInUserId;
        //    user.UserTypeId =_accountDalSql.GetUserTypeByUserId(loggedInUserId);
        //    //sb.Append("<div style='display: flex;'>");
        //    sb.Append("<div class='row'>");


        //    foreach (var card in cards)
        //    {
        //        sb.Append("<div class='col-sm-3'>");
        //        sb.Append("<div class='card' style='margin-bottom:20px'>");
        //        sb.Append("<div class='card-body'>");
        //        //sb.Append($"<a href='/ShowTime/BookTicket/{card.Id}'>");
        //        sb.Append($"<img class='card-img-top' src='{card.MovieImage}' alt='Card image cap' style=' height:280px'>");
        //        //sb.Append("</a>");


        //        sb.Append($"<h5 class='card-title'>{card.Title}</h5>");
        //        sb.Append($"<p class='card-text'>{card.Genre}</p>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");
        //        sb.Append("</div>");
        //    }
        //    sb.Append("</div>");
        //    //sb.Append("</div>");

        //    ViewBag.CardHtml = sb.ToString();

        //    return View(user);
        //}


        public ActionResult Index()
        {
            int loggedInUserId = (int)Session["UserId"];
            List<MovieViewModel> movies = _movieDalSql.GetMoviesForKendoGrid();
            UserViewModel user = new UserViewModel();
            user.UserId = loggedInUserId;
            user.UserTypeId = _accountDalSql.GetUserTypeByUserId(loggedInUserId);

            StringBuilder sb = new StringBuilder();
            sb.Append("<strong>Recommended Movies</strong>");
            sb.Append("<div id='carouselExampleControls' class='carousel slide' data-ride='carousel'>");
            sb.Append("<div class='carousel-inner'>");
            

            int totalMovies = movies.Count;
            int numberOfSlides = (int)Math.Ceiling((double)totalMovies / 4);

            for (int i = 0; i < numberOfSlides; i++)
            {
                sb.Append("<div class='carousel-item");
                if (i == 0)
                {
                    sb.Append(" active");
                }
                sb.Append("'>");
                sb.Append("<div class='row'>");

                for (int j = i * 4; j < (i * 4) + 4 && j < totalMovies; j++)
                {
                    var movie = movies[j];
                    sb.Append("<div class='col-sm-3'>");
                    sb.Append("<div class='card' style='margin-bottom:20px'>");
                    sb.Append($"<img class='card-img-top' src='{movie.MovieImage}' alt='{movie.Title}' style='height: 300px; object-fit: cover;'>");
                    sb.Append("<div class='card-body'>");
                    sb.Append($"<h5 class='card-title'>{movie.Title}</h5>");
                    sb.Append($"<p class='card-text'>{movie.Genre}</p>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                }

                sb.Append("</div>");
                sb.Append("</div>");
            }

            sb.Append("</div>");
            sb.Append("<a class='carousel-control-prev' href='#carouselExampleControls' role='button' data-slide='prev'>");
            sb.Append("<span class='carousel-control-prev-icon' aria-hidden='true'></span>");
            sb.Append("<span class='sr-only'>Previous</span>");
            sb.Append("</a>");
            sb.Append("<a class='carousel-control-next' href='#carouselExampleControls' role='button' data-slide='next'>");
            sb.Append("<span class='carousel-control-next-icon' aria-hidden='true'></span>");
            sb.Append("<span class='sr-only'>Next</span>");
            sb.Append("</a>");
            sb.Append("</div>");

            ViewBag.CarouselHtml = sb.ToString();

            return View(user);
        }





    }
}