using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BethOlmo_blog.Models
{
    public class CategoryBlogPost
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public int CategoryId { get; set; }
    }
}