﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BethOlmo_blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdateReason { get; set; }

        //Navigational Properties
        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}