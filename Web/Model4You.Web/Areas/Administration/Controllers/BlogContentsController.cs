using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Models;

namespace Model4You.Web.Areas.Administration.Controllers
{
    public class BlogContentsController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public BlogContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Administration/BlogContents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogContents.Include(b => b.Blog);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/BlogContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogContent == null)
            {
                return NotFound();
            }

            return View(blogContent);
        }

        // GET: Administration/BlogContents/Create
        public IActionResult Create()
        {
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id");
            return View();
        }

        // POST: Administration/BlogContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Content,ImageUrl,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] BlogContent blogContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", blogContent.BlogId);
            return View(blogContent);
        }

        // GET: Administration/BlogContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents.FindAsync(id);
            if (blogContent == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", blogContent.BlogId);
            return View(blogContent);
        }

        // POST: Administration/BlogContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Content,ImageUrl,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] BlogContent blogContent)
        {
            if (id != blogContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogContentExists(blogContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "Id", "Id", blogContent.BlogId);
            return View(blogContent);
        }

        // GET: Administration/BlogContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogContent = await _context.BlogContents
                .Include(b => b.Blog)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogContent == null)
            {
                return NotFound();
            }

            return View(blogContent);
        }

        // POST: Administration/BlogContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogContent = await _context.BlogContents.FindAsync(id);
            _context.BlogContents.Remove(blogContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogContentExists(int id)
        {
            return _context.BlogContents.Any(e => e.Id == id);
        }
    }
}
