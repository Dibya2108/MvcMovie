using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModel
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public SelectList Languages { get; set; }
        public string LanguageName { get; set; }
        public int SelectedLanguageId { get; set; }
        public int LanguageID { get; set; }

        public bool IsDeleted { get; set; }
        public string UserName { get; set; }
        public bool AddToWatchlist { get; set; }
        public bool IsChecked { get; set; }
        public bool IsFavourite { get; set; }

        public string movieImageString { get; set; }
        public string MovieImage { get; set; }
    }
}
