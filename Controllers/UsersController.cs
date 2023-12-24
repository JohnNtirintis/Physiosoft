using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;
using Physiosoft.DTO.User;
using Physiosoft.Repisotories;
using Physiosoft.Logger;

namespace Physiosoft.Controllers
{
    // TODO RESOLVE USER REPOISTORY
    public class UsersController : Controller
    {
        private readonly PhysiosoftDbContext _context;
        private readonly IUserRepository _userRepository;

        public UsersController(PhysiosoftDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Users.ToListAsync());
            var users = await _context.Users.ToListAsync();
            var userDTOs = users.Select(users => new UserUtilDTO
            {
                UserId = users.UserId,
                Username = users.Username,
                Email = users.Email,
            }).ToList();

            return View(userDTOs);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"Error! Given ID was null");
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                NLogger.LogError($"Error! Didnt find user with ID: {id}");
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View(new UserSignupDTO());
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserSignupDTO request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool signupSuccess = await _userRepository.SignupUserAsync(request);
                    if (signupSuccess)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "User already exists.");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex))
                {
                    ModelState.AddModelError("", "The entered value already exists. Please use a unique value.");
                    NLogger.LogError($"Duplicate value error. Error: {ex.Message}");
                }
                else
                {
                    NLogger.LogError($"Error occurred while signing up a user entity. Ex: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                NLogger.LogError($"Error occurred while signing up a user entity. Ex: {ex.Message}");
            }

            return View(request);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"ID in User EDIT was null.");
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                NLogger.LogError($"User in EDIT with id {id} was NOT found.");
                return NotFound();
            }

            var userPatchDTO = new UserPatchDTO
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,
            };

            return View(userPatchDTO);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,Email")] User user)
        public async Task<IActionResult> Edit(int id, UserPatchDTO request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }

            var user = await _context.Users.FindAsync(id);
            if(user == null) 
            {
                NLogger.LogError($"User in EDIT with id {id} was NOT found.");
                return NotFound();
            }

            user.Username = request.Username!;
            user.Email = request.Email!;
            user.Password = request.Password!;

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    NLogger.LogError($"Errorm user with id: {user.UserId} doesnt exist!");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                NLogger.LogError($"Error! {ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
         
        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NLogger.LogError($"Given ID was null");
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                NLogger.LogError($"User in EDIT with id {id} was NOT found.");
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }else
            {
                NLogger.LogError($"User in EDIT with id {id} was NOT found.");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            // Check if the exception is due to a unique constraint violation
            return ex.InnerException?.Message.Contains("unique constraint") ?? false;
        }
    }
}
