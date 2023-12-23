using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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
                return StatusCode(500); // Return a status code indicating an internal server error
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
                            NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        }
                    }

                    NLogger.LogInfo($"Returning patient Create view with errors.");
                    return View(patient);
                }
            } catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    string duplicateColumn = GetDuplicateColumn(ex);
                    NLogger.LogError($"Duplicate value {duplicateColumn} Error occurred while creating a patient entity. Ex: {ex.Message}");
                    ModelState.AddModelError("", "The entered value already exists. Please use a unique value.");
                    return View(patient);
                }
                else
                {
                    NLogger.LogError($"Error occurred while creating a patient entity. Ex: {ex.Message}");
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An Error has occured.");
                NLogger.LogError($"Error occurred while creating a patient entity. Ex: {ex.Message}");
                return StatusCode(500);
            }
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

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException ex)
                {
                    if (IsUniqueConstraintViolation(ex))
                    {
                        string duplicateColumn = GetDuplicateColumn(ex);
                        NLogger.LogError($"Duplicate value Error {duplicateColumn} occurred while editing a patient entity. Ex: {ex.Message}");
                        ModelState.AddModelError("", $"The entered value for {duplicateColumn} already exists. Please use a unique value.");
                        return View(patient);
                    }
                    else
                    {
                        NLogger.LogError($"Error occurred while editing a patient entity. Ex: {ex.Message}");
                        return StatusCode(500); // Return a status code indicating an internal server error
                    }
                }  
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An Error has occured.");
                    NLogger.LogError($"Error occurred while editing a patient entity. Ex: {ex.Message}");
                    return StatusCode(500); // Return a status code indicating an internal server error
                }
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

        /*[HttpGet]
        private async Task<IActionResult> GetPatientLastName(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                return Json(patient?.Lastname ?? string.Empty);
            }
            catch(Exception ex)
            {
                NLogger.LogError($"Error occurred in GetPatientsLastName: {ex.Message}");
                return Json(string.Empty);
            }
           
        }*/

        [HttpGet]
        public async Task<IActionResult> GetPatientLastName(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                return Ok(patient.Lastname);
            }
            else
            {
                return NotFound();
            }
        }


        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
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

                        // Extract the column name from the index name if possible
                        // This is dependent on your naming convention for unique indexes
                        // For example, if your unique indexes are named like "UQ_TableName_ColumnName"
                        string[] parts = indexName.Split('_');
                        if (parts.Length >= 3)
                        {
                            return parts[2]; // Assuming the third part is the column name
                        }
                    }
                }
            }

            return "Unknown"; // Default value if the column name could not be determined
        }

    }
}