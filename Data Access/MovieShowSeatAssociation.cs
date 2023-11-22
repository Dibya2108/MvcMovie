using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class MovieShowSeatAssociation
    {
        [Key]
        public int MovieShowSeatAssociationId { get; set; }
        public int MovieId { get; set; }
        public int FirstShowSeatCount { get; set; }
        public int SecondShowSeatCount { get; set; }
        public int ThirdShowSeatCount { get; set; }

        public int SeatNormal { get; set; }
        public int SeatExecutive { get; set; }
        public int SeatPremium { get; set; }
        public int SeatVIP { get; set; }
    }
}
