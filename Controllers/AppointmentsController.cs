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
    public class AppointmentsController : Controller
    {
        private readonly PhysiosoftDbContext _context;

        public AppointmentsController(PhysiosoftDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            try {
                var physiosoftDbContext = _context.Appointments.Include(a => a.Patient).Include(a => a.Physio);
                return View(await physiosoftDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error in calling appointments to list: {ex.Message}");
            }
            
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"Error! Given id in appointments was null.");
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physio)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                NLogger.LogError($"Error! Didnt find an appointment with id: {id}");
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId");
            ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,PatientID,PhysioID,AppointmentDate,DurationMinutes,AppointmentStatus,Notes,PatientIssuse,HasScans")] Appointment appointment)
        {

            ModelState.Remove("Physio");
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Prepare ViewData for dropdowns when ModelState is not valid
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId", appointment.PatientID);
            ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId", appointment.PhysioID);

            var errors = ModelState
                .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

            foreach (var error in errors)
            {
                foreach (var errorMessage in error.Errors)
                {
                    //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                    NLogger.LogError($"Key: { error.Key}, Error: { errorMessage}");
                }
            }

            return View(appointment);

        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"Error! Given ID in appointments edit was null");
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                NLogger.LogError($"Error! Didnt find an appointment with the ID: {id}");
                return NotFound();
            }
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId", appointment.PatientID);
            ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId", appointment.PhysioID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,PatientID,PhysioID,AppointmentDate,DurationMinutes,AppointmentStatus,Notes,PatientIssuse,HasScans")] Appointment appointment)
        {
            

            if (id != appointment.AppointmentID)
            {
                NLogger.LogError($"Error! Didnt find an appointment with an ID: {id}");
                return NotFound();
            }

            ModelState.Remove("Physio");
            ModelState.Remove("Patient");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentID))
                    {
                        NLogger.LogError($"Error! Didnt find an appointment with an ID: {id}");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // Prepare ViewData for dropdowns when ModelState is not valid
            ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId", appointment.PatientID);
            ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId", appointment.PhysioID);

            var errors = ModelState
                .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

            foreach (var error in errors)
            {
                foreach (var errorMessage in error.Errors)
                {
                    NLogger.LogError($"Key: { error.Key}, Error: { errorMessage}");
                    //Console.WriteLine($"Key: {error.Key}, Error: {errorMessage}");
                }
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"Error! ID given in appointments delete was null");
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Physio)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                NLogger.LogError($"Error! Didnt find an appointment with an ID: {id}");
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment != null)
                {
                    _context.Appointments.Remove(appointment);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                NLogger.LogError($"Error! Coudlnt delete appointment with id: {id}. Exception: {ex.Message}");
            }
            
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // TODO
            // Check if the exception is due to a unique constraint violation
            return ex.InnerException?.Message.Contains("unique constraint") ?? false;
        }
    }
}
