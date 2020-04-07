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
    public class ModelInformationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDeletableEntityRepository<ModelInformation> mdRepository;

        public ModelInformationsController(ApplicationDbContext context,
            IDeletableEntityRepository<ModelInformation> mdRepository)
        {
            _context = context;
            this.mdRepository = mdRepository;
        }

        // GET: Administration/ModelInformations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ModelsInformation.Include(m => m.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/ModelInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelInformation = await _context.ModelsInformation
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelInformation == null)
            {
                return NotFound();
            }

            return View(modelInformation);
        }

        // GET: Administration/ModelInformations/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Administration/ModelInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Age,Gender,Height,ModelType,Bust,Waist,Hips,InstagramUrl,FacebookUrl,Nationality,Ethnicity,Country,City,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] ModelInformation modelInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", modelInformation.UserId);
            return View(modelInformation);
        }

        // GET: Administration/ModelInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelInformation = await _context.ModelsInformation.FindAsync(id);
            if (modelInformation == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", modelInformation.UserId);
            return View(modelInformation);
        }

        // POST: Administration/ModelInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Age,Gender,Height,ModelType,Bust,Waist,Hips,InstagramUrl,FacebookUrl,Nationality,Ethnicity,Country,City,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] ModelInformation modelInformation)
        {
            if (id != modelInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modelInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelInformationExists(modelInformation.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", modelInformation.UserId);
            return View(modelInformation);
        }

        // GET: Administration/ModelInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelInformation = await _context.ModelsInformation
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modelInformation == null)
            {
                return NotFound();
            }

            return View(modelInformation);
        }

        // POST: Administration/ModelInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelInformation = await _context.ModelsInformation.FindAsync(id);
            this.mdRepository.Delete(modelInformation);
           await this.mdRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelInformationExists(int id)
        {
            return _context.ModelsInformation.Any(e => e.Id == id);
        }
    }
}
