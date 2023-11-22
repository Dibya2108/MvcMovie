using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class ShowTime
    {
        [Key]
        public int ShowTimeId { get; set; }
        public int MovieId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FirstShowTime { get; set; }
        public string SecondShowTime { get; set;}
        public string ThirdShowTime { get; set;}
        public bool IsHousefull { get; set; }
    }
}
