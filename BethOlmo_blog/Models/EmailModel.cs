using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BethOlmo_blog.Models
{
    public class EmailModel
    {
        //[Required, Display(Name = "FirstName")]
        //public string FirstName { get; set; }

        //[Required, Display(Name = "LastName")]
        //public string LastName { get; set; }

        [Required, Display(Name = "FromName")]
        public string FromName { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string FromEmail { get; set;}

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}