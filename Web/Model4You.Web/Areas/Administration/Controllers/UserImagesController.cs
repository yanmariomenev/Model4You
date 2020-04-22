using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model4You.Data;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;

namespace Model4You.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class UserImagesController : AdministrationController
    {
        private readonly ApplicationDbContext _context;
        private readonly IDeletableEntityRepository<UserImage> imagesRepository;

        public UserImagesController(ApplicationDbContext context,
            IDeletableEntityRepository<UserImage> imagesRepository)
        {
            _context = context;
            this.imagesRepository = imagesRepository;
        }

        // GET: Administration/UserImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = this.imagesRepository
                .AllWithDeleted().OrderBy(x => x.IsDeleted);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/UserImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage =
                await this.imagesRepository
                    .AllWithDeleted()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
            if (userImage == null)
            {
                return NotFound();
            }

            return View(userImage);
        }

        // GET: Administration/UserImages/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Administration/UserImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageUrl,UserId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] UserImage userImage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userImage.UserId);
            return View(userImage);
        }

        // GET: Administration/UserImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var userImage = await _context.UserImages.FindAsync(id);
            var userImage = await this.imagesRepository.AllWithDeleted().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (userImage == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userImage.UserId);
            return View(userImage);
        }

        // POST: Administration/UserImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageUrl,UserId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] UserImage userImage)
        {
            if (id != userImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserImageExists(userImage.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userImage.UserId);
            return View(userImage);
        }

        // GET: Administration/UserImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userImage =
                await this.imagesRepository.AllWithDeleted().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (userImage == null)
            {
                return NotFound();
            }

            return View(userImage);
        }

        // POST: Administration/UserImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userImage = await _context.UserImages.FindAsync(id);
            userImage.IsDeleted = true;
            await this.imagesRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserImageExists(int id)
        {
            return _context.UserImages.Any(e => e.Id == id);
        }
    }
}
