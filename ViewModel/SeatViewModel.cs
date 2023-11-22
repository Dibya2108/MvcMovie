using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class SeatViewModel
    {
        public int SeatTypeId { get; set; }
        public string TypeName { get; set;}
        public int Price { get; set; }
        public int BookticketId { get; set; }
        public int MovieId { get; set; }
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public int PaymentStatus { get; set; }
        public int NoOfTicket { get; set; }
        public int UserId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string SelectedShowtime { get; set; } 
        public string SelectedSeatType { get; set; }

        public int TicketQuantity { get; set; }

    }
}
