using Data_Access;
using FourthWebApp.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace MvcMovie.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private readonly AccountDalSql _accountDalSql = new AccountDalSql();
        private readonly MovieDalSql _movieDalSql = new MovieDalSql();

        

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool registrationSuccess = _accountDalSql.RegisterUser(model);

                if (registrationSuccess)
                {
                    return RedirectToAction("RegisterSuccess", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                }
            }
            return View(model);
        }
        public ActionResult RegisterSuccess()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel model)
        {

            bool isAuthenticated = _accountDalSql.AuthenticateUser(model.UserName, model.Password);

            if (isAuthenticated)
            {
                int userId = _accountDalSql.GetUserIdByUsername(model.UserName);

                Session["UserId"] = userId;
                int userType = _accountDalSql.GetUserTypeByUserId(userId);

                ViewBag.UserType = userType;

                

                if (userType == 2)
                {
                    return RedirectToAction("Index", "Dashboard",new { userId });
                }
                else
                {
                    return RedirectToAction("Index", "Movie");
                }
                
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            return View();
        }

        //public ActionResult UserProfile()
        //{
        //    int userId = (int)Session["UserId"];

            
        //    UserViewModel user = _accountDalSql.GetUserById(userId);

        //    if (user == null)
        //    {

        //        return HttpNotFound();

        //    }
        //    return View(user);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UserProfile(UserViewModel model)
        //{
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            bool updateSuccess = _accountDalSql.UpdateUserProfile(model);

                    
        //            if (updateSuccess)
        //            {
        //                return RedirectToAction("UserProfile", "Account", new { userId = model.UserId });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Profile update failed. Please try again.");
        //            }
        //        }
        //        return View(model);
        //    }
        //}

        //public ActionResult WatchList()
        //{
        //    int userId = (int)Session["UserId"];
            
        //    if (userId == 0)
        //    {
        //        return RedirectToAction("Login");
        //    }

            
        //    return View();

        //}

        //public ActionResult GetWatchlist([DataSourceRequest] DataSourceRequest request)
        //{
        //    int loggedInUserId = (int)Session["UserId"];
        //    List<WatchViewModel> watch = _accountDalSql.GetAllWatchList(loggedInUserId);

        //    return Json(watch.ToDataSourceResult(request));
        //}

        //public ActionResult ManageWatchList(int WatchListId = 0)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        WatchViewModel model = new WatchViewModel();
        //        int loggedInUserId = (int)Session["UserId"];

        //        if (WatchListId != 0)
        //        {
        //            model = _accountDalSql.GetWatchViewModel(WatchListId);

        //            List<int> selectedMovieIds = _accountDalSql.GetSelectedMovieIdsForWatchList(WatchListId);
        //            model.MovieIds = selectedMovieIds;
        //            model.MovieIdString = string.Join(",", selectedMovieIds);
        //        }
                

        //        model.MovieCollection = _accountDalSql.GetAvailableMovies();

        //        return View(model);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ManageWatchList(WatchViewModel model)
        //{
        //    try
        //    {
        //        int loggedInUserId = (int)Session["UserId"];

        //        if (ModelState.IsValid)
        //        {
        //            // Converting the selected values to a List<int>
        //            //List<int> selectedMovieIds = model.MovieIds != null ? model.MovieIds.ToList() : new List<int>();

        //            List<int> selectedMovieIds = !string.IsNullOrWhiteSpace(model.MovieIdString)
        //        ? model.MovieIdString.Split(',').Select(int.Parse).ToList()
        //        : new List<int>();

        //            if (model.WatchListId == 0)
        //            {
        //                _accountDalSql.AddWatchList(model.Name, model.Description, loggedInUserId, selectedMovieIds);
        //            }
        //            else
        //            {
        //                // Updating WatchList with the selectedMovieIds
        //                model.MovieIds = selectedMovieIds;
        //                _accountDalSql.UpdateWatchList(model);
        //            }

        //            //return JavaScript("CloseWatchList()");
        //            TempData["WatchSuccess"] = true;
        //            return RedirectToAction("WatchList", new { userId = loggedInUserId });

        //        }

        //        model.MovieCollection = _accountDalSql.GetAvailableMovies();
        //        return View(model);
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError(string.Empty, "An error occurred while saving the watch list.");
        //        model.MovieCollection = _accountDalSql.GetAvailableMovies();
        //        return View(model);
        //    }
        //}

        //public ActionResult GetAllMovies([DataSourceRequest] DataSourceRequest request, int watchlisId = 0)
        //{
        //    List<MovieViewModel> MovieCollection = _accountDalSql.GetAvailableMovies();
        //    return Json(MovieCollection, JsonRequestBehavior.AllowGet);
        //}


        //public string GetWatchListTable()
        //{
        //    int loggedInUserId = (int)Session["UserId"];

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<table id='tableWatch'>");
        //    sb.Append("<tr>");
        //    sb.Append("<th>" + "No.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>");
        //    sb.Append("<th>" + "WatchList Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>");
        //    sb.Append("<th>" + "Description &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>");
        //    sb.Append("<th>" + "Count");
        //    sb.Append("</tr>");

        //    List<WatchViewModel> watchList = _accountDalSql.GetAllWatchList(loggedInUserId);

        //    int serialNumber = 1;

        //    foreach (var watch in watchList)
        //    {
        //        sb.Append("<tr>");
        //        sb.Append("<td>" + serialNumber + "</td>");
        //        sb.Append("<td><a href='#' onclick='AddWatchList(" + watch.WatchListId + ")'>" + watch.Name + "</td>");
        //        sb.Append("<td>" + watch.Description + "</td>");
        //        sb.Append("<td>" + watch.Count + "</td>");
        //        sb.Append("</tr>");

        //        serialNumber++;
        //    }

        //    sb.Append("</table>");
        //    return sb.ToString();
        //}
        //public string GetFavouritesTable()
        //{

        //    int loggedInUserId = (int)Session["UserId"];
        //    List<WatchViewModel> movFav = _accountDalSql.GetFavoriteMovies(loggedInUserId);

        //    string sb2 = "<div>\n";

        //    var groupedList = movFav.GroupBy(u => u.WatchListId).Select(grp => grp.ToList()).ToList();
        //    foreach (var watch in groupedList)
        //    {
        //        var watchName = watch.FirstOrDefault();

        //        sb2 += @"<div><span><a href='#' onclick='openFavMovieManagement(" + watchName.WatchListId + ")'><strong>" + watchName.Name + "</strong></a></span></div>\n";
        //        sb2 += "<div>\n";

        //        foreach (var movie in watch)
        //        {
        //            if (movie.MovieId != 0)
        //            {
        //                sb2 += @"<span>" + movie.Title + "</span> <br>\n";
        //            }

        //        }

        //        sb2 += "</div>\n";

        //    }
        //    sb2 += "</div>\n";

        //    return sb2;
        //}

        //public ActionResult ManageFavourites(int WatchListId = 0)
        //{

        //    if (Session["UserId"] == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        WatchViewModel model = new WatchViewModel();
        //        int loggedInUserId = (int)Session["UserId"];

        //        if (WatchListId != 0)
        //        {
        //            model = _accountDalSql.GetWatchViewModel(WatchListId);

        //            model.MovieCollection = _accountDalSql.GetAvailableMoviesForWatchList(WatchListId);

        //            List<int> favoriteMovieIds = _accountDalSql.GetFavoriteMovieIdsForWatchList(WatchListId);

        //            foreach (var movie in model.MovieCollection)
        //            {
        //                movie.IsChecked = favoriteMovieIds.Contains(movie.Id);
        //            }

        //            var checkboxesHtml = new StringBuilder();
        //            var i = 0;
        //            foreach (var movie in model.MovieCollection)
        //            {
        //                var checkBoxStatus = "";
        //                checkBoxStatus = movie.IsChecked ? "checked" : "";
        //                checkboxesHtml.Append("<div>");
        //                //checkboxesHtml.Append("<input type='hidden' class='MovieList' id='MovieCollection_" + movie.Id + "' />");
        //                checkboxesHtml.Append("<input type='checkbox' class='MovieList' id='MovieCollection_" + movie.Id + "' " + checkBoxStatus + "/>");
        //                checkboxesHtml.Append("<label>" + movie.Title + "</label><br />");
        //                i++;
        //                checkboxesHtml.Append("</div>");
        //            }
        //            ViewBag.CheckboxesHtml = checkboxesHtml.ToString();

        //        }
        //        else
        //        {
        //            model.WatchListCollection = _accountDalSql.GetAllWatchListExceptFav(loggedInUserId);
        //        }
        //        model.WatchListId = WatchListId;
        //        return View(model);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ManageFavourites(WatchViewModel model)
        //{
        //    int loggedInUserId = (int)Session["UserId"];

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            //Delete Previous Mapping

        //            _accountDalSql.DeleteFavourites(model.WatchListId);

        //            if (!string.IsNullOrEmpty(model.CheckedMovieIds))
        //            {
        //                var trimChar = "~;~";
        //                //Save New Mapping
        //                model.CheckedMovieIds = model.CheckedMovieIds.TrimEnd(trimChar.ToCharArray());
        //                var splittedString = model.CheckedMovieIds.Split(trimChar.ToCharArray());

        //                foreach (var item in splittedString)
        //                {

        //                    var innersplittedString = item.Split(',');

        //                    if (innersplittedString.Length == 2)
        //                    {
        //                        WatchViewModel view = new WatchViewModel();
        //                        view.WatchListId = model.WatchListId;
        //                        view.MovieId = int.Parse(innersplittedString[0]);
        //                        view.IsFavourite = bool.Parse(innersplittedString[1]);
        //                        view.CreatedBy = loggedInUserId;

        //                        int addId = _accountDalSql.AddFavoriteMovies(view);
        //                    }

        //                }
        //            }
        //            return JavaScript("CloseFavMovieManagement()");
        //        }

        //        catch (Exception)
        //        {

        //            ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        //        }

        //    }
        //    return View(model);
        //}
        //public string GetMoviesForWatchlist(int WatchListId)
        //{
        //    var movies = _accountDalSql.GetAvailableMoviesForWatchList(WatchListId);

        //    StringBuilder checkboxesHtml = new StringBuilder();
        //    foreach (var movie in movies)
        //    {

        //        checkboxesHtml.Append("<div>");
        //        checkboxesHtml.Append("<input type='checkbox' class='MovieList' id='MovieCollection_" + movie.Id + "'/>");
        //        checkboxesHtml.Append("<label>" + movie.Title + "</label>");
        //        checkboxesHtml.Append("</div>");
        //    }
        //    return checkboxesHtml.ToString();
        //}

        //public ActionResult DeleteWatchList(int watchListId)
        //{
        //    if (watchListId == 0)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    WatchViewModel view = _accountDalSql.Delete(watchListId);

        //    if (view == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(view);
        //}
        //[HttpPost, ActionName("DeleteWatchList")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteWatchListConfirmed(int watchListId)
        //{
        //    int loggedInUserId = (int)Session["UserId"];
        //    var deleteResult = _accountDalSql.DeleteConfirm(watchListId);

        //    if (!deleteResult)
        //    {
        //        return HttpNotFound();
        //    }
        //    return RedirectToAction("WatchList", new { userId = loggedInUserId });
        //}



    }
}