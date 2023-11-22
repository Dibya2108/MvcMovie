using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IsActive { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
        public int UserTypeId { get; set; }
    }
}
