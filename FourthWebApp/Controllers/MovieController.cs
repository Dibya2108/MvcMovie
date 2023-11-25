using Data_Access;
using MvcMovie.Models;
using MvcMovie.Utils;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using System.Web.Hosting;

namespace MvcMovie.Controllers
{
    
        public class MovieController : Controller
        {
        MovieDalSql _movieDalSql = new MovieDalSql();
        string photoPath = Path.Combine(HostingEnvironment.MapPath("~/Images/RawImage/"), "profile_photo.jpg");
        public ActionResult Index()
            {
            
            return View();
            }

        public ActionResult GetMovies([DataSourceRequest] DataSourceRequest request)
        {
            List <MovieViewModel> movie  = _movieDalSql.GetMoviesForKendoGrid();

            return Json(movie.ToDataSourceResult(request));
        }

        public ActionResult Manage(int id = 0)
            {
            MovieViewModel model = new MovieViewModel();
            if (id != 0)
            {
                model = _movieDalSql.GetMovieViewModel(id);
                if (model == null)
                {
                    return HttpNotFound();
                }
            }

            var languages = _movieDalSql.GetLanguages();
            model.Languages = new SelectList(languages, "Value", "Text");
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(MovieViewModel movieViewModel)
        {
            //int movId = movieViewModel.Id;

            int i = 0;

            if (movieViewModel.Id == 0)
            {

                movieViewModel.IsDeleted = false;
                i = _movieDalSql.InsertMovie(movieViewModel);
                return JavaScript("CloseManageMovie(1," + i + @")");


            }
            else
            {
                _movieDalSql.UpdateMovie(movieViewModel);

            }
            movieViewModel.Languages = new SelectList(_movieDalSql.GetLanguages(), "Value", "Text");
            //return JavaScript("CloseManageMovie(" + movId + @");");
            return JavaScript("CloseManageMovie(0," + i + @")");

        }

        public JsonResult CheckDupMovie(int Id, string Title)
        {
            bool isExist = _movieDalSql.MovieExist(Id, Title);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                MovieViewModel movie = _movieDalSql.GetMovieById(id);

                if (movie == null)
                {
                    return HttpNotFound();
                }

                return View(movie);

            }

            public string Delete(int id)
            {
            string msg = "";
            try
            {
                _movieDalSql.DeleteConfirm(id);
                msg = "Ok";
            }
            catch (Exception e)
            {
                msg = "ERROR : " + e.Message;
            }
            return msg;

            

            }

            //[HttpPost, ActionName("Delete")]
            //[ValidateAntiForgeryToken]
            //public ActionResult DeleteConfirmed(int id)
            //{
            //    var deleteResult = _movieDalSql.DeleteConfirm(id);

            //    if (!deleteResult)
            //    {
            //        return HttpNotFound();
            //    }
            //    return RedirectToAction("Index");
            //}

            public string GetMoviesByLanguage(int? languageId)
            {
                List<MovieViewModel> movieList = new List<MovieViewModel>();
                StringBuilder sb = new StringBuilder();
                movieList = _movieDalSql.GetMoviesByLanguage(languageId);
                foreach (var movie in movieList)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>" + movie.Title + "</td>");
                    sb.Append("<td>" + movie.Genre + "</td>");
                    sb.Append("<td>" + movie.ReleaseDate.ToShortDateString() + "</td>");
                    sb.Append("<td>" + movie.Rating + " </td>");
                    sb.Append("<td>" + movie.LanguageName + " </td>");
                    sb.Append("<td>" + movie.Price + "</td>");
                    sb.Append("<td><a style='cursor :pointer' onclick='ManageMovie(" + movie.Id + ")'><i class='fa fa-pencil'></i></a></td>");
                    sb.Append("<td><a style='cursor :pointer' onclick='DetailsMovie(" + movie.Id + ")'><i class='fa fa-book'></i></a></td>");
                    sb.Append("<td><a style='cursor :pointer' onclick='DeleteMovie(" + movie.Id + ")'><i class='fa fa-trash'></i></a></td>");
                    //sb.Append("<td>@Html.ActionLink('Edit', 'Manage', new { id = " + movie.Id + "})</td>");
                    sb.Append("</tr>");
                }
                return sb.ToString();
            }

        

        public ActionResult ImageUpload(int id)
        {
            string defaultImage = "/Images/Large/Movies/movie.png";

            var movie = _movieDalSql.GetMovieViewModel(id);
            string imagePath = _movieDalSql.GetMovieImageFilePath(id);

            // If no image is found for the movie, use the default image
            if (imagePath == defaultImage)
            {
                imagePath = defaultImage;
            }
            //ViewBag.Photo = System.IO.File.Exists(photoPath) ? "/images/profile_photo.jpg" : "/images/Large/Movies/movie.png";
            ViewBag.Photo = imagePath;
            return View(movie);
        }


        public ActionResult Save(HttpPostedFileBase Photo, int id)
        {
            ;

            if (Photo != null)
            {
                // Get the movie's image file path
                string imagePath = _movieDalSql.GetMovieImageFilePath(id);

                // Save the uploaded image
                string fileExtension = Path.GetExtension(Photo.FileName).ToLower();
                string uniqueFileName = Guid.NewGuid() + fileExtension;
                string relativePath = "/Images/RawImage/";

                string physicalPath = Server.MapPath(Path.Combine(relativePath, uniqueFileName));
                Photo.SaveAs(physicalPath);

                // Update the movie's image file path and save it
                _movieDalSql.SaveMovieImageFilePath(id, Path.Combine(relativePath, uniqueFileName));
            }

            return RedirectToAction("Index");
        }

    }
}
