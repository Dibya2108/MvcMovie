using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class WatchListMovieAssociation
    {
        [Key]
        public int WatchListMovieAssociationId { get; set; }
        public int WatchListId { get; set; }
        public int MovieId { get; set; }
        public bool IsFavorite { get; set; }
    }
}
