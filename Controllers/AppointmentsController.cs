using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.Logger;
using Physiosoft.Models;
using Microsoft.Data.SqlClient;

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
            try
            {
                var physiosoftDbContext = _context.Appointments.Include(a => a.Patient).Include(a => a.Physio);
                return View(await physiosoftDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error in calling appointments to list: {ex.Message}");
                return StatusCode(500); // Return a status code indicating an internal server error
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,PatientID,PhysioID,AppointmentDate,DurationMinutes,AppointmentStatus,Notes,PatientIssuse,HasScans")] Appointment appointment)
        {
            try
            {
                ModelState.Remove("Physio");
                ModelState.Remove("Patient");

                if (ModelState.IsValid)
                {
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Prepare ViewData for dropdowns when ModelState is not valid
                    ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId", appointment.PatientID);
                    ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId", appointment.PhysioID);

                    // Log errors
                    var errors = ModelState
                        .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                    foreach (var error in errors)
                    {
                        foreach (var errorMessage in error.Errors)
                        {
                            NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        }
                    }
                    return View(appointment);
                }
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    string duplicateColumn = GetDuplicateColumn(ex);
                    NLogger.LogError($"Duplicate value Error {duplicateColumn} occurred while editing an appointment entity. Ex: {ex.Message}");
                    ModelState.AddModelError(duplicateColumn, $"The {duplicateColumn} field is required.");
                    return View(appointment);
                }
                else
                {
                    NLogger.LogError($"Error in appointments create! Exception: {ex.Message}");
                    return StatusCode(500);
                }
            }     
            catch(Exception ex)
            {
                NLogger.LogError($"Error! Exception: {ex.Message}");
                return StatusCode(500);
            }       
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
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (IsUniqueConstraintViolation(ex))
                    {
                        string duplicateColumn = GetDuplicateColumn(ex);
                        NLogger.LogError($"Duplicate value {duplicateColumn} Error occurred while editing an appointment entity. Ex: {ex.Message}");
                        ModelState.AddModelError(duplicateColumn, $"The {duplicateColumn} field is required.");
                        return View(appointment);
                    }
                    else
                    {
                        NLogger.LogError($"Error in appointments edit! Exception: {ex.Message}");
                        return StatusCode(500);
                    }
                }
                catch (Exception ex)
                {
                    NLogger.LogError($"Error! Exception: {ex.Message}");
                    return StatusCode(500);
                }
            }
            else
            {
                // Prepare ViewData for dropdowns when ModelState is not valid
                ViewData["PatientID"] = new SelectList(_context.Patients, "PatientId", "PatientId", appointment.PatientID);
                ViewData["PhysioID"] = new SelectList(_context.Physios, "PhysioId", "PhysioId", appointment.PhysioID);

                var errors = ModelState
                    .Select(kvp => new { Key = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) });

                foreach (var error in errors)
                {
                    foreach (var errorMessage in error.Errors)
                    {
                        NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                    }
                }
                return View(appointment);
            }
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
                return View("Error", new ErrorViewModel { RequestId = id.ToString(), ErrorMessage = "Error occurred while trying to delete the appointment." });
            }
            
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                // Check if the exception is a SQL Server exception for a unique constraint violation
                return sqlEx.Number == 2627 || sqlEx.Number == 2601;
            }

            return false;
        }

        private string GetDuplicateColumn(DbUpdateException ex)
        {
            string? errorMessage = ex.InnerException?.Message;

            if (errorMessage != null)
            {
                string uniqueIndexPrefix = "with unique index '";
                int startIndex = errorMessage.IndexOf(uniqueIndexPrefix);

                if (startIndex != -1)
                {
                    startIndex += uniqueIndexPrefix.Length;
                    int endIndex = errorMessage.IndexOf("'", startIndex);

                    if (endIndex != -1)
                    {
                        string indexName = errorMessage.Substring(startIndex, endIndex - startIndex);

                        // the name of the column will always be the last index.
                        // i.e. IX_Appointments_PatientID
                        string[] parts = indexName.Split('_');
                        if (parts.Length >= 3)
                        {
                            return parts[2]; 
                        }
                    }
                }
            }
            // Default value if the column name could not be determined
            return "Unknown"; 
        }
    }
}
