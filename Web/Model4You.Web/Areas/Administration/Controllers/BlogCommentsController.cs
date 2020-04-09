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
    [Area("Administration")]
    public class BlogCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Administration/BlogComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogComments.Include(b => b.BlogContent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/BlogComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.BlogContent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }

        // GET: Administration/BlogComments/Create
        public IActionResult Create()
        {
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id");
            return View();
        }

        // POST: Administration/BlogComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogContentId,Name,Email,Content,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] BlogComment blogComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blogComment.BlogContentId);
            return View(blogComment);
        }

        // GET: Administration/BlogComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments.FindAsync(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blogComment.BlogContentId);
            return View(blogComment);
        }

        // POST: Administration/BlogComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogContentId,Name,Email,Content,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] BlogComment blogComment)
        {
            if (id != blogComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCommentExists(blogComment.Id))
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
            ViewData["BlogContentId"] = new SelectList(_context.BlogContents, "Id", "Id", blogComment.BlogContentId);
            return View(blogComment);
        }

        // GET: Administration/BlogComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.BlogContent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }

        // POST: Administration/BlogComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogComment = await _context.BlogComments.FindAsync(id);
            _context.BlogComments.Remove(blogComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogCommentExists(int id)
        {
            return _context.BlogComments.Any(e => e.Id == id);
        }
    }
}
