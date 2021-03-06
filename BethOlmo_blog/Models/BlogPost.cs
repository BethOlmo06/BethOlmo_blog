﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace BethOlmo_blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string Abstract { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string MediaURL { get; set; }
        public bool Published { get; set; }

        //Navigational Properties
        public virtual ICollection<Comment>Comments { get; set; }
        public virtual ICollection<Category>Categories { get; set; }
        public BlogPost()
        {
            Comments = new HashSet<Comment>();
            Categories = new HashSet<Category>();
        }
    }
}