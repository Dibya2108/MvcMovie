using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public int SelectedLanguageID { get; set; }

        [ForeignKey("SelectedLanguageID")]
        public virtual Languages Language { get; set; }
        public bool IsDeleted { get; set; }
        public Movies()
        {
            IsDeleted = false;
        }

    }
}
