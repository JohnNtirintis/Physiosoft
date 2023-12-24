using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.Logger;

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
                return StatusCode(500); // Return a status code indicating an internal server error
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
        public IActionResult Create()
        {
            return View(new Physio());
        }

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
                            NLogger.LogError($"Key: {error.Key}, Error: {errorMessage}");
                        }
                    }
                    NLogger.LogInfo($"Returning Physio Create view with errors.");
                    return View(request);
                }
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    string duplicateColumn = GetDuplicateColumn(ex);
                    NLogger.LogError($"Duplicate value Error {duplicateColumn} occurred while editing a physio entity. Ex: {ex.Message}");
                    ModelState.AddModelError(duplicateColumn, $"The {duplicateColumn} field is required.");
                    return View(request);
                }
                else
                {
                    NLogger.LogError($"Error in physio create! Exception: {ex.Message}");
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error! Exception: {ex.Message}");
                return StatusCode(500);
            }
        }

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
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException ex)
                {
                    if (IsUniqueConstraintViolation(ex))
                    {
                        string duplicateColumn = GetDuplicateColumn(ex);
                        NLogger.LogError($"Duplicate value Error {duplicateColumn} occurred while editing a physio entity. Ex: {ex.Message}");
                        ModelState.AddModelError(duplicateColumn, $"The {duplicateColumn} field is required.");
                        return View(physio);
                    }
                    else
                    {
                        NLogger.LogError($"Error in physio create! Exception: {ex.Message}");
                        return StatusCode(500);
                    }
                }     
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "An Error has occured.");
                    NLogger.LogError($"Error occurred while editing a physios entity. Ex: {ex.Message}");
                    return StatusCode(500);
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
                return View(physio);
            }
        }

        // GET: Physios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var physio = new Physio();
            if (id == null)
            {
                NLogger.LogError($"ID in Physio Delete was null.");
                return NotFound();
            }
            try
            {
                physio = await _context.Physios
                .FirstOrDefaultAsync(m => m.PhysioId == id);
                if (physio == null)
                {
                    NLogger.LogError($"Physio in Delete with id {id} was NOT found and returned null.");
                    return NotFound();
                }
            }
            catch (DbUpdateException ex)
            {
                NLogger.LogError($"Database update exception in physios delete: {ex.Message}");
                return StatusCode(500);
            }
            catch (Exception ex) 
            {
                NLogger.LogError($"Error in physios delete! Exception: {ex.Message}");
                return StatusCode(500);
            }
            
            return View(physio);       
        }

        // POST: Physios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var physio = await _context.Physios.FindAsync(id);
                if (physio != null)
                {
                    _context.Physios.Remove(physio);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    NLogger.LogError($"Physio with id {id} in Delete was not found.");
                    return NotFound(); 
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                NLogger.LogError($"Concurrency error occurred in DeleteConfirmed: {ex.Message}");
                return StatusCode(500);
            }
            catch (DbUpdateException ex)
            {
                NLogger.LogError($"Database update error occurred in DeleteConfirmed: {ex.Message}");
                return StatusCode(500); 
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error in DeleteConfirmed: {ex.Message}");
                return StatusCode(500); 
            }


            return RedirectToAction(nameof(Index));
        }

        private bool PhysioExists(int id)
        {
            return _context.Physios.Any(e => e.PhysioId == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhysioLastName(int id)
        {
            var physio = await _context.Physios.FindAsync(id);
            if (physio != null)
            {
                return Ok(physio.Lastname);
            }
            else
            {
                return NotFound();
            }
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
                        // i.e. uq_physios_telephone
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
