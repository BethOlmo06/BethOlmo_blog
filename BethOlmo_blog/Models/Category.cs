using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace BethOlmo_blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Nav
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        public Category()
        {
            BlogPosts = new HashSet<BlogPost>();
        }

    }
}