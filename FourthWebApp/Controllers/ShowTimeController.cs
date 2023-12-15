using Data_Access;
using FourthWebApp.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using static Kendo.Mvc.UI.UIPrimitives;

namespace MvcMovie.Controllers
{
    public class ShowTimeController : BaseController
    {

        ShowTimeDalSql _ShowTimeDalSql = new ShowTimeDalSql();
        //AccountDalSql _accountDalSql = new AccountDalSql();
        // GET: ShowTime
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetShowTime([DataSourceRequest] DataSourceRequest request, bool showAll = false)
        {
            DateTime currentDate = DateTime.Now;
            List<ShowTimeViewModel> movie;

            if (showAll)
            {
                movie = _ShowTimeDalSql.GetShowTimeForKendoGrid();
            }
            else
            {
                movie = _ShowTimeDalSql.GetShowTimeForKendoGrid()
                    .Where(c => c.EndDate > currentDate)
                     .ToList() ;
            }


            return Json(movie.ToDataSourceResult(request));
        }

        public ActionResult GetMovies([DataSourceRequest] DataSourceRequest request)
        {
            List<MovieViewModel> movies = _ShowTimeDalSql.GetMovieForKendoGrid();

            return Json(movies, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Manage(int showTimeId = 0)
        {
            ShowTimeViewModel show = new ShowTimeViewModel();
            

                if(showTimeId != 0)
                {
                     show = _ShowTimeDalSql.GetShowViewModel(showTimeId); 
                     

                    if (show == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                    show.StartDateString = show.StartDate.ToString("MM/dd/yyyy");
                    show.EndDateString = show.EndDate.ToString("MM/dd/yyyy");
                    }
                
                }

                else
                {
                
                show.StartDateString = show.StartDate.ToString("MM/dd/yyyy");
                show.EndDateString = show.EndDate.ToString("MM/dd/yyyy");
                }
            return View(show);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ShowTimeViewModel model)
        {
            
            if (model.ShowTimeId == 0)
                {
                model.CreatedBy = (int)Session["UserId"];
                _ShowTimeDalSql.CreateShowTime(model);
                 
                }
                else
                {
                  _ShowTimeDalSql.UpdateShowTime(model);
                   
                }
            return JavaScript("CloseManageShowTime()");
        }

        public ActionResult BookTicket(int showTimeId)
        {
            var existingShowTime = _ShowTimeDalSql.GetShowViewModel(showTimeId);
            var showtimes = _ShowTimeDalSql.GetTimesForKendoGrid(showTimeId);

            // showtime values as a list
            var showtimeList = showtimes.FirstOrDefault();
            int userId = (int)Session["UserId"];
            //int userType = _accountDalSql.GetUserTypeByUserId(userId);

            //ViewBag.UserType = userType;
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
           
                
                    var seatTypesId = _ShowTimeDalSql.GetSeatTypesId(model.SelectedSeatType);

                    // Update the properties of the existing model
                    model.MovieId = model.MovieId;
                    //model.ShowDate = model.SelectedDate;
                    
                    model.ShowTime = model.SelectedShowtime;
                    model.TypeName = model.SelectedSeatType;
                    model.SeatTypeId = seatTypesId;
                    model.PaymentStatus = 1;
                    model.PaymentAmount = _ShowTimeDalSql.CalculatePaymentAmount(seatTypesId, model.TicketCount);
                    model.NoOfTicket = model.TicketCount;
                    model.UserId = (int)Session["UserId"];
            //int id = model.MovieId;
            //int userId = model.UserId;
            //var seatTypeName = model.TypeName;
            //var payment = model.PaymentStatus;
            //var amount = model.PaymentAmount;
            //var NoOfTicket = model.NoOfTicket; 

            var seatTypes = _ShowTimeDalSql.GetSeatTypes();
                    var radioGroupOptions = seatTypes.Select(seatType => $"{seatType.TypeName} - Rs {seatType.Price}").ToArray();
                    ViewBag.SeatTypeOptions = radioGroupOptions;

                    int id =_ShowTimeDalSql.SaveBookTicket(model);




            return JavaScript($"CloseBookShow({id})");

        }
        public ActionResult BookingSuccessfull(int id)
        {
            ViewBag.PrintId = id;
            return View();

        }


        public void PrintTictetToPdf(int id)
        {
            

            string htmlString = GetPrintTicketPdf(id);
            var generator = new NReco.PdfGenerator.HtmlToPdfConverter();
            
            var pdfBytes = generator.GeneratePdf(htmlString);
            Response.ContentType = "application/pdf";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.AddHeader("Content-Disposition", "Inline; filename=" + "PrintTicket.pdf");
            Response.BinaryWrite(pdfBytes);
            Response.Flush();
            Response.End();
        }

        private string GetPrintTicketPdf(int id)
        {
            ShowTimeViewModel view = new ShowTimeViewModel();
            view = _ShowTimeDalSql.GetBookingsInfo(id);
            throw new NotImplementedException();
        }

        

        public string DeleteShow(int showTimeId)
        {
            string msg = "";
            try
            {
                _ShowTimeDalSql.DeleteConfirm(showTimeId);
                msg = "Ok";
            }
            catch (Exception e)
            {
                msg = "ERROR : " + e.Message;
            }
            return msg;
        }


        public ActionResult BookTicketIndex()
        {
            int userId = (int)Session["UserId"];
            //int userType = _accountDalSql.GetUserTypeByUserId(userId);

            //ViewBag.UserType = userType;
            return View();
        }

        public ActionResult GetBookings([DataSourceRequest] DataSourceRequest request)
        {
            List<ShowTimeViewModel> bookings = _ShowTimeDalSql.GetBookTicketForKendoGrid();

            return Json(bookings.ToDataSourceResult(request));
        }

        public ActionResult MyBookings()
        {
            int userId = (int)Session["UserId"];
             
            
            return View(userId);
        }


        public ActionResult GetMyBookings([DataSourceRequest] DataSourceRequest request)
        {
            //int loggedInUserId = (int)Session["UserId"];
            int loggedInUserId = (int)Session["UserId"];
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