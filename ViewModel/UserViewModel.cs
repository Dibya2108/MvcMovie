using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name"), MaxLength(100)]
        public string UserName { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name"), MaxLength(255)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name"), MaxLength(255)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //  [Remote("EmailAlreadyExists", "Account", HttpMethod = "POST", ErrorMessage = "Email Address already registered with us.")]
        [Display(Name = "Email"), MaxLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        [Display(Name = "Password"), MaxLength(50)]
        public string Password { get; set; }
        public bool IsActive { get; set; }


        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }

        public SelectList UserTypes { get; set; }
    }
}
