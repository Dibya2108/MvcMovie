using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModel
{
    public class ShowTimeViewModel
    {
        public int ShowTimeId { get; set; }

        [Required(ErrorMessage = "required")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "required")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "required")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string FirstShowTime { get; set; }

        [Required(ErrorMessage = "required")]
        public string SecondShowTime { get; set; }

        [Required(ErrorMessage = "required")]
        public string ThirdShowTime { get; set; }


        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public bool IsHousefull { get; set; }
        public int MovieShowSeatAssociationId { get; set; }
        [Required(ErrorMessage = "required")]
        public int FirstShowSeatCount { get; set; }
        public int SecondShowSeatCount { get; set; }
        public int ThirdShowSeatCount { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }

        [Required(ErrorMessage = "required")]
        public string SelectedShowtime { get; set; } // Property for the selected showtime
        public string SelectedSeatType { get; set; }

        public int TicketQuantity { get; set; }

        public int SeatTypeId { get; set; }
        public string TypeName { get; set; }
        public int Price { get; set; }
        public int BookticketId { get; set; }
        
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public int PaymentStatus { get; set; }
        public int NoOfTicket { get; set; }
        
        public int PaymentAmount { get; set; }

        public string SelectedTime { get; set; }

        public string SelectedSeat { get; set; }
        public int TicketCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SelectedDate { get; set; }

        public List<SelectListItem> SeatTypeOptions { get; set; }
        
             
        

    }
}
