using Data_Access;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace FourthWebApp.Controllers
{
    public class ShowTimeController : Controller
    {

        ShowTimeDalSql _ShowTimeDalSql = new ShowTimeDalSql();
        // GET: ShowTime
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetShowTime([DataSourceRequest] DataSourceRequest request)
        {
            List<ShowTimeViewModel> movie = _ShowTimeDalSql.GetShowTimeForKendoGrid();

            return Json(movie.ToDataSourceResult(request));
        }

        public ActionResult GetMovies([DataSourceRequest] DataSourceRequest request)
        {
            List<MovieViewModel> movies = _ShowTimeDalSql.GetMovieForKendoGrid();

            return Json(movies, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Manage(int showTimeId = 0)
        {

                if(showTimeId != 0)
                {
                    var existingShowTime = _ShowTimeDalSql.GetShowViewModel(showTimeId); 

                    if (existingShowTime == null)
                    {
                        return HttpNotFound();
                    }
                    
                if (existingShowTime.StartDate != null)
                {
                    existingShowTime.StartDateString = existingShowTime.StartDate.ToString("MM/dd/yyyy");
                }
                if (existingShowTime.EndDate != null)
                {
                    existingShowTime.EndDateString = existingShowTime.EndDate.ToString("MM/dd/yyyy");
                }

                return View(existingShowTime);
                }

                else
                {
                    var model = new ShowTimeViewModel();
                    return View(model);
                }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ShowTimeViewModel model)
        {
            
                if (model.ShowTimeId == 0)
                {
                    // This is a new ShowTime
                    var newShowTime = new ShowTimeViewModel
                    {
                        MovieId = model.MovieId,
                        Title = model.Title,
                        StartDateString = model.StartDateString,
                        EndDateString = model.EndDateString,
                        FirstShowTime = model.FirstShowTime,
                        SecondShowTime = model.SecondShowTime,
                        ThirdShowTime = model.ThirdShowTime
                    };

                    
                     _ShowTimeDalSql.CreateShowTime(newShowTime);
                {

                }
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    // This is an existing ShowTime, so update it
                    var existingShowTime = _ShowTimeDalSql.GetShowViewModel(model.ShowTimeId);
                    if (existingShowTime == null)
                    {
                        return HttpNotFound();
                    }

                    // Update the properties of the existing ShowTime
                    existingShowTime.MovieId = model.MovieId;
                    existingShowTime.Title = model.Title;
                    existingShowTime.StartDateString = model.StartDateString;
                    existingShowTime.EndDateString = model.EndDateString;
                    existingShowTime.FirstShowTime = model.FirstShowTime;
                    existingShowTime.SecondShowTime = model.SecondShowTime;
                    existingShowTime.ThirdShowTime = model.ThirdShowTime;

                     _ShowTimeDalSql.UpdateShowTime(existingShowTime);

                    // Redirect to a success or index page
                    return RedirectToAction("Index");
                }
        }

        public ActionResult BookTicket(int showTimeId)
        {
            var existingShowTime = _ShowTimeDalSql.GetShowViewModel(showTimeId);
            var showtimes = _ShowTimeDalSql.GetTimesForKendoGrid(showTimeId);

            // showtime values as a list
            var showtimeList = showtimes.FirstOrDefault();

            // list of SelectListItem with three options
            var showtimeOptions = new List<Kendo.Mvc.UI.DropDownListItem>
    {
        new Kendo.Mvc.UI.DropDownListItem
        {
            Text = showtimeList.FirstShowTime,
            Value = showtimeList.FirstShowTime
        },
        new Kendo.Mvc.UI.DropDownListItem
        {
            Text = showtimeList.SecondShowTime,
            Value = showtimeList.SecondShowTime
        },
        new Kendo.Mvc.UI.DropDownListItem
        {
            Text = showtimeList.ThirdShowTime,
            Value = showtimeList.ThirdShowTime
        }
    };

            ViewBag.ShowtimeOptions = showtimeOptions;

            
            existingShowTime.SelectedShowtime = showtimeList.FirstShowTime;

            var seatTypes = _ShowTimeDalSql.GetSeatTypes();

            var seatTypeOptions = seatTypes.Select(seatType => new SelectListItem
            {
                Text = $"{seatType.TypeName} - Rs {seatType.Price}",
                Value = seatType.TypeName 
            }).ToList();
           

            // Default value for SelectedSeatType
            existingShowTime.SelectedSeatType = "Normal - Rs 200";

            
            existingShowTime.SeatTypeOptions = seatTypeOptions;

            return View(existingShowTime);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookTicket(ShowTimeViewModel model)
        {
           
                try
                {
                    var seatTypesId = _ShowTimeDalSql.GetSeatTypesId(model.SelectedSeatType);

                    // Update the properties of the existing model
                    model.MovieId = model.MovieId;
                    model.ShowDate = model.StartDate;
                    model.ShowTime = model.SelectedShowtime;
                    model.TypeName = model.SelectedSeatType;
                    model.SeatTypeId = seatTypesId;
                    model.PaymentStatus = 1;
                    model.PaymentAmount = _ShowTimeDalSql.CalculatePaymentAmount(seatTypesId, model.TicketCount);
                    model.NoOfTicket = model.TicketCount;
                    model.UserId = 1;

                    var seatTypes = _ShowTimeDalSql.GetSeatTypes();
                    var radioGroupOptions = seatTypes.Select(seatType => $"{seatType.TypeName} - Rs {seatType.Price}").ToArray();
                    ViewBag.SeatTypeOptions = radioGroupOptions;

                    _ShowTimeDalSql.SaveBookTicket(model);

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while saving the ticket.");
                }
           
            return View(model);
        }


        public ActionResult Delete(int ShowTimeId)
        {
            if (ShowTimeId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShowTimeViewModel view = _ShowTimeDalSql.Delete(ShowTimeId);

            if (view == null)
            {
                return HttpNotFound();
            }
            return View(view);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ShowTimeId)
        {
            //int loggedInUserId = (int)Session["UserId"];
            var deleteResult = _ShowTimeDalSql.DeleteConfirm(ShowTimeId);

            if (!deleteResult)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index"/*, new { userId = loggedInUserId }*/);
        }

        public ActionResult BookTicketIndex()
        {
            return View();
        }

        public ActionResult GetBookings([DataSourceRequest] DataSourceRequest request)
        {
            List<ShowTimeViewModel> bookings = _ShowTimeDalSql.GetBookTicketForKendoGrid();

            return Json(bookings.ToDataSourceResult(request));
        }

        public ActionResult MyBookings()
        {
            int userId = 1; 
            
            return View(userId);
        }


        public ActionResult GetMyBookings([DataSourceRequest] DataSourceRequest request)
        {
            //int loggedInUserId = (int)Session["UserId"];
            int loggedInUserId = 1;
            List<ShowTimeViewModel> watch = _ShowTimeDalSql.GetMyBookings(loggedInUserId);

            return Json(watch.ToDataSourceResult(request));
        }

        public ActionResult GetShowBookings([DataSourceRequest] DataSourceRequest request)
        {
            List<ShowTimeViewModel> book = _ShowTimeDalSql.GetBookings();

            return Json(book, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetShowTimings([DataSourceRequest] DataSourceRequest request)
        {
            List<ShowTimeViewModel> time = _ShowTimeDalSql.GetBookingsTime();

            return Json(time, JsonRequestBehavior.AllowGet);
        }



    }
}