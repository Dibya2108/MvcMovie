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
                
                    if (!_movieDalSql.IsMovieNameUnique(movieViewModel.Id, movieViewModel.Title))
                    {
                        ViewBag.msg = "Duplicate Movie Name";
                                                
                    }

                    if (movieViewModel.Id == 0)
                    {
                        
                        movieViewModel.IsDeleted = false;
                        _movieDalSql.InsertMovie(movieViewModel);


                    }
                    else
                    {
                        _movieDalSql.UpdateMovie(movieViewModel);

                    }
            movieViewModel.Languages = new SelectList(_movieDalSql.GetLanguages(), "Value", "Text");
            return JavaScript("CloseManageMovie()");
            
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

            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                MovieViewModel movieViewModel = _movieDalSql.Delete(id.Value);
                if (movieViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(movieViewModel);
            }

            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                var deleteResult = _movieDalSql.DeleteConfirm(id);

                if (!deleteResult)
                {
                    return HttpNotFound();
                }
                return RedirectToAction("Index");
            }

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

        //public class FileClass
        //{
        //    public int UploaderNumber { get; set; }
        //    public string FilePath { get; set; }
        //    public string FileName { get; set; }
        //}
        //public List<FileClass> Files
        //{
        //    get
        //    {
        //        if (Session["File"] == null)
        //        {
        //            Session["File"] = new List<FileClass>();
        //        }

        //        return (List<FileClass>)Session["File"];
        //    }
        //    set
        //    {
        //        Session["File"] = value;
        //    }
        //}

        //public ActionResult ImageUpload(int movieId = 1)
        //{
        //    string defaultImage = "0.jpg";
            
        //    var movie = _movieDalSql.GetMovieViewModel(movieId);
        //    if (!string.IsNullOrEmpty(movie.movieImageString))
        //    {
        //        string physicalPath = ConfigurationManager.AppSettings["MovieImage"] + "/Large/Movies/" + movie.movieImageString + "?id=" + Guid.NewGuid();
        //        movie.MovieImage = physicalPath;
        //    }
        //    else
        //    {
        //        string physicalPath = ConfigurationManager.AppSettings["MovieImage"] + "/Large/Movies/" + defaultImage + "?id=" + Guid.NewGuid();
        //        movie.MovieImage = physicalPath;
        //    }
        //    return View(movie);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ImageUpload(MovieViewModel movie, HttpPostedFileBase imageFile)
        //{
        //    if (imageFile != null && imageFile.ContentLength > 0)
        //    {
        //        // Get the file extension (without the dot)
        //        string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

        //        // Check if the file extension is valid
        //        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
        //        {
        //            // Generate a unique file name for the uploaded image
        //            string uniqueFileName = Guid.NewGuid() + fileExtension;

        //            // Set the relative path where you want to save the image
        //            string relativePath = ConfigurationManager.AppSettings["MovieImage"] + "/Large/Movies/";

        //            // Combine the path and the unique file name to get the full physical path
        //            string physicalPath = Server.MapPath(Path.Combine(relativePath, uniqueFileName));

        //            // Save the uploaded image to the specified path
        //            imageFile.SaveAs(physicalPath);

        //            // Update the movie's image property with the new file path
        //            movie.MovieImage = Path.Combine(relativePath, uniqueFileName);
        //        }
        //        else
        //        {
        //            // Handle the case where the uploaded file has an invalid extension
        //            ModelState.AddModelError("imageFile", "Please upload a valid image file (jpg, jpeg, or png).");
        //            return View(movie);
        //        }
        //    }
        //    else
        //    {
        //        // Handle the case where no file was uploaded
        //        ModelState.AddModelError("imageFile", "Please select an image to upload.");
        //        return View(movie);
        //    }


        //    return View(movie);
        //}

        //[HttpPost]
        //public ActionResult UploadImage()
        //{
        //    try
        //    {
        //        var uploadedFile = Request.Files[0]; // Assuming you only expect one file in the request

        //        if (uploadedFile != null && uploadedFile.ContentLength > 0)
        //        {
        //            // Check if the file extension is valid
        //            string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
        //            if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
        //            {
        //                // Generate a unique file name for the uploaded image
        //                string uniqueFileName = Guid.NewGuid() + fileExtension;

        //                // Set the relative path where you want to save the image
        //                string relativePath = ConfigurationManager.AppSettings["MovieImage"] + "/Large/Movies/";

        //                // Combine the path and the unique file name to get the full physical path
        //                string physicalPath = Server.MapPath(Path.Combine(relativePath, uniqueFileName));

        //                // Save the uploaded image to the specified path
        //                uploadedFile.SaveAs(physicalPath);

        //                // You can save the file path or other relevant information in your data store if needed
        //                // Example: _movieDalSql.SaveImageFilePath(uniqueFileName);

        //                // Return a JSON response to the client
        //                return Json(new { success = true, fileName = uniqueFileName });
        //            }
        //            else
        //            {
        //                // Invalid file extension
        //                return Json(new { success = false, errorMessage = "Please upload a valid image file (jpg, jpeg, or png)." });
        //            }
        //        }
        //        else
        //        {
        //            // No file was uploaded
        //            return Json(new { success = false, errorMessage = "Please select an image to upload." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that may occur during the upload
        //        return Json(new { success = false, errorMessage = "An error occurred during file upload." });
        //    }
        //}

        //[HttpPost]
        //public ActionResult RemoveImage(string fileName)
        //{
        //    try
        //    {
        //        // Delete the image file from your storage location
        //        string imagePath = ConfigurationManager.AppSettings["MovieImage"] + "/Large/Movies/" + fileName;
        //        string physicalPath = Server.MapPath(imagePath);

        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            System.IO.File.Delete(physicalPath);

        //            // Optionally, remove the file path from your data store
        //            // Example: _movieDalSql.RemoveImageFilePath(fileName);

        //            return Json(new { success = true });
        //        }
        //        else
        //        {
        //            // File not found
        //            return Json(new { success = false, errorMessage = "File not found." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any exceptions that may occur during file removal
        //        return Json(new { success = false, errorMessage = "An error occurred during file removal." });
        //    }
        //}

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
            //if (Photo != null)
            //{
            //    Photo.SaveAs(photoPath);
            //}
            //return ImageUpload();

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
