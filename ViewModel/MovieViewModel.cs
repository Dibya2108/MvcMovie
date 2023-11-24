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
        //---------Movie Table------------//
        public int Id { get; set; }

        [Required(ErrorMessage = "required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "required")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "required")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "required")]
        public decimal Price { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public bool IsDeleted { get; set; }
        public SelectList Languages { get; set; }
        public int SelectedLanguageId { get; set; }
        public string MovieImage { get; set; }

        //---------Language Table-----------//

        [Required(ErrorMessage = "required")]
        public string LanguageName { get; set; }        
        public int LanguageID { get; set; }



        
        
        public bool IsChecked { get; set; }
        
        
    }
}
