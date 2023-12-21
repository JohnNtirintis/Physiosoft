using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.Logger;

namespace Physiosoft.Controllers
{
    public class PatientsController : Controller
    {
        private readonly PhysiosoftDbContext _context;

        public PatientsController(PhysiosoftDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.Patients.ToListAsync());
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error! Ex: {ex.Message}");
            }

        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID for patient was null in details view.");
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                NLogger.LogError($"Patient with id: {id} was not found in details view.");
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // TODO: Cant see last input and create button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,Firstname,Lastname,Telephone,Address,Vat,Ssn,RegNum,Notes,Email,HasReviewed,PatientIssue")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
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


                }
            } catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    ModelState.AddModelError("", "The entered value already exists. Please use a unique value.");
                }
                else
                {
                    NLogger.LogError($"Error occurred while creating a patient entity. Ex: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error occurred while creating a patient entity. Ex: {ex.Message}");
            }


            NLogger.LogInfo($"Returning patient Create view with errors.");
            return View(patient);
        }

        // GET: Patients/Edit/5
        // TODO  Cant see the last input-label
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID in Patient EDIT was null.");
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                NLogger.LogError($"Patient in EDIT with id {id} was NOT found.");
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientId,Firstname,Lastname,Telephone,Address,Vat,Ssn,RegNum,Notes,Email,HasReviewed,PatientIssue")] Patient patient)
        {
            if (id != patient.PatientId)
            {
                NLogger.LogError($"Didnt find Patient with ID: {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientId))
                    {
                        NLogger.LogError($"Didnt find Patient with ID: {id}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
                        //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                        NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                    }
                }

                return View(patient);
            }
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID in Patients Delete was null.");
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientId == id);
            if (patient == null)
            {
                NLogger.LogError($"Patient in Delete with id {id} was NOT found and returned null.");
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
            return ex.InnerException?.Message.Contains("unique constraint") ?? false;
        }
    }  
}