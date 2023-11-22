using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class Languages
    {
        [Key]
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }

        public ICollection<Movies> Movies { get; set; }
    }
}
