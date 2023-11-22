using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class WatchViewModel
    {
        public int WatchListId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UserId { get; set; }

        public int WatchListMovieAssociationId { get; set; }
        public int MovieId { get; set; }
        public List<int> MovieIds { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public IEnumerable<MovieViewModel> MovieCollection { get; set; }
        public bool IsFavourite { get; set; }
        public List<WatchViewModel> WatchListCollection { get; set; }
        public string CheckedMovieIds { get; set; }

        public string MovieIdString { get; set; }
    }
}
