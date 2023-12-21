using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.DTO.Physio;
using Physiosoft.DTO.User;
using Physiosoft.Logger;
using Physiosoft.Repisotories;

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
            try
            {
                return View(await _context.Physios.ToListAsync());
            } 
            catch (Exception ex)
            {
                NLogger.LogError($"Error! Ex: {ex.Message}");
            }
            
        }

        // GET: Physios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID for physio was null in details view.");
                return NotFound();
            }

            var physio = await _context.Physios
                .FirstOrDefaultAsync(m => m.PhysioId == id);

            if (physio == null)
            {
                NLogger.LogError($"Physio with id: {id} was not found in details view.");
                return NotFound();
            }

            NLogger.LogInfo($"Returning physio with id {id} in details view");
            return View(physio);
        }


        // GET: Physios/Create
        /*public IActionResult Create()
        {
            return View(new PhysioUtilDTO());
        }*/
        // TODO: CHECK IF IT WORKS DUE TO APPOINTMENTS MAPPING
        public IActionResult Create()
        {
            return View(new Physio());
        }

        // TODO: CRASHED WHEN ENTERED DUPLICATE PHONE NUMBER
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Physio request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var physio = new Physio
                    {
                        Firstname = request.Firstname,
                        Lastname = request.Lastname,
                        Telephone = request.Telephone
                    };

                    _context.Add(physio);
                    await _context.SaveChangesAsync();
                    NLogger.LogInfo($"Succesfully created and saved Physio, redirecting to action now.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                         .Select(e => e.ErrorMessage);

                    var errors = ModelState
                    .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                    foreach (var error in errors)
                    {
                        foreach (var errorMessage in error.Errors)
                        {
                            //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                            NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    ModelState.AddModelError("", "The entered value already exists. Please use a unique value.");
                }
                else
                {
                    NLogger.LogError($"Error occurred while creating a physio entity. Ex: {ex.Message}");
                }
            } catch (Exception ex)
            {
                NLogger.LogError($"Error occurred while creating a physio entity. Ex: {ex.Message}");
            }

            NLogger.LogInfo($"Returning Physio Create view with errors.");
            return View(request);
        }
            // GET: Physios/Create
            /* public IActionResult Create()
             {
                 return View();
             }

             Post
             [HttpPost]
             [ValidateAntiForgeryToken]
             public async Task<IActionResult> Create([Bind("Firstname,Lastname,Telephone")] Physio physio)
             {
                 if (ModelState.IsValid)
                 {
                     _context.Add(physio);
                     await _context.SaveChangesAsync();
                     return RedirectToAction(nameof(Index));
                 }
                 else
                 {
                     var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                          .Select(e => e.ErrorMessage);

                     var errors = ModelState
                     .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                     foreach (var error in errors)
                     {
                         foreach (var errorMessage in error.Errors)
                         {
                             Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                         }
                     }

                     return View(physio);
                 }
             }*/

            // GET: Physios/Edit/5
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID in Physio EDIT was null.");
                return NotFound();
            }

            var physio = await _context.Physios.FindAsync(id);

            if (physio == null)
            {
                NLogger.LogError($"Physio in EDIT with id {id} was NOT found.");
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
                NLogger.LogError($"Physio in EDIT with id {physio.PhysioId} doesnt match the given ID: {id}");
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
                        NLogger.LogError($"Physio in EDIT with id {physio.PhysioId} was NOT found.");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                } catch (Exception ex)
                {
                    NLogger.LogError($"Error! {ex.Message}");
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage);

                var errors = ModelState
                .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Errors)
                    {
                        NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                    }
                }

                return View(physio);
            }
        }

        // GET: Physios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID in Physio Delete was null.");
                return NotFound();
            }

            var physio = await _context.Physios
                .FirstOrDefaultAsync(m => m.PhysioId == id);
            if (physio == null)
            {
                NLogger.LogError($"Physio in Delete with id {id} was NOT found and returned null.");
                return NotFound();
            }

            else
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage);

                var errors = ModelState
                .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Errors)
                    {
                        NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                    }
                }

                return View(physio);
            }
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
                await _context.SaveChangesAsync();
            }
            else
            {
                NLogger.LogError($"Physio with id {id} in Delete wasnt found and returned null.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PhysioExists(int id)
        {
            return _context.Physios.Any(e => e.PhysioId == id);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
            return ex.InnerException?.Message.Contains("unique constraint") ?? false;
        }
    }
}
