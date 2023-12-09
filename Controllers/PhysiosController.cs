﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;

namespace Physiosoft.Controllers
{
    public class PhysiosController : Controller
    {
        private readonly PhysiosoftDbContext _context;

        public PhysiosController(PhysiosoftDbContext context)
        {
            _context = context;
        }

        // GET: Physios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Physios.ToListAsync());
        }

        // GET: Physios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physio = await _context.Physios
                .FirstOrDefaultAsync(m => m.PhysioId == id);
            if (physio == null)
            {
                return NotFound();
            }

            return View(physio);
        }

        // GET: Physios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Physios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhysioId,Firstname,Lastname,Telephone")] Physio physio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(physio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(physio);
        }

        // GET: Physios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physio = await _context.Physios.FindAsync(id);
            if (physio == null)
            {
                return NotFound();
            }
            return View(physio);
        }

        // POST: Physios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhysioId,Firstname,Lastname,Telephone")] Physio physio)
        {
            if (id != physio.PhysioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(physio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhysioExists(physio.PhysioId))
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
            return View(physio);
        }

        // GET: Physios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physio = await _context.Physios
                .FirstOrDefaultAsync(m => m.PhysioId == id);
            if (physio == null)
            {
                return NotFound();
            }

            return View(physio);
        }

        // POST: Physios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var physio = await _context.Physios.FindAsync(id);
            if (physio != null)
            {
                _context.Physios.Remove(physio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhysioExists(int id)
        {
            return _context.Physios.Any(e => e.PhysioId == id);
        }
    }
}
