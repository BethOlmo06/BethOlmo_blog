using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BethOlmo_blog.Helpers;
using BethOlmo_blog.Models;
using PagedList;
using PagedList.Mvc;

namespace BethOlmo_blog.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);

            int pageSize = 10; //specifies the number of posts per page
            int pageNumber = (page ?? 1); //?? null coalescing operator

            var model = blogList.OrderByDescending(b => b.Created).ToPagedList(pageNumber, pageSize);
            return View(model);

        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                    p.Body.Contains(searchStr) ||
                    p.Comments.Any(c => c.Body.Contains(searchStr) ||
                    c.Author.FirstName.Contains(searchStr) ||
                    c.Author.LastName.Contains(searchStr) ||
                    c.Author.DisplayName.Contains(searchStr) ||
                    c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details( string Slug)
        {
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Slug == Slug);
            
            if (blogPost == null)
            {
                return HttpNotFound();
            }    
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryIds = new MultiSelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Body,Abstract,MediaURL,Published")] BlogPost blogPost, List<int> categoryIds, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.BlogPosts.Any(b => b.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }

                blogPost.Slug = slug;
                blogPost.Created = DateTime.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();

                if (categoryIds != null)
                {
                    foreach (var categoryId in categoryIds)
                    {
                        db.CategoryBlogPosts.Add(new CategoryBlogPost
                        {
                            BlogPostId = blogPost.Id,
                            CategoryId = categoryId
                        });
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(blogPost);
        }



        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Slug,Body,Abstract,MediaURL,Created, Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.BlogPosts.Any(b => b.Slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                blogPost.Slug = slug;

                blogPost.Updated = DateTime.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }



        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
